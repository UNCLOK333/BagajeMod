using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace BagajeMod.Content.EmoteBubbles
{
    public class CacaEmote : ModEmoteBubble
    {
        public override void SetStaticDefaults()
        {
            AddToCategory(EmoteID.Category.Items);
        }
    }
}