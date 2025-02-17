using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Items.Placeable
{
    public class CacaRosaEndurecidaBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Cantidad requerida para investigación en el juego.
            Item.ResearchUnlockCount = 10; // Cambiado a un número razonable para este material.

            // Define la prioridad de ordenamiento en el inventario. Se utiliza un valor cercano al de materiales comunes.
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 58; // Por debajo de PlatinumBar para indicar un menor valor relativo.
        }

        public override void SetDefaults()
        {
            // Configura el bloque colocable que representa la barra.
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.PlacedCacaRosaEndurecida>());
            Item.width = 18; // Ancho del sprite del objeto.
            Item.height = 18; // Alto del sprite del objeto.
            Item.value = 500; // Valor en monedas de cobre (bajo por ser un material cómico o menor).
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<CacaRosa>(5) // 5 minerales necesarios para fabricar 1 barra.
                .AddIngredient(ItemID.ClayBlock, 10)
                .AddTile(TileID.Furnaces) // Se requiere una forja para crearla.
                .Register();
        }
    }
}
