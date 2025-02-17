using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BagajeMod.Content.Pets.FloppaPet;
using Terraria.DataStructures;

namespace BagajeMod.Content.Pets.FloppaPet
{
    public class FloppaPetItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 4));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish); // Copia las configuraciones predeterminadas del Zephyr Fish.

            Item.shoot = ModContent.ProjectileType<FloppaPet.FloppaPetProjectile>(); // Dispara el proyectil de la mascota Floppa.
            Item.buffType = ModContent.BuffType<FloppaPetBuff>(); // Aplica el buff de la mascota Floppa al usar el objeto.
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                player.AddBuff(Item.buffType, 3600); // Aplica el buff por 1 minuto real (3600 ticks).
            }
            return true;
        }
    }
}
