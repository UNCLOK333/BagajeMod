using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Items.Placeable.FurnitureCaca
{
    public class CacaDresser : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnitureCaca.CacaDresser>());

            Item.width = 26;
            Item.height = 22;
            Item.value = 500;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<LadrilloDeCacaRosa>(), 16)
                .AddTile<Tiles.FurnitureCaca.BancodetrabajoCaca>()
                .Register();
        }
    }
}