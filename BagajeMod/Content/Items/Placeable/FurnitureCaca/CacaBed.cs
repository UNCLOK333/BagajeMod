using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BagajeMod.Content.Tiles.FurnitureCaca;

namespace BagajeMod.Content.Items.Placeable.FurnitureCaca
{
    public class CacaBed : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnitureCaca.CacaBed>());
            Item.width = 28;
            Item.height = 20;
            Item.value = 2000;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<LadrilloDeCacaRosa>(), 15)
                .AddIngredient(ItemID.Silk, 5)
                .AddTile<Tiles.FurnitureCaca.BancodetrabajoCaca>()
                .Register();
        }
    }
}