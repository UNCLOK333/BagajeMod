using BagajeMod.Content.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BagajeMod.Content.Tiles
{
    public class TorretaColocableTile : ModTile
    {
        private int shootTimer = 0;
        private const int ShootInterval = 60; // Intervalo de disparo en ticks (60 ticks = 1 segundo)

        public override void SetStaticDefaults()
        {
            // Configuración básica del tile
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;

            // Hitbox de 4x3 bloques
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1);
            TileObjectData.addTile(Type);

            // Nombre y color en el mapa
            //ModTranslation name = CreateMapEntryName();
            //name.SetDefault("Torreta Colocable");
            //AddMapEntry(new Color(200, 200, 200), name);

            // Polvo y sonido al destruirse
            DustType = DustID.Iron;
            HitSound = SoundID.Tink;
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];

            // 1. Calcular posición real del cofre (esquina superior izquierda)
            int left = i - (tile.TileFrameX % 36 / 18);
            int top = j - (tile.TileFrameY % 38 / 18);

            // 2. Verificar si el cofre está bloqueado
            if (IsLockedChest(left, top))
            {
                // 3. Intentar desbloquear (solo de noche)
                if (!Main.dayTime && UnlockChest(left, top, out short frameAdjustment, out int dustType))
                {
                    // 4. Ajustar frames y partículas
                    AdjustChestFrames(left, top, frameAdjustment);
                    SpawnUnlockParticles(left, top, dustType);
                    SoundEngine.PlaySound(SoundID.Unlock);

                    // 5. Forzar actualización del tile inmediatamente
                    WorldGen.SquareTileFrame(left, top, true);
                    NetMessage.SendTileSquare(-1, left, top, 2, 2);

                    return true;
                }
                return false;
            }

            // 6. Abrir/cerrar cofre normalmente
            int chestIndex = Chest.FindChest(left, top);
            ToggleChestUI(player, left, top, chestIndex);
            return true;
        }

        // ======= Métodos auxiliares =======
        private bool UnlockChest(int left, int top, out short frameAdjustment, out int dustType)
        {
            frameAdjustment = -36;
            dustType = DustID.GoldCoin;
            return true; // Siempre desbloquea si es de noche
        }

        private void AdjustChestFrames(int left, int top, short adjustment)
        {
            for (int x = left; x < left + 2; x++)
            {
                for (int y = top; y < top + 2; y++)
                {
                    Main.tile[x, y].TileFrameX += adjustment;
                }
            }
        }

        private void SpawnUnlockParticles(int left, int top, int dustType)
        {
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(new Vector2(left * 16, top * 16), 32, 32, dustType);
            }
        }

        private void ToggleChestUI(Player player, int left, int top, int chestIndex)
        {
            // 7. Lógica mejorada para abrir/cerrar
            if (player.chest == chestIndex)
            {
                player.chest = -1;
                SoundEngine.PlaySound(SoundID.MenuClose);
                Recipe.FindRecipes();
            }
            else
            {
                player.chest = chestIndex;
                Main.recBigList = false;
                SoundEngine.PlaySound(SoundID.MenuOpen);

                // 8. Forzar cierre del inventario primero
                player.CloseSign();
                player.SetTalkNPC(-1);
                Main.npcChatCornerItem = 0;

                // 9. Sincronización en multiplayer
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.SyncChestItem, -1, -1, null, left, top);
                }
            }
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ModContent.ItemType<TorretaColocableItem>();
            player.cursorItemIconText = "";
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }



        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            // Suelta el ítem al destruirse
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 64, 48, ModContent.ItemType<TorretaColocableItem>());
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            // Lógica de disparo
            if (closer)
            {
                shootTimer++;
                if (shootTimer >= ShootInterval)
                {
                    shootTimer = 0;
                    ShootDart(i, j);
                }
            }
        }

        private void ShootDart(int i, int j)
        {
            Player player = Main.LocalPlayer;
            int dartType = ItemID.PoisonDart; // Tipo de dardo que se utilizará

            // Buscar dardos en el inventario del jugador
            for (int slot = 0; slot < player.inventory.Length; slot++)
            {
                Item item = player.inventory[slot];
                if (item.type == dartType && item.stack > 0)
                {
                    // Dirección de disparo (basada en la orientación del tile)
                    Tile tile = Main.tile[i, j];
                    int direction = (tile.TileFrameX / 72) == 0 ? 1 : -1;

                    // Posición de disparo (centro de la torreta)
                    Vector2 position = new Vector2(i * 16 + 32, j * 16 + 24);

                    // Velocidad y tipo de proyectil
                    Vector2 velocity = new Vector2(direction * 10f, 0f);
                    int projectileType = ProjectileID.PoisonDart;
                    int damage = 30;
                    float knockback = 2f;

                    // Crear el proyectil
                    Projectile.NewProjectile(new EntitySource_TileBreak(i, j), position, velocity, projectileType, damage, knockback, Main.myPlayer);

                    // Reducir la cantidad de dardos en el inventario
                    item.stack--;

                    // Sonido de disparo
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item17, position);

                    return; // Salir del bucle después de disparar
                }
            }

            // Si no se encontraron dardos, opcionalmente puedes agregar una lógica aquí
            // por ejemplo, desactivar la torreta o mostrar un mensaje al jugador.
        }


    }
}