using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace BagajeMod.Content.Systems
{
    public class DarkOverlaySystem : ModSystem
    {
        public static float DarkenIntensity = 0f; // 0 = transparente, 1 = completamente oscuro
        private Texture2D overlayTexture;

        public override void Load()
        {
            // Cargar la textura cuando se inicializa el mod
            overlayTexture = ModContent.Request<Texture2D>("BagajeMod/Assets/Textures/negro").Value;
        }

        public override void Unload()
        {
            // Liberar la referencia a la textura cuando se cierre el mod
            overlayTexture = null;
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            // Asegurar que el overlay solo se dibuje si la intensidad es mayor que 0
            if (DarkenIntensity > 0f && overlayTexture != null)
            {
                spriteBatch.Draw(
                    overlayTexture,
                    new Rectangle(0, 0, Main.screenWidth, Main.screenHeight),
                    Color.White * DarkenIntensity // Color.White para mantener la textura original
                );
            }
        }
    }
}
