using BagajeMod.Content.Items.Placeable;
using BagajeMod.Content.Pets.FloppaPet;
using BagajeMod.Event;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace BagajeMod.Content.NPCs
{
    public class Bungol : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f 
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        
        public override void SetDefaults()
        {
            NPC.width = 18; // Ancho del NPC
            NPC.height = 40; // Altura del NPC
            NPC.damage = 10; // Daño que inflige
            NPC.defense = 2; // Defensa
            NPC.lifeMax = 150; // Vida máxima
            NPC.HitSound = SoundID.NPCHit1; // Sonido al ser golpeado
            NPC.DeathSound = SoundID.NPCDeath1; // Sonido al morir
            NPC.value = 25f; // Dinero que suelta al morir
            NPC.knockBackResist = 0.5f; // Resistencia al retroceso
            NPC.aiStyle = 3; // Estilo de IA similar al Zombie

            AIType = NPCID.Zombie; // Basado en la IA del Zombie
            AnimationType = NPCID.Zombie; // Usa la animación del Zombie
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,

				new FlavorTextBestiaryInfoElement("Bungol es una criatura extraña que ronda en la noche."),
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldNightMonster.Chance * 0.2f;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CacaRosa>(), 1, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ItemID.CopperCoin, 1, 13, 32));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FloppaPetItem>(), 3333));

        }
        public override void OnKill()
        {
            // Posición de la explosión (centro del NPC)
            Vector2 explosionPosition = NPC.Center;

            // Radio de la explosión
            int explosionRadius = 3;

            // Daño de la explosión
            int explosionDamage = 1000;

            // Generar la explosión sin destruir bloques
            for (int x = -explosionRadius; x <= explosionRadius; x++)
            {
                for (int y = -explosionRadius; y <= explosionRadius; y++)
                {
                    int xPosition = (int)(x + explosionPosition.X / 16.0f);
                    int yPosition = (int)(y + explosionPosition.Y / 16.0f);

                    if (WorldGen.InWorld(xPosition, yPosition))
                    {
                        // Crear efecto de polvo para simular la explosión
                        Dust.NewDust(explosionPosition, NPC.width, NPC.height, DustID.FireworkFountain_Pink, 0f, 0f, 100, default(Color), 2f);
                    }
                }
            }

            // Aplicar daño a los jugadores cercanos
            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead && Vector2.Distance(player.Center, explosionPosition) <= explosionRadius * 16)
                {
                    player.Hurt(PlayerDeathReason.LegacyDefault(), explosionDamage, 0);
                }
            }

            // Reproducir sonido de explosión
            SoundEngine.PlaySound(SoundID.Item14, explosionPosition);

            // Llama al método OnEnemyKill del sistema de invasión
            InvasionBSystem.OnEnemyKill(NPC);
            base.OnKill();
        }
    }
}
