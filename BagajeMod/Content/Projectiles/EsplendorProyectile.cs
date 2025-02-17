using BagajeMod.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Projectiles
{
    public class EsplendorProyectile : ModProjectile // Clase que define un proyectil personalizado basado en "ModProjectile".
    {
        // Rango mínimo y máximo del proyectil cuando se extiende. Estas propiedades son virtuales, por lo que pueden ser sobrescritas en clases derivadas.
        protected virtual float HoldoutRangeMin => 24f; // Distancia mínima de la lanza desde el jugador (en píxeles).
        protected virtual float HoldoutRangeMax => 96f; // Distancia máxima de la lanza desde el jugador (en píxeles).

        // Define las propiedades básicas del proyectil.
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Spear); // Clona los valores predeterminados de una lanza del juego base. Esto incluye propiedades como tamaño, estilo AI, daño, etc.
        }

        // Lógica personalizada que se ejecuta antes de la inteligencia artificial (AI) predeterminada.
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner]; // Obtiene la instancia del jugador que lanzó el proyectil.
            int duration = player.itemAnimationMax; // Duración total en cuadros (frames) de la animación del ítem.

            player.heldProj = Projectile.whoAmI; // Actualiza el ID del proyectil sostenido por el jugador.

            // Resetea el tiempo restante del proyectil si es mayor a la duración permitida.
            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = duration;
            }

            // Normaliza la dirección del proyectil (reduce su vector de velocidad a un vector unitario).
            Projectile.velocity = Vector2.Normalize(Projectile.velocity);

            float halfDuration = duration * 0.5f; // Calcula la mitad de la duración.
            float progress;

            // Calcula el progreso de la animación, que va de 0.0 a 1.0 y luego regresa a 0.0.
            if (Projectile.timeLeft < halfDuration)
            {
                progress = Projectile.timeLeft / halfDuration;
            }
            else
            {
                progress = (duration - Projectile.timeLeft) / halfDuration;
            }

            // Calcula la posición del proyectil usando interpolación suave entre el rango mínimo y máximo.
            Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);

            // Ajusta la rotación del sprite del proyectil según su dirección.
            if (Projectile.spriteDirection == -1)
            {
                // Si el sprite está mirando a la izquierda, rota 45 grados.
                Projectile.rotation += MathHelper.ToRadians(45f);
            }
            else
            {
                // Si el sprite está mirando a la derecha, rota 135 grados.
                Projectile.rotation += MathHelper.ToRadians(135f);
            }

            // Evita generar partículas si el código se ejecuta en un servidor dedicado.
            if (!Main.dedServ)
            {
                // Genera partículas personalizadas con cierta probabilidad para efectos visuales.
                if (Main.rand.NextBool(3))
                { // 1 de cada 3 veces.
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Sparkle>(), Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, Alpha: 128, Scale: 1.2f);
                }

                if (Main.rand.NextBool(4))
                { // 1 de cada 4 veces.
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Sparkle>(), Alpha: 128, Scale: 0.3f);
                }
            }

            return false; // Devuelve `false` para evitar que se ejecute la inteligencia artificial predeterminada del proyectil.
        }
    }
}
