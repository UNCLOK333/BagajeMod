using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BagajeMod.Content.Tiles;

namespace BagajeMod.Content.Items.Placeable
{
    public class CacaRosa : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.PlacedCacaRosa>());
            Item.width = 16; 
            Item.height = 14; 
            Item.value = Item.buyPrice(0, 0, 0, 50); 
            Item.rare = ItemRarityID.White; 
            Item.maxStack = 999; 
        }
    }
}
