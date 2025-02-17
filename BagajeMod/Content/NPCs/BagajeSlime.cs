using BagajeMod.Content.Items;
using BagajeMod.Content.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;


namespace BagajeMod.Content.NPCs
{
    public class BagajeSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // Establece la cantidad de frames que usa el NPC
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.damage = 20;
            NPC.width = 36;
            NPC.height = 31;
            NPC.aiStyle = NPCAIStyleID.Slime;
            NPC.defense = 5;
            NPC.lifeMax = 100;
            NPC.knockBackResist = 0.7f;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.alpha = 60;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AnimationType = NPCID.BlueSlime;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("Ni los mecos son tan pegajosos.")
            });
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 10)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            // Probabilidad de aparición en la superficie durante el día
            if (spawnInfo.Player.ZoneOverworldHeight && Main.dayTime)
            {
                return 0.2f;
            }
            return 0f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            // Añade un debuff al jugador cuando lo golpea
            if (hurtInfo.Damage > 0)
            {
                target.AddBuff(BuffID.Slimed, 60); // Aplica el debuff "Slimed" durante 1 segundo
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // Configura el loot del slime
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CacaRosa>(), 1, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ItemID.CopperCoin, 1, 13, 32)); 
        }

    }
}
