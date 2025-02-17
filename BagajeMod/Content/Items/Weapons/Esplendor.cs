using BagajeMod.Content.Items.Placeable;
using BagajeMod.Content.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Items.Weapons
{
    public class Esplendor : ModItem 
    {
        public override void SetStaticDefaults()
        {
            
            ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
            
            ItemID.Sets.Spears[Item.type] = true;
        }

        public override void SetDefaults()
        {
            
            Item.rare = ItemRarityID.Pink; 
            Item.value = Item.sellPrice(silver: 10); 

            
            Item.useStyle = ItemUseStyleID.Shoot; 
            Item.useAnimation = 12; 
            Item.useTime = 2; 
            Item.UseSound = SoundID.Item71; 
            Item.autoReuse = true; 

            Item.damage = 9;
            Item.knockBack = 0.5f;
            Item.noUseGraphic = true; 
            Item.DamageType = DamageClass.Melee; 
            Item.noMelee = true; 

            Item.shootSpeed = 3.7f; 
            Item.shoot = ModContent.ProjectileType<EsplendorProyectile>();
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override bool? UseItem(Player player)
        {
            
            if (!Main.dedServ && Item.UseSound.HasValue)
            {
                SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
            }
            return null; 
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CacaRosa>(), 20)
                .AddTile<Tiles.FurnitureCaca.BancodetrabajoCaca>()
                .Register();
        }
    }
}
