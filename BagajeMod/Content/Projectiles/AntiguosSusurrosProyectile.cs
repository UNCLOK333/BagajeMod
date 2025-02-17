using BagajeMod.Content.Dusts;// Importa partículas personalizadas del mod.
using BagajeMod.Content.Items.Weapons; // Importa ítems de armas personalizadas.
using Microsoft.Xna.Framework; // Importa herramientas matemáticas y de vectores.
using System;
using System.Collections.Generic; // Importa colecciones como listas.
using Terraria; // Importa las clases principales del juego Terraria.
using Terraria.Audio; // Permite manejar sonidos personalizados.
using Terraria.ID; // Importa identificadores de objetos y proyectiles.
using Terraria.ModLoader; // Proporciona herramientas para crear mods.

namespace BagajeMod.Content.Projectiles // Define el espacio de nombres del proyectil.
{
    // Esta clase representa un proyectil avanzado que puede adherirse a enemigos.
    public class AntiguosSusurrosProyectile : ModProjectile
    {
        // Estas propiedades facilitan el manejo de las variables AI para que el código sea más legible.
        public bool IsStickingToTarget
        { // Determina si el proyectil está adherido a un enemigo.
            get => Projectile.ai[0] == 1f; // Devuelve verdadero si `ai[0]` es 1.
            set => Projectile.ai[0] = value ? 1f : 0f; // Asigna 1 si está adherido, de lo contrario, 0.
        }

        public int TargetWhoAmI
        { // Identifica al enemigo objetivo al que está adherido.
            get => (int)Projectile.ai[1]; // Convierte el valor en un índice de NPC.
            set => Projectile.ai[1] = value; // Asigna un índice.
        }

        public int GravityDelayTimer
        { // Temporizador para aplicar gravedad al proyectil.
            get => (int)Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        public float StickTimer
        { // Temporizador para calcular cuánto tiempo ha estado adherido.
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }

        // Configura propiedades específicas del proyectil.
        public override void SetDefaults()
        {
            Projectile.width = 16; // Ancho del proyectil.
            Projectile.height = 16; // Altura del proyectil.
            Projectile.aiStyle = 0; // Estilo de IA personalizada.
            Projectile.friendly = true; // Puede dañar enemigos.
            Projectile.hostile = false; // No puede dañar al jugador.
            Projectile.DamageType = DamageClass.Ranged; // Define que el daño es a distancia.
            //Projectile.penetrate = 2; // Puede atravesar hasta 2 enemigos.
            Projectile.timeLeft = 200; // Tiempo de vida (10 segundos).
            //Projectile.alpha = 255; // Comienza completamente transparente.
            Projectile.light = 0.5f; // Emite luz moderada.
            //Projectile.ignoreWater = true; // Ignora el agua.
            Projectile.tileCollide = true; // Colisiona con bloques.
            //Projectile.hide = false; // Es invisible por defecto.
        }

        // Método principal de IA para actualizar el comportamiento del proyectil.
        public override void AI()
        {
            if (IsStickingToTarget)
            { // Si está adherido a un enemigo:
                StickyAI(); // Ejecuta la IA específica para proyectiles adheridos.
            }
            else
            {
                NormalAI(); // Ejecuta la IA estándar si no está adherido.
            }
        }

        // IA estándar para proyectiles no adheridos.
        private void NormalAI()
        {
            GravityDelayTimer++; // Incrementa el temporizador de gravedad.
            if (GravityDelayTimer >= 45)
            { // Si se supera el retraso de gravedad:
                Projectile.velocity.X *= 0.98f; // Aplica resistencia al viento.
                Projectile.velocity.Y += 0.10f; // Aplica gravedad.
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f); // Ajusta la rotación.

            // Genera partículas de "polvo" mientras se mueve.
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width,
                    ModContent.DustType<Mosca>(),
                    Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f,
                    200, Scale: 1.2f);
            }
        }

        // IA para proyectiles adheridos a un enemigo.
        private void StickyAI()
        {
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            StickTimer += 1f;

            // Cada 30 ticks, realiza un efecto visual de impacto.
            if (StickTimer % 30f == 0f)
            {
                Main.npc[TargetWhoAmI].HitEffect(0, 1.0);
            }

            // Si supera el tiempo máximo adherido o el objetivo ya no es válido, elimina el proyectil.
            if (StickTimer >= 900 || !Main.npc[TargetWhoAmI].active)
            {
                Projectile.Kill();
            }
            else
            {
                Projectile.Center = Main.npc[TargetWhoAmI].Center - Projectile.velocity * 2f; // Se mantiene en el centro del NPC.
            }
        }

        // Método ejecutado cuando el proyectil "muere".
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position); // Reproduce un sonido.
            Vector2 usePos = Projectile.position; // Posición de referencia para generar partículas.

            // Genera partículas al morir.
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(usePos, Projectile.width, Projectile.height, DustID.Tin);
                dust.noGravity = true;
            }
        }

        // Lógica para cuando el proyectil golpea a un enemigo.
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            IsStickingToTarget = true; // Marca el proyectil como adherido.
            TargetWhoAmI = target.whoAmI; // Registra al enemigo objetivo.
            //target.AddBuff(ModContent.BuffType<Buffs.ExampleJavelinDebuff>(), 900); // Aplica un efecto negativo al enemigo.
        }
    }
}
