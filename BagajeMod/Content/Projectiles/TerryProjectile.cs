using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Projectiles
{
    public class TerryProjectile : ModProjectile
    {
        private float acceleration = 0.5f; // Ajusta este valor para cambiar la aceleración
        private float maxSpeed = 20f;
        public override void SetStaticDefaults()
        {
            // Total de frames de animación
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20; // Ancho del proyectil
            Projectile.height = 20; // Alto del proyectil

            Projectile.friendly = false; // Puede dañar a los enemigos
            Projectile.hostile = true; // No es hostil (no daña al jugador)
            Projectile.DamageType = DamageClass.Ranged; // Tipo de daño (rango)
            Projectile.ignoreWater = true; // No se ve afectado por el agua
            Projectile.tileCollide = false; // No colisiona con los bloques
            Projectile.penetrate = 1; // Número de enemigos que puede atravesar
            Projectile.timeLeft = 300; // Duración del proyectil en ticks (5 segundos)
            Projectile.alpha = 0;
        }

        public override void AI()
        {
            // Aumentar la velocidad gradualmente (aceleración)
            if (Projectile.velocity.Length() < maxSpeed)
            {
                Projectile.velocity *= 1f + acceleration; // Aumenta la velocidad
            }

            // Limitar la velocidad máxima
            if (Projectile.velocity.Length() > maxSpeed)
            {
                Projectile.velocity = Vector2.Normalize(Projectile.velocity) * maxSpeed;
            }


            // Movimiento en línea recta
            //float speed = 10f; // Velocidad del proyectil
            //Projectile.velocity = Vector2.Normalize(Projectile.velocity) * speed;

            // Rotación del proyectil:
            // Se establece la rotación en función de la dirección de la velocidad sin sumar Pi/2,
            // de modo que el sprite se dibuje "acostado" (horizontal) en vez de vertical.
            Projectile.rotation = Projectile.velocity.ToRotation();

            // Animación (si tienes frames)
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }


        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Obtener la textura del proyectil
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            // Calcular el frame actual
            int frameHeight = texture.Height / Main.projFrames[Type];
            int startY = frameHeight * Projectile.frame;
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);

            // Calcular el origen (centro de la textura)
            Vector2 origin = sourceRectangle.Size() / 2f;

            // Dibujar el proyectil
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                sourceRectangle,
                Projectile.GetAlpha(lightColor),
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false; // Evitar que el juego dibuje el proyectil por defecto
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            // Efecto de destrucción al golpear al jugador
            SpawnBloodParticles();
        }

        public override void OnKill(int timeLeft)
        {
            // Efecto de destrucción al desaparecer sin golpear
            SpawnBloodParticles();
        }

        private void SpawnBloodParticles()
        {
            // Generar partículas de sangre
            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Blood, velocity, 0, default, 2f);
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
            }

            // Efecto adicional de Gore (opcional)
            if (Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < 3; i++)
                {
                    Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, Projectile.velocity * 0.5f, GoreID.Smoke1 + Main.rand.Next(3), 1f);
                }
            }
        }

    }
}