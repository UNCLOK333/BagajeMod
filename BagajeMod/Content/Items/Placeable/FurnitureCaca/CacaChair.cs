using BagajeMod.Content.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Items.Placeable.FurnitureCaca
{
    public class CacaChair : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnitureCaca.CacaChair>());
            Item.width = 12;
            Item.height = 30;
            Item.value = 150;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<LadrilloDeCacaRosa>(), 4)
                .AddTile<Tiles.FurnitureCaca.BancodetrabajoCaca>()
                .Register();
        }
    }
}