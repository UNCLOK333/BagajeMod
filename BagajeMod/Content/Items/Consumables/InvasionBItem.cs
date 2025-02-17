using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BagajeMod.Event;

namespace BagajeMod.Content.Items.Consumables
{
    public class InvasionBItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 4;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 28;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Green;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            return !InvasionBSystem.EventIsOngoing;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                InvasionBSystem.TryStartEvent();
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
