using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BagajeMod.Content.Dusts;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace BagajeMod.Content.Walls
{
    internal class ParedDeLadrilloDeCacaRosa : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;

            DustType = ModContent.DustType<Sparkle>();

            AddMapEntry(new Color(255, 95, 177));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
