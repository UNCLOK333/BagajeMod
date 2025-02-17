using BagajeMod.Content.Items.Placeable;
using BagajeMod.Content.Projectiles;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Items.Tools
{
    public class Cacaña : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanFishInLava[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WoodFishingPole);

            Item.fishingPole = 13;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<Projectiles.Floton>();
        }

        public override void HoldItem(Player player)
        {
            player.accFishingLine = true;
        }
        public override void ModifyFishingLine(Projectile bobber, ref Vector2 lineOriginOffset, ref Color lineColor)
        {
            // Change these two values in order to change the origin of where the line is being drawn.
            // This will make it draw 43 pixels right and 30 pixels up from the player's center, while they are looking right and in normal gravity.
            lineOriginOffset = new Vector2(43, -30);

            // Sets the fishing line's color. Note that this will be overridden by the colored string accessories.
            if (bobber.ModProjectile is Floton exampleBobber)
            {
                // ExampleBobber has custom code to decide on a line color.
                lineColor = exampleBobber.FishingLineColor;
            }
            else
            {
                // If the bobber isn't ExampleBobber, a Fishing Bobber accessory is in effect and we use DiscoColor instead.
                lineColor = Main.DiscoColor;
            }
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