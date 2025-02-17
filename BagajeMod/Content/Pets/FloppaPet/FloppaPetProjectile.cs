using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Pets.FloppaPet
{
    public class FloppaPetProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = Main.projFrames[ProjectileID.BabyDino]; // Usa las mismas animaciones que el Baby Dino
            Main.projPet[Projectile.type] = true;

        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyDino); // Clona las propiedades del Baby Dino
            AIType = ProjectileID.BabyDino; // Usa la misma IA
            Projectile.width = 66; 
            Projectile.height = 64;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Mantén el proyectil activo mientras el jugador tenga el buff asociado
            if (!player.dead && player.HasBuff(ModContent.BuffType<FloppaPetBuff>()))
            {
                Projectile.timeLeft = 2;
            }
        }
    }
}
