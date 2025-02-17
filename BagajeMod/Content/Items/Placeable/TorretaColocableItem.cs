using BagajeMod.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Items.Placeable
{
    public class TorretaColocableItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Torreta Colocable");
            //Tooltip.SetDefault("Una torreta que dispara dardos automáticamente.\n'¡Pum, pum, pum!'");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 22;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Pink;
            Item.createTile = ModContent.TileType<Tiles.TorretaColocableTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TorretaDeDardosMitski>(), 1)
                .AddIngredient(ItemID.IronBar, 69)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}