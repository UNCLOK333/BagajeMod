using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Items.Placeable.FurnitureCaca
{
    public class BancodetrabajoCaca : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnitureCaca.BancodetrabajoCaca>());
            Item.width = 28;
            Item.height = 14;
            Item.value = 150;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.CraftingObjects;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WorkBench)
                .AddIngredient(ModContent.ItemType<CacaRosa>(), 8)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}