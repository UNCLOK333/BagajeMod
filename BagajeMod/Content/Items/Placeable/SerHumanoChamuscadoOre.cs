using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Items.Placeable
{
    public class SerHumanoChamuscadoOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 58;

            // This ore can spawn in slime bodies like other pre-boss ores. (copper, tin, iron, etch)
            // It will drop in amount from 3 to 13.
            ItemID.Sets.OreDropsFromSlime[Type] = (1, 5);
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.SerHumanoChamuscadoOre>());
            Item.width = 12;
            Item.height = 12;
            Item.value = 3000;
        }
    }
}