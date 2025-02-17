using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BagajeMod.Content.NPCs.Terry;

namespace BagajeMod.Common.Systems
{
    public class DownedBossSystem : ModSystem
    {
        public static bool downedTerryBoss = false;

        public override void OnWorldLoad()
        {
            downedTerryBoss = false;
        }

        public override void OnWorldUnload()
        {
            downedTerryBoss = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (downedTerryBoss)
            {
                tag["downedTerryBoss"] = true;
            }
        }

        public override void LoadWorldData(TagCompound tag)
        {
            downedTerryBoss = tag.ContainsKey("downedTerryBoss");
        }
    }
}
