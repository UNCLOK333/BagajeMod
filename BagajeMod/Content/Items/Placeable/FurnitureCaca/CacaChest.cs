using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Items.Placeable.FurnitureCaca
{
    public class CacaChest : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnitureCaca.CacaChest>());
            // Item.placeStyle = 1; // Use this to place the chest in its locked style
            Item.width = 26;
            Item.height = 22;
            Item.value = 500;
        }

        
    }

    public class ExampleChestKey : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3; // Biome keys usually take 1 item to research instead.
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GoldenKey);
        }
    }
}
