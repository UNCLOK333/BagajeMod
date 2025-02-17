using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BagajeMod.Content.Items.Placeable;

namespace BagajeMod.Content.Items.Weapons
{
    public class LlamaEterna : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 23;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 1;
            Item.width = 46;
            Item.height = 48;

            Item.value = Item.sellPrice(silver: 8, copper: 25);
            Item.rare = ItemRarityID.Green;

            Item.useTime = 30;
            Item.useAnimation = 30;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(silver: 9);
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }
        public override bool MeleePrefix()
        {
            return true; // return true to allow weapon to have melee prefixes (e.g. Legendary)
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CacaRosa>(), 5) 
                .AddIngredient(ItemID.Wood, 3) // nomas pa chingar
                .AddTile<Tiles.FurnitureCaca.BancodetrabajoCaca>()
                .Register();
        }

    }

}
