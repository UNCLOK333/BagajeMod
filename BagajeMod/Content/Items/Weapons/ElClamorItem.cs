using BagajeMod.Content.Projectiles;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using BagajeMod.Content.Buffs;
using BagajeMod.Content.Projectiles.Minions;
using Microsoft.Xna.Framework;
using BagajeMod.Content.Items.Placeable;

namespace BagajeMod.Content.Items.Weapons
{
    public class ElClamorItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;

            ItemID.Sets.StaffMinionSlotsRequired[Type] = 1f; 
        }

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.knockBack = 3f;
            Item.mana = 10;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing; 
            Item.value = Item.sellPrice(copper: 188);
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item44;


            Item.noMelee = true; 
            Item.DamageType = DamageClass.Summon; 
            Item.buffType = ModContent.BuffType<ElClamorBuff>();
            Item.shoot = ModContent.ProjectileType<ElClamorMinion>();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CacaRosa>(), 6)
                .AddIngredient(ItemID.Wood, 3) // nomas pa chingar
                .AddTile<Tiles.FurnitureCaca.BancodetrabajoCaca>()
                .Register();
        }
    }
}