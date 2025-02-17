using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Items
{
    public class FragmentoDeAlma : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Nombre y descripción del ítem

            // Permite que el ítem se pueda intercambiar en el modo viaje (Journey Mode)
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            // Tamaño del ítem (18x24 píxeles)
            Item.width = 18;
            Item.height = 24;

            // Valor en monedas al vender el ítem
            Item.value = Item.sellPrice(silver: 25); // 25 plata
            Item.rare = ItemRarityID.Green; // Rareza verde

            // El ítem es un material (se puede usar para fabricar otros objetos)
            Item.material = true;

            // Cantidad máxima que se puede apilar
            Item.maxStack = 999;
        }

    }
}