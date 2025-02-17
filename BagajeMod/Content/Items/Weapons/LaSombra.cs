using BagajeMod.Content.Items.Placeable;
using BagajeMod.Content.Projectiles; // Importa la ruta de los proyectiles personalizados del mod.
using Microsoft.Xna.Framework; // Proporciona estructuras matemáticas y gráficas como Vector2.
using Terraria; // Importa el núcleo del juego Terraria.
using Terraria.Audio; // Maneja los sonidos en Terraria.
using Terraria.DataStructures; // Contiene estructuras relacionadas con datos y lógica del juego.
using Terraria.ID; // Proporciona identificadores para objetos, proyectiles, etc.
using Terraria.ModLoader; // Herramientas principales para la creación de mods en Terraria.

namespace BagajeMod.Content.Items.Weapons
{
    // Define un arma personalizada basada en "ModItem".
    public class LaSombra : ModItem
    {
        // Configura las propiedades predeterminadas del arma.
        public override void SetDefaults()
        {
            // Propiedades generales del objeto.
            Item.width = 62; // Ancho del objeto en píxeles.
            Item.height = 32; // Altura del objeto en píxeles.
            Item.scale = 0.75f; // Escala del tamaño del objeto.
            Item.rare = ItemRarityID.Green; // Rango de rareza, se usa para el color del nombre.

            // Propiedades de uso.
            Item.useTime = 20; // Tiempo en ticks para usar el objeto (60 ticks = 1 segundo).
            Item.useAnimation = 8; // Duración de la animación de uso en ticks.
            Item.useStyle = ItemUseStyleID.Shoot; // Estilo de uso (disparar, golpear, etc.).
            Item.autoReuse = true; // Permite mantener el clic para usar automáticamente.

            // Configura el sonido de uso.
            Item.UseSound = new SoundStyle($"{nameof(BagajeMod)}/Assets/Sounds/Items/Guns/ExampleGun")
            {
                Volume = 0.4f, // Volumen del sonido.
                PitchVariance = 0.3f, // Variación de tono aleatoria.
                MaxInstances = 1, // Máximo número de instancias de este sonido que pueden reproducirse simultáneamente.
            };

            // Propiedades del arma.
            Item.DamageType = DamageClass.Ranged; // Define el tipo de daño como "a distancia".
            Item.damage = 20; // Daño base del arma.
            Item.knockBack = 5f; // Retroceso al impactar enemigos.
            Item.noMelee = true; // Evita que la animación del arma cause daño cuerpo a cuerpo.

            // Propiedades específicas de armas de fuego.
            Item.shoot = ProjectileID.PinkLaser; // Por defecto, todos los cañones vanilla usan este proyectil.
            Item.shootSpeed = 6f; // Velocidad del proyectil en píxeles por frame.
            Item.useAmmo = AmmoID.Bullet; // Tipo de munición que usa el arma (en este caso, balas).
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CacaRosa>(), 23)
                .AddTile<Tiles.FurnitureCaca.BancodetrabajoCaca>()
                .Register();
        }

        // Ajusta la posición del arma en las manos del jugador.
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2f, -2f); // Desplaza el arma ligeramente hacia arriba y la derecha.
        }

        // Modifica las estadísticas del disparo, incluyendo proyectiles aleatorios.
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Un 33% de probabilidad de cambiar el tipo de proyectil a uno personalizado.
            if (Main.rand.NextBool(3))
            {
                type = ModContent.ProjectileType<BalaPinkPopiProyectile>();
            }
        }

        // **Sección de ejemplos comentados para modificaciones adicionales:**
        // - Reemplazar balas normales por balas de alta velocidad.
        // - Disparar múltiples proyectiles en un arco.
        // - Ajustar la salida del proyectil para alinearse con la boca del arma.
        // - Implementar ráfagas de disparos como el Rifle de Asalto Reloj.
        // - Disparar dos tipos diferentes de proyectiles al mismo tiempo.
        // - Seleccionar aleatoriamente proyectiles entre varios tipos.

        // Los ejemplos comentados pueden usarse como referencia o guía para personalizar aún más la funcionalidad.
    }
}
