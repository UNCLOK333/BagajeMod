using BagajeMod.Content.Items.Placeable;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Items.Weapons
{
    public class DelCosmos : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToStaff(ProjectileID.BlackBolt, 7, 20, 11);
            Item.width = 30;
            Item.height = 28;
            Item.UseSound = SoundID.Item71;

            // A special method that sets the damage, knockback, and bonus critical strike chance.
            // This weapon has a crit of 32% which is added to the players default crit chance of 4%
            Item.SetWeaponValues(25, 6, 32);

            Item.SetShopValues(ItemRarityColor.LightRed4, 10000);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CacaRosa>(), 20)
                .AddTile<Tiles.FurnitureCaca.BancodetrabajoCaca>()
                .Register();
        }

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            if (player.statLife < player.statLifeMax2 / 2)
            {
                mult *= 0.5f; 
            }
        }
    }
}