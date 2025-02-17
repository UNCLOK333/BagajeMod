using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Items.Placeable.FurnitureCaca
{
    public class CacaToilet : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnitureCaca.CacaToilet>());
            Item.width = 16;
            Item.height = 24;
            Item.value = 150;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<LadrilloDeCacaRosa>(), 6)
                .AddTile<Tiles.FurnitureCaca.BancodetrabajoCaca>()
                .Register();
        }
    }
}