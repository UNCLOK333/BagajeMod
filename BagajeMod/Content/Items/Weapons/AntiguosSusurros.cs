using BagajeMod.Content.Items.Placeable;
using Microsoft.Build.Evaluation;
using Terraria; // Importa las clases principales del juego Terraria.
using Terraria.ID; // Importa identificadores de elementos y proyectiles predeterminados.
using Terraria.ModLoader; // Importa herramientas para crear mods en Terraria.

namespace BagajeMod.Content.Items.Weapons // Define el espacio de nombres donde se encuentra este ítem.
{
    public class AntiguosSusurros : ModItem // Clase que define un nuevo ítem basado en "ModItem".
    {
        // Configura las propiedades del ítem.
        public override void SetDefaults()
        {
            // Modifica cualquiera de estos valores según sea necesario, pero se recomienda mantener ciertos valores como `useStyle`, `noUseGraphic` y `noMelee`.

            // Propiedades comunes.
            Item.rare = ItemRarityID.Pink; // Define la rareza del ítem (Rosa).
            Item.value = Item.sellPrice(silver: 5); // Precio de venta en 5 monedas de plata.
            Item.maxStack = 999; // Cantidad máxima que se puede apilar en el inventario.

            // Propiedades de uso.
            Item.useStyle = ItemUseStyleID.Swing; // Estilo de uso: similar a lanzar (balanceo).
            Item.useAnimation = 25; // Duración de la animación de uso (en ticks, 25 ticks = ~0.42 segundos).
            Item.useTime = 25; // Tiempo entre usos del ítem (en ticks, igual a la animación).
            Item.UseSound = SoundID.Item1; // Sonido que se reproduce al usar el ítem.
            Item.autoReuse = true; // Permite usar el ítem automáticamente si se mantiene presionado.
            Item.consumable = true; // El ítem se consume al usarse.

            // Propiedades del arma.
            Item.damage = 9; // Daño que inflige el arma.
            Item.knockBack = 0.5f; // Retroceso que aplica en los enemigos.
            Item.noUseGraphic = true; // El sprite del ítem no se muestra cuando se utiliza.
            Item.noMelee = true; // El daño no proviene del golpe del ítem, sino del proyectil.
            Item.DamageType = DamageClass.Throwing; // El tipo de daño es "a distancia".

            // Propiedades del proyectil.
            Item.shootSpeed = 9f; // Velocidad del proyectil (en píxeles por cuadro).
            Item.shoot = ModContent.ProjectileType<Projectiles.AntiguosSusurrosProyectile>(); // Proyectil que se lanza al usar el ítem.
        }

        // Define las recetas para crear este ítem.
        public override void AddRecipes()
        {
            CreateRecipe(30)
                .AddIngredient(ModContent.ItemType<CacaRosa>(), 20)
                .AddTile<Tiles.FurnitureCaca.BancodetrabajoCaca>()
                .Register();
        }
    }
}
