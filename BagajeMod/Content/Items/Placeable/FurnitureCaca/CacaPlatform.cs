using Terraria;
using Terraria.ModLoader;

namespace BagajeMod.Content.Items.Placeable.FurnitureCaca
{
    public class CacaPlatform : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 200;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnitureCaca.CacaPlatform>());
            Item.width = 8;
            Item.height = 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ModContent.ItemType<LadrilloDeCacaRosa>(), 1)
                .AddTile<Tiles.FurnitureCaca.BancodetrabajoCaca>()
                .Register();
        }
    }
}