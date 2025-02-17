using Terraria.ModLoader;

namespace BagajeMod
{
    public class BagajeMod : Mod
    {
        public static BagajeMod Instance { get; private set; }

        public override void Load()
        {
            Instance = this;
        }

        public override void Unload()
        {
            Instance = null;
        }
    }
}