using BagajeMod.Content.Items.Armor.Vanity;
using BagajeMod.Content.Items;
using BagajeMod.Content.NPCs.Terry;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagajeMod.Content.Items.Consumables
{
    // Código básico para la bolsa de tesoros de un jefe
    public class TerryBossBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Esta configuración es común para todas las bolsas de jefes.
            // Creará un efecto de brillo alrededor del objeto cuando se suelte en el mundo.
            // También permitirá que nuestra bolsa de jefe suelte armaduras de desarrollador.
            ItemID.Sets.BossBag[Type] = true;
            ItemID.Sets.PreHardmodeLikeBossBag[Type] = true; // Esta configuración asegura que la armadura de desarrollador solo se suelte en semillas de mundo especiales, ya que ese es el comportamiento de las bolsas de jefes de pre-hardmode.

            Item.ResearchUnlockCount = 3;
        }

        public override void SetDefaults()
        {
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Expert;
            Item.expert = true; // Esto asegura que "Experto" aparezca en la descripción emergente y que el color del nombre del objeto cambie.
        }

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // We have to replicate the expert drops from MinionBossBody here

            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<TerryMask>(), 7));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentoDeAlma>(), 1, 12, 16));
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<TerryBody>()));
        }
    }
}
