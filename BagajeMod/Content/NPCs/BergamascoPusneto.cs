using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class BergamascoPusneto : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // Define el número de frames de animación del NPC
            Main.npcFrameCount[NPC.type] = 10;
        }

        public override void SetDefaults()
        {
            // Estilo de IA (comportamiento similar al Unicornio)
            NPC.aiStyle = NPCAIStyleID.Unicorn;

            // Daño que inflige el NPC
            NPC.damage = 54;

            // Tamaño del NPC (ancho y alto)
            NPC.width = 46;
            NPC.height = 30;

            // Defensa del NPC
            NPC.defense = 2;

            // Vida máxima del NPC
            NPC.lifeMax = 120;

            // Resistencia al retroceso (0 = nada, 1 = inmune)
            NPC.knockBackResist = 0.3f;

            // Tipo de IA (comportamiento similar al Lobo)
            AIType = NPCID.Wolf;

            // Valor en monedas al ser derrotado
            NPC.value = Item.buyPrice(0, 0, 2, 0);

            // Sonidos al ser golpeado y al morir
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath5;

            // Banner y banner item asociado al NPC
            Banner = NPC.type;
            //BannerItem = ModContent.ItemType<>();

            // Ajustar estadísticas en modo Experto y Maestro
          
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                // Condiciones de spawn (noche)
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,

                // Bioma de spawn (superficie)
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,

                // Texto de descripción en el Bestiario
                new FlavorTextBestiaryInfoElement("Una raza de perros muy peligosa, no dudara en morder a los que huelen a polla")
            });
        }

        public override void FindFrame(int frameHeight)
        {
            // Si el NPC es un ícono del Bestiario
            if (NPC.IsABestiaryIconDummy)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter > 5)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y > frameHeight * 7)
                {
                    NPC.frame.Y = frameHeight;
                }
            }
            else
            {
                // Dirección del sprite (izquierda o derecha)
                NPC.spriteDirection = NPC.direction;

                // Animación de salto
                if (NPC.velocity.Y < 0f) // jump
                {
                    NPC.frame.Y = frameHeight * 8;
                    NPC.frameCounter = 0.0;
                }
                // Animación de caída
                else if (NPC.velocity.Y > 0f) // fall
                {
                    NPC.frame.Y = frameHeight * 9;
                    NPC.frameCounter = 0.0;
                }
                // Animación de caminar
                else
                {
                    NPC.frameCounter += Math.Abs(NPC.velocity.X) * 0.4f;
                    if (NPC.frameCounter > 4)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y += frameHeight;
                        if (NPC.frame.Y < frameHeight)
                        {
                            NPC.frame.Y = frameHeight;
                        }
                        if (NPC.frame.Y > frameHeight * 7)
                        {
                            NPC.frame.Y = frameHeight;
                        }
                    }
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            // Verificar si el jugador está en el bioma desértico durante el día
            if (spawnInfo.Player.ZoneDesert && Main.dayTime)
            {
                // Probabilidad de aparición del 23% en el desierto durante el día
                return 0.23f;
            }
            // Si no se cumplen las condiciones, el NPC no aparece
            return 0f;
        }


        // Método que se ejecuta cuando el NPC golpea a un jugador
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            // Aplicar sangrado al jugador si el daño es mayor a 0
            if (hurtInfo.Damage > 0)
                target.AddBuff(BuffID.Bleeding, 180, true);
        }


        // Método que se ejecuta cuando el NPC es golpeado
        public override void HitEffect(NPC.HitInfo hit)
        {
            // Generar partículas de sangre al ser golpeado
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }

            // Generar más partículas y Gore al morir
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }

            }
        }

        // Método para definir los drops del NPC
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // Añadir drops al loot table
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
