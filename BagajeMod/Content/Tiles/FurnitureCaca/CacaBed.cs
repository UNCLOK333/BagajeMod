using BagajeMod.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BagajeMod.Content.Tiles.FurnitureCaca
{
    public class CacaBed : ModTile // Clase que representa un tile personalizado, en este caso, una cama
    {
        public const int NextStyleHeight = 38; 
        // Constante que define la altura entre estilos de este tile, calculada sumando todas las alturas y ajustes de padding.

        public override void SetStaticDefaults()
        {
            // Configuración de propiedades para el tile
            Main.tileFrameImportant[Type] = true; // Indica que el tile depende de sus frames (es decir, se dibuja dependiendo de su estado)
            Main.tileLavaDeath[Type] = true; // Este tile se destruye al entrar en contacto con lava
            TileID.Sets.HasOutlines[Type] = true; // Permite que se muestre un contorno cuando el jugador pasa el cursor por encima
            TileID.Sets.CanBeSleptIn[Type] = true; // Define que este tile puede ser usado como una cama para dormir
            TileID.Sets.InteractibleByNPCs[Type] = true; // Los NPCs pueden interactuar con este tile
            TileID.Sets.IsValidSpawnPoint[Type] = true; // Este tile puede ser un punto de spawn válido
            TileID.Sets.DisableSmartCursor[Type] = true; // Desactiva el uso del cursor inteligente en este tile

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair); 
            // Se agrega a la lista de objetos que cuentan como "sillas" para las habitaciones de NPCs

            DustType = ModContent.DustType<Sparkle>(); 
            // Define el tipo de polvo que se generará al interactuar con el tile
            AdjTiles = new int[] { TileID.Beds }; 
            // Define que este tile es funcionalmente similar a las camas estándar

            // Configuración de colocación del tile
            TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2); 
            // Copia las propiedades del estilo de cama 4x2 predeterminado
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 }; 
            // Define las alturas de las coordenadas para las filas del tile
            TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2); 
            // Ajusta el padding de las coordenadas del tile
            TileObjectData.addTile(Type); // Agrega este tile al conjunto de datos de tiles

            // Configuración del mapa
            AddMapEntry(new Color(191, 142, 111), Language.GetText("ItemName.Bed")); 
            // Agrega este tile al mapa con un color específico y un nombre localizado
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            // Permite el uso de interacción inteligente en este tile
            return true;
        }

        public override void ModifySmartInteractCoords(ref int width, ref int height, ref int frameWidth, ref int frameHeight, ref int extraY)
        {
            // Ajusta las coordenadas de interacción inteligente para este tile
            width = 2; // Ancho del tile
            height = 2; // Altura del tile
        }

        public override void ModifySleepingTargetInfo(int i, int j, ref TileRestingInfo info)
        {
            // Modifica la posición visual del jugador al dormir en la cama
            info.VisualOffset.Y += 4f; 
            // Baja un poco al jugador porque la cama personalizada es más baja que las estándar
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            // Define la cantidad de polvo que se genera al interactuar con el tile
            num = 1;
        }

        public override bool RightClick(int i, int j)
        {
            // Maneja la interacción al hacer clic derecho sobre el tile
            Player player = Main.LocalPlayer; // Obtiene al jugador local
            Tile tile = Main.tile[i, j]; // Obtiene los datos del tile en las coordenadas dadas
            int spawnX = (i - (tile.TileFrameX / 18)) + (tile.TileFrameX >= 72 ? 5 : 2); 
            // Calcula la posición X de spawn dependiendo del frame del tile
            int spawnY = j + 2; // Calcula la posición Y de spawn

            if (tile.TileFrameY % NextStyleHeight != 0)
            {
                spawnY--; // Ajusta la posición Y si el frame no coincide con el estilo base
            }

            if (!Player.IsHoveringOverABottomSideOfABed(i, j))
            {
                // Si el jugador está en rango y no está interactuando con la parte inferior de la cama
                if (player.IsWithinSnappngRangeToTile(i, j, PlayerSleepingHelper.BedSleepingMaxDistance))
                {
                    player.GamepadEnableGrappleCooldown(); // Habilita un cooldown para evitar bugs al interactuar
                    player.sleeping.StartSleeping(player, i, j); // Activa el estado de dormir
                }
            }
            else
            {
                // Si se hace clic en la parte inferior, ajusta el punto de spawn del jugador
                player.FindSpawn();

                if (player.SpawnX == spawnX && player.SpawnY == spawnY)
                {
                    player.RemoveSpawn(); 
                    // Elimina el punto de spawn si ya está configurado en este tile
                    Main.NewText(Language.GetTextValue("Game.SpawnPointRemoved"), byte.MaxValue, 240, 20); 
                    // Muestra un mensaje de que el punto de spawn fue eliminado
                }
                else if (Player.CheckSpawn(spawnX, spawnY))
                {
                    player.ChangeSpawn(spawnX, spawnY); 
                    // Cambia el punto de spawn al nuevo calculado
                    Main.NewText(Language.GetTextValue("Game.SpawnPointSet"), byte.MaxValue, 240, 20); 
                    // Muestra un mensaje de que el punto de spawn fue configurado
                }
            }

            return true;
        }

        public override void MouseOver(int i, int j)
        {
            // Muestra el ícono del cursor cuando el jugador pasa el ratón sobre el tile
            Player player = Main.LocalPlayer;

            if (!Player.IsHoveringOverABottomSideOfABed(i, j))
            {
                if (player.IsWithinSnappngRangeToTile(i, j, PlayerSleepingHelper.BedSleepingMaxDistance))
                {
                    // Si el jugador está en rango para interactuar
                    player.noThrow = 2; // Desactiva lanzar objetos
                    player.cursorItemIconEnabled = true; // Habilita el ícono del cursor
                    player.cursorItemIconID = ItemID.SleepingIcon; // Ícono predeterminado para camas
                }
            }
            else
            {
                player.noThrow = 2;
                player.cursorItemIconEnabled = true;
                player.cursorItemIconID = ModContent.ItemType<Items.Placeable.FurnitureCaca.CacaBed>(); 
                // Muestra un ícono personalizado del mod si el cursor está en la parte inferior del tile
            }
        }
    }
}