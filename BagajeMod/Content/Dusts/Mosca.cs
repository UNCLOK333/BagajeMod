using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BagajeMod.Content.Dusts
{
    internal class Mosca : ModDust
    {
        public override string Texture => "BagajeMod/Content/Dusts/Mosca";

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = false; 
            dust.noLight = false; 
            dust.scale = 1f; 
            dust.alpha = 100; 

            
            dust.frame = new Rectangle(0, 0, 4, 6);


        }

        public override bool Update(Dust dust)
        {
            
            dust.velocity *= 0.98f;
            dust.velocity += new Vector2(Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-0.1f, 0.1f)); 

            dust.scale -= 0.01f; 

            if (dust.scale < 0.5f) 
            {
                dust.active = false;
            }

            
            return false;
        }
    }
}
