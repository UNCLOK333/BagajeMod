using BagajeMod.Content.Items.Placeable;
using BagajeMod.Content.NPCs.Terry;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Items.Consumables
{
    public class RestosDeVida : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12; // Indica que es un ítem de invocación de jefe
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.value = 100;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossSpawners;
        }

        public override bool CanUseItem(Player player)
        {
            // Verificar si el jefe ya está invocado
            bool bossNotSpawned = !NPC.AnyNPCs(ModContent.NPCType<TerryBody>());

            // Obtener la posición del jugador en bloques
            float playerY = player.position.Y / 16f;

            // Definir las alturas mínima y máxima en bloques para la capa deseada
            float minLayer = Main.UnderworldLayer - 200f;
            float maxLayer = Main.UnderworldLayer;

            // Verificar si el jugador está dentro del rango de altura especificado
            bool withinLayer = playerY >= minLayer && playerY <= maxLayer;

            // Permitir el uso del ítem solo si el jefe no está invocado y el jugador está dentro de la capa especificada
            return bossNotSpawned && withinLayer;
        }


        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                // Reproducir sonido de invocación
                SoundEngine.PlaySound(SoundID.Roar, player.position);

                int type = ModContent.NPCType<TerryBody>();

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // Invocar jefe en Singleplayer o Servidor
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                }
                else
                {
                    // Enviar solicitud de invocación al servidor en Multiplayer
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SerHumanoChamuscadoOre>(), 10)
                .AddIngredient(ItemID.StoneBlock, 50)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
