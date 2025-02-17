using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace BagajeMod.MainMenu
{
    public class BagajeMainMenu : ModMenu
    {
        public override string DisplayName => "Bagaje Mod Menu";

        // Carga el logo de tu mod (asegúrate de que la ruta sea correcta y sin la extensión)
        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("BagajeMod/Assets/Textures/Logo");

        // Puedes reutilizar el mismo fondo para SunTexture y MoonTexture
        public override Asset<Texture2D> SunTexture => ModContent.Request<Texture2D>("BagajeMod/Assets/Textures/MenuBackground");
        public override Asset<Texture2D> MoonTexture => ModContent.Request<Texture2D>("BagajeMod/Assets/Textures/MenuBackground");

        // Para la música del menú, usa MusicLoader.GetMusicSlot
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/AhoraEstasAqui");

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            // Dibujar el fondo del menú
            Texture2D backgroundTexture = ModContent.Request<Texture2D>("BagajeMod/Assets/Textures/MenuBackground").Value;
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

            // Dibujar el logo centrado en la parte superior
            Texture2D logoTexture = Logo.Value;
            Vector2 logoPosition = new Vector2(Main.screenWidth / 2f, 100f);
            spriteBatch.Draw(logoTexture, logoPosition, null, drawColor, logoRotation, logoTexture.Size() * 0.5f, logoScale, SpriteEffects.None, 0f);

            // Devuelve false para evitar que se dibuje el logo por defecto
            return false;
        }
    }
}
