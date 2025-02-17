using System.Collections.Generic;
using BagajeMod.Content.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace BagajeMod.Content.NPCs
{
    [AutoloadHead]
    public class Bungolnut : ModNPC
    {

        private static int ShimmerHeadIndex;
        private static Profiles.StackedNPCProfile NPCProfile;

        public static Asset<Texture2D> AltTexture;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 23;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 60;
            NPCID.Sets.AttackAverageChance[NPC.type] = 15;
            NPCID.Sets.ShimmerTownTransform[NPC.type] = true;
            NPCID.Sets.ShimmerTownTransform[Type] = true;
            NPC.Happiness
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Nurse, AffectionLevel.Love)
                .SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Hate);

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                Velocity = 0.5f // Slight movement in the bestiary
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifiers);

            if (!Main.dedServ)
            {
                AltTexture = ModContent.Request<Texture2D>(Texture + "Alt", AssetRequestMode.AsyncLoad);
            }
            NPCProfile = new Profiles.StackedNPCProfile(
                new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture + "_Party"),
                new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex, Texture + "_Shimmer_Party")
            );
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.lavaImmune = false;
            NPC.width = 24;
            NPC.height = 44;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 50;
            NPC.defense = 80;
            NPC.lifeMax = 380;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.PartyGirl;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("Bungolnut es un Bungol que adora objetos extravagantes.")
            });
        }

        //public override bool CanTownNPCSpawn(int numTownNPCs)
        //{ // Requirements for the town NPC to spawn.
        //    if (TownNPCRespawnSystem.unlockedExamplePersonSpawn)
        //    {
        //        // If Example Person has spawned in this world before, we don't require the user satisfying the ExampleItem/ExampleBlock inventory conditions for a respawn.
        //        return true;
        //    }
        //
        //    foreach (var player in Main.ActivePlayers)
        //    {
        //        // Player has to have either an ExampleItem or an ExampleBlock in order for the NPC to spawn
        //        if (player.inventory.Any(item => item.type == ModContent.ItemType<ExampleItem>() || item.type == ModContent.ItemType<Items.Placeable.ExampleBlock>()))
        //        {
        //            return true;
        //        }
        //    }
        //
        //    return false;
        //}

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            for (var i = 0; i < 255; i++)
            {
                Player player = Main.player[i];
                foreach (Item item in player.inventory)
                {
                    if (item.type == ItemID.Gel)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override ITownNPCProfile TownNPCProfile()
        {
            return NPCProfile;
        }


        public override List<string> SetNPCNameList() => new()
        {
            "Bungolnut",
            "Nutty",
            "Mr. Bungol",
            "Menso",
            "Curio"
        };

        public override string GetChat()
        {
            WeightedRandom<string> dialogue = new();

            dialogue.Add("¿Te gustan las colecciones? Tengo unas que te encantarán.");
            dialogue.Add("La extravagancia es mi pasión, y veo que tú tienes potencial.");
            dialogue.Add("Cada objeto cuenta una historia, si sabes escuchar.");
            dialogue.Add("¡Trae algo brillante y podríamos hacer negocios!");

            if (Main.LocalPlayer.ZoneJungle)
                dialogue.Add("La jungla tiene tesoros que aún no he visto...");

            if (Main.LocalPlayer.ZoneSnow)
                dialogue.Add("El hielo nunca me ha gustado, pero el cristal es increible.");

            return dialogue;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override void AddShops()
        {
            NPCShop shop = new(Type);
            shop.Add(ItemID.GoldCoin)
                .Add(ItemID.MagicMirror)
                .Add(ItemID.WoodenBoomerang)
                .Add(ItemID.RareEnchantment)
                .Register();
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Flare_Blue, 2f * hit.HitDirection, -2f);
                }
            }
        }
        //public override void ModifyNPCLoot(NPCLoot npcLoot)
        //{
        //   npcLoot.Add(ItemDropRule.Common(itemId: ModContent.ItemType<ItemID.Gel>()));
        //}

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        //public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        //{
        //   projType = ModContent.ProjectileType<Ethereal Lance> ();
        //    attackDelay = 1;
        //}

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<SparklingBall>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
            // SparklingBall is not affected by gravity, so gravityCorrection is left alone.
        }


    }
}
