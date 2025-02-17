using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace BagajeMod.Content.Items.Placeable
{
    internal class ParedDeLadrilloDeCacaRosa : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
        }

        public override void SetDefaults()
        {
            // ModContent.WallType<Walls.ExampleWall>() retrieves the id of the wall that this item should place when used.
            // DefaultToPlaceableWall handles setting various Item values that placeable wall items use.
            // Hover over DefaultToPlaceableWall in Visual Studio to read the documentation!
            Item.DefaultToPlaceableWall(ModContent.WallType<Walls.ParedDeLadrilloDeCacaRosa>());
        }

        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient<LadrilloDeCacaRosa>(1)
                .AddTile<Tiles.FurnitureCaca.BancodetrabajoCaca>()
                .Register();
        }
    }
}
