using System;
using BagajeMod.Content.Buffs;
using BagajeMod.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Projectiles.Minions
{
    public class ElClamorMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 11;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public sealed override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.aiStyle = ProjAIStyleID.Pet;
            AIType = ProjectileID.JumperSpider;
            Projectile.timeLeft = 18000;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.netImportant = true;
            Projectile.localNPCHitCooldown = 33 * Projectile.MaxUpdates; 
        }

        public override bool MinionContactDamage()
        {
            return true;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!CheckActive(player))
                return;

            if (Main.rand.NextBool(20))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Mosca>());
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.1f; // Reduce la velocidad de movimiento.
                Main.dust[dust].fadeIn = 13f; // Aumenta el tiempo antes de desvanecerse.
            }

            player.AddBuff(ModContent.BuffType<ElClamorBuff>(), 3600); // Renueva el buff.
        }

        private bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(ModContent.BuffType<ElClamorBuff>());
                return false;
            }

            if (owner.HasBuff(ModContent.BuffType<ElClamorBuff>()))
            {
                Projectile.timeLeft = 2; // Mantener al minion activo.
            }

            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Puedes añadir efectos visuales o lógicos al golpear a un NPC si lo deseas.
            int dustAmount = 5; // Cantidad de partículas de sangre generadas.
            for (int i = 0; i < dustAmount; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood);
                Main.dust[dust].velocity *= 1.5f;
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false; // Evita efectos al colisionar con bloques.
    }
}
