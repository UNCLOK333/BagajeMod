using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BagajeMod.Content.Tiles
{
    public class PlacedCacaRosa : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;

            DustType = DustID.Confetti_Pink;

            AddMapEntry(new Color(240, 52, 237));
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
        public override void FloorVisuals(Player player)
        {
            // Aplica el efecto Stinky al jugador
            player.AddBuff(BuffID.Stinky, 300); // 300 = 5 segundos (60 ticks = 1 segundo)


        }
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            // Probabilidad de generar partículas de moscas
            if (Main.rand.NextBool(5)) // 20% de probabilidad en cada frame
            {
                // Encuentra una posición aleatoria sobre el bloque
                int x = Main.rand.Next(16); // Coordenada aleatoria en el eje X dentro del bloque (16 píxeles de ancho)
                int y = Main.rand.Next(16); // Coordenada aleatoria en el eje Y dentro del bloque (16 píxeles de alto)

                // Genera la partícula sobre el bloque
                Dust dust = Dust.NewDustDirect(
                    Position: new Vector2(x, y),
                    Width: 0,
                    Height: 10,
                    ExtrasID.RoamingFly,
                    SpeedX: 2f,
                    SpeedY: 0f,
                    Alpha: 100,
                    newColor: default,
                    Scale: 1.5f
                );

                dust.velocity *= 0.5f; // Reduce la velocidad de las partículas
                dust.noGravity = true; // Hace que las partículas floten
            }
        }

    }
}