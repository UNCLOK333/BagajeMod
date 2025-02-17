using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Items.Weapons
{
    public class TorretaDeDardosMitski : ModItem
    {
        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
            Item.width = 66; // Ancho de la textura
            Item.height = 36; // Alto de la textura
            Item.damage = 30; // Daño base
            Item.knockBack = 2f; // Fuerza de retroceso
            Item.useTime = 6; // Tiempo entre usos (en ticks)
            Item.useAnimation = 6; // Duración de la animación (en ticks)
            Item.useStyle = ItemUseStyleID.Shoot; // Estilo de uso (arma de rango)
            Item.noMelee = true; // No hace daño cuerpo a cuerpo
            Item.value = Item.buyPrice(gold: 2); // Valor en oro
            Item.rare = ItemRarityID.Pink; // Rareza
            Item.UseSound = SoundID.Item17; // Sonido al usar
            Item.autoReuse = true; // Se puede usar continuamente
            Item.shoot = ProjectileID.CrystalDart; // Tipo de proyectil que dispara
            Item.shootSpeed = 16f; // Velocidad del proyectil
            Item.useAmmo = AmmoID.Dart; // Munición que usa (dardos)
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2f, -2f); // Desplaza el arma ligeramente hacia arriba y la derecha.
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<FragmentoDeAlma>(), 20)
                .AddIngredient(ModContent.ItemType<Placeable.SerHumanoChamuscadoOre>(), 30)
                .AddIngredient(ItemID.UnicornHorn, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}