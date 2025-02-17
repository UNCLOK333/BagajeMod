using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using BagajeMod.Content.Mounts;
using Terraria.GameContent.ItemDropRules;
using BagajeMod.Content.Pets.FloppaPet;

namespace BagajeMod.Content.Items.Mounts
{
    public class FloppMountItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);

            Item.useStyle = ItemUseStyleID.HoldUp; // Estilo de uso (sostener hacia arriba).

            Item.value = Item.sellPrice(gold: 10); // Precio de venta del objeto.
            Item.rare = ItemRarityID.Yellow; // Rareza del objeto.
            Item.UseSound = SoundID.Item25; // Sonido al usar el objeto.
            Item.noMelee = true; // Indica que no es un arma cuerpo a cuerpo.
            Item.mountType = ModContent.MountType<FloppMount>(); // Vincula el objeto a la montura Floppa.
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Añade el objeto como posible botín en cofres del subsuelo.
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FloppMountItem>(), 300)); // 1 en 5 de probabilidad.
        }
    }
}
