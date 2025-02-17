using BagajeMod.Content.EmoteBubbles;
using BagajeMod.Content.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Items.Tools
{
    public class PicodeCacaRosa : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(silver: 16);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.pick = 56;
            Item.attackSpeedOnlyAffectsWeaponAnimation = true;
        }
        public override void UseAnimation(Player player)
        {
            if (Main.myPlayer == player.whoAmI && player.ItemTimeIsZero && Main.rand.NextBool(60))
            {
                EmoteBubble.MakePlayerEmote(player, ModContent.EmoteBubbleType<CacaEmote>());
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CacaRosa>(), 18)
                .AddIngredient(ItemID.Wood, 10) // nomas pa chingar
                .AddTile<Tiles.FurnitureCaca.BancodetrabajoCaca>()
                .Register();
        }
    }
}