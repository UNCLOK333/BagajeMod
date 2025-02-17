using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public class BungolPesado : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 10;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.damage = Main.hardMode ? 100 : 30;
            NPC.width = 40;
            NPC.height = 52;
            NPC.defense = Main.hardMode ? 10 : 2;
            NPC.lifeMax = Main.hardMode ? 150 : 50;
            NPC.knockBackResist = Main.hardMode ? 0.2f : 0.5f;
            NPC.value = Item.buyPrice(0, 0, 1, 0);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            //Banner = NPC.type;
            //BannerItem = ModContent.ItemType<>();

        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,
                new FlavorTextBestiaryInfoElement("De la raza de los Bungoles, este bungol se mete chochos")
            });
        }

        public override void FindFrame(int frameHeight)
        {
            if (Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) < 18f || NPC.IsABestiaryIconDummy)
            {
                NPC.frameCounter += 1.0;
                if (NPC.frameCounter > 6.0)
                {
                    NPC.frameCounter = 0.0;
                    NPC.frame.Y = NPC.frame.Y + frameHeight;
                }
                if (NPC.frame.Y < frameHeight * 5)
                    NPC.frame.Y = frameHeight * 5;
                if (NPC.frame.Y > frameHeight * 9)
                    NPC.frame.Y = frameHeight * 5;
            }
            else
            {
                NPC.frameCounter += (double)Math.Abs(NPC.velocity.X);
                if (NPC.frameCounter > 6.0)
                {
                    NPC.frameCounter = 0.0;
                    NPC.frame.Y = NPC.frame.Y + frameHeight;
                }
                if (NPC.velocity.Y == 0f)
                {
                    if (NPC.direction == 1)
                        NPC.spriteDirection = 1;
                    if (NPC.direction == -1)
                        NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.frameCounter = 0.0;
                    NPC.frame.Y = frameHeight;
                    return;
                }
                if (NPC.velocity.X == 0f)
                {
                    NPC.frameCounter = 0.0;
                    NPC.frame.Y = 0;
                }
                else
                {
                    if (NPC.frame.Y > frameHeight * 4)
                        NPC.frame.Y = 0;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            // Verifica si el jugador está en la Mazmorra
            if (spawnInfo.Player.ZoneDungeon)
            {
                // Define la probabilidad de aparición del NPC en la Mazmorra
                return 0.1f; // Ajusta este valor según la frecuencia deseada
            }
            // Si el jugador no está en la Mazmorra, el NPC no aparece
            return 0f;
        }

        public override void AI()
        {
            // Asegurarse de que el NPC tenga un objetivo
            NPC.TargetClosest(true);
            Player target = Main.player[NPC.target];

            // Variables de movimiento
            float speed = 4f; // Velocidad de movimiento del NPC
            float acceleration = 0.1f; // Aceleración del NPC

            // Comportamiento de seguimiento del jugador
            Vector2 direction = target.Center - NPC.Center;
            direction.Normalize();
            direction *= speed;

            if (NPC.velocity.X < direction.X)
            {
                NPC.velocity.X += acceleration;
                if (NPC.velocity.X < 0 && direction.X > 0)
                    NPC.velocity.X += acceleration;
            }
            else if (NPC.velocity.X > direction.X)
            {
                NPC.velocity.X -= acceleration;
                if (NPC.velocity.X > 0 && direction.X < 0)
                    NPC.velocity.X -= acceleration;
            }

            // Comportamiento de salto
            if (NPC.collideY && Main.rand.NextBool(100)) // Probabilidad de saltar cuando está en el suelo
            {
                NPC.velocity.Y = -10f; // Fuerza del salto
                NPC.netUpdate = true;
            }

            // Ataque sísmico al aterrizar
            if (NPC.velocity.Y == 0 && NPC.oldVelocity.Y != 0) // Detecta cuando aterriza después de un salto
            {
                PerformSeismicAttack();
            }

            // Actualizar la dirección del sprite según la dirección del movimiento
            if (NPC.velocity.X != 0)
            {
                NPC.spriteDirection = NPC.direction;
            }
        }

        private void PerformSeismicAttack()
        {
            // Radio del ataque sísmico
            float seismicRadius = 100f;

            // Daño del ataque sísmico
            int damage = 50;

            // Generar una onda sísmica que daña a los jugadores en el radio
            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead && Vector2.Distance(player.Center, NPC.Center) <= seismicRadius)
                {
                    player.Hurt(PlayerDeathReason.LegacyDefault(), damage, 0);
                }
            }

            // Efectos visuales y sonoros del ataque sísmico
            // Puedes agregar efectos de partículas y sonidos aquí según tus preferencias
        }




        //------------------------------------------------------------------------------------------------------------------------------------
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CacaRosa>(), 1, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ItemID.CopperCoin, 1, 13, 32));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FloppaPetItem>(), 3333));
        }

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
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            // Aplicar sangrado al jugador si el daño es mayor a 0
            if (hurtInfo.Damage > 0)
                target.AddBuff(BuffID.Bleeding, 180, true);
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
