using System.Collections.Generic;
using BagajeMod.Common.Systems;
using BagajeMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;


namespace BagajeMod.Content.NPCs.Terry
{
    [AutoloadBossHead]
    public class TerryBody : ModNPC
    {

        private bool screenShakeTriggered = false;
        private int transitionTimer = 0; // Temporizador para la transición a la segunda fase
        private const int TransitionDuration = 60; // Duración de la transición (en ticks)
        private int shootTimer = 0;


        public static int secondStageHeadSlot = -1; //icono

        // Propiedad para controlar la segunda fase usando NPC.ai[0]
        public bool SecondStage
        {
            get => NPC.ai[0] == 1f;
            set => NPC.ai[0] = value ? 1f : 0f;
        }
        public Vector2 FirstStageDestination
        {
            get => new Vector2(NPC.ai[1], NPC.ai[2]);
            set
            {
                NPC.ai[1] = value.X;
                NPC.ai[2] = value.Y;
            }
        }
        public int MinionMaxHealthTotal
        {
            get => (int)NPC.ai[3];
            set => NPC.ai[3] = value;
        }
        // Salud actual de los minions
        public int MinionHealthTotal { get; set; }

        // Última posición objetivo registrada
        public Vector2 LastFirstStageDestination { get; set; } = Vector2.Zero;

        // Bandera para minions ya invocados
        public bool SpawnedMinions
        {
            get => NPC.localAI[0] == 1f;
            set => NPC.localAI[0] = value ? 1f : 0f;
        }

        private const int FirstStageTimerMax = 90;
        public ref float FirstStageTimer => ref NPC.localAI[1];
        public ref float SecondStageTimer_SpawnEyes => ref NPC.localAI[3];

        
        // Tipo de minion asociado
        public static int MinionType()
        {
            return ModContent.NPCType<TerryClone>();
        }

        public static int MinionCount()
        {
            int count = 3;

            if (Main.expertMode)
            {
                count += 3; // Increase by 5 if expert or master mode
            }

            if (Main.getGoodWorld)
            {
                count += 5; // Increase by 5 if using the "For The Worthy" seed
            }

            return count;
        }

        public override void Load()
        {
            string texture = BossHeadTexture + "_SecondStage"; // Ruta de la textura para la segunda fase
            secondStageHeadSlot = Mod.AddBossHeadTexture(texture, -1); // Registra la textura (-1 evita sobrescribir el icono de la primera fase)
        }
        public override void BossHeadSlot(ref int index)
        {
            int slot = secondStageHeadSlot;
            if (SecondStage && slot != -1)
            {
                index = slot;
            }
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;

            NPCID.Sets.MPAllowedEnemies[Type] = true;

            NPCID.Sets.BossBestiaryPriority.Add(Type);

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "BagajeMod/Assets/Textures/Bestiary/TerryPreview",
                PortraitScale = 0.6f,
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public override void SetDefaults()
        {
            NPC.width = 120; 
            NPC.height = 120; 
            NPC.damage = 50;
            NPC.defense = 20;
            NPC.lifeMax = 5000;
            NPC.knockBackResist = 0f; 
            NPC.aiStyle = -1; 
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.noGravity = true; 
            NPC.noTileCollide = true; // Atraviesa bloques
            NPC.boss = true;
            NPC.SpawnWithHigherTime(15);
            NPC.npcSlots = 10f;

            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/LostTheBattle");
            }
        }
        //"Fuerte y poderosa alma rencarnada de miles de humanos que alguna vez estuvieron por bajas alturas de nuestro mundo"
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new FlavorTextBestiaryInfoElement("Fuerte y poderosa alma rencarnada de miles de humanos que alguna vez estuvieron por bajas alturas de nuestro mundo")
            });
        }


        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // 1. Trofeo: 1/10 de probabilidad
           npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Furniture.TerryTrophy>(), 10));

            // 2. Modo Clásico (no experto)
           LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            // Máscara del jefe: 1/7 de probabilidad
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Armor.Vanity.TerryMask>(), 7));

            // Ejemplo de ítem que cae en modo clásico
            int itemType = ModContent.ItemType<Items.FragmentoDeAlma>();
            var parameters = new DropOneByOne.Parameters()
            {
                ChanceNumerator = 1,
                ChanceDenominator = 1,
                MinimumStackPerChunkBase = 1,
                MaximumStackPerChunkBase = 1,
                MinimumItemDropsCount = 12,
                MaximumItemDropsCount = 15,
            };
            notExpertRule.OnSuccess(new DropOneByOne(itemType, parameters));

             //Añadir la regla para modo no experto
            npcLoot.Add(notExpertRule);

            // 3. Modo Experto: Bolsa del tesoro
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.Consumables.TerryBossBag>()));

            // 4. Modo Maestro: Reliquia y mascota
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Placeable.Furniture.TerryRelic>()));
            //npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<Items.Pets.TerryBossPetItem>(), 4));
        }

        public override void OnKill()
        {
            if (!Main.dedServ)
            {
                Main.NewText("¡Terry ha sido derrotado!", 175, 75, 255); // Mensaje en pantalla al morir
            }

            // This sets downedMinionBoss to true, and if it was false before, it initiates a lantern night
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedTerryBoss, -1);
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            // Here you'd want to change the potion type that drops when the boss is defeated. Because this boss is early pre-hardmode, we keep it unchanged
            // (Lesser Healing Potion). If you wanted to change it, simply write "potionType = ItemID.HealingPotion;" or any other potion type
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            // Establece el cooldown para evitar abuso de invulnerabilidad tras recibir daño.
            cooldownSlot = ImmunityCooldownID.Bosses; // Usa el cooldown de jefe.
            return true; // Permite que el ataque impacte al jugador.
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 10)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            // Crea efectos de partículas al recibir daño
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }

            if (NPC.life <= 0)
            {
                // Cambia las dimensiones del NPC al morir
                NPC.position.X += NPC.width / 2;
                NPC.position.Y += NPC.height / 2;
                NPC.width = (int)(200 * NPC.scale);
                NPC.height = (int)(100 * NPC.scale);
                NPC.position.X -= NPC.width / 2;
                NPC.position.Y -= NPC.height / 2;

                // Genera partículas adicionales
                for (int i = 0; i < 40; i++)
                {
                    int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 0f, 0f, 100, default, 2f);
                    Main.dust[dustIndex].velocity *= 3f;

                    if (Main.rand.NextBool())
                    {
                        Main.dust[dustIndex].scale = 0.5f;
                        Main.dust[dustIndex].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }

                for (int k = 0; k < 70; k++)
                {
                    int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 0f, 0f, 100, default, 3f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 5f;

                    int secondDustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 0f, 0f, 100, default, 2f);
                    Main.dust[secondDustIndex].velocity *= 2f;
                }

                // Genera elementos visuales de "destrucción" (Gores)
                //if (Main.netMode != NetmodeID.Server)
                //{
                //    float randomSpread = Main.rand.Next(-200, 201) / 100f;
                //    for (int i = 1; i <= 7; i++)
                //    {
                //        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>($"GoreNombre{i}").Type, NPC.scale);
                //    }
                //}
            }
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            // Ajusta los valores de vida y daño según la dificultad y el número de jugadores
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance * bossAdjustment);
            NPC.damage = (int)(NPC.damage * (Main.expertMode ? 1.5f : 1.0f));
        }

        public override void AI()
        {
            // 1. Buscar jugador objetivo
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];


            // 2. Comportamiento si jugador muerto
            if (player.dead)
            {
                NPC.velocity.Y -= 0.04f;
                NPC.EncourageDespawn(10);
                return;
            }


            // 3. Verificar cambio de fase
            CheckSecondStage();

            // 4. Lógica de fase
            if (SecondStage)
            {
                // Efectos de segunda fase
                if (!screenShakeTriggered)
                {
                    TriggerScreenShake();
                    screenShakeTriggered = true;
                }
                

                // Aumentar el tamaño del jefe en un 50%
                if (NPC.scale < 1.3f)
                {
                    NPC.scale = 1.3f;
                    NPC.width = (int)(NPC.width * 1.3f);
                    NPC.height = (int)(NPC.height * 1.3f);
                }

                // Invocar minions en la segunda fase
                if (!SpawnedMinions)
                {
                    SpawnMinions();
                }

                DoSecondStage(player);
            }
            else
            {
                DoFirstStage(player);
            }



            // 5. Rotación del sprite (siempre vertical)
            NPC.rotation = 0f; // Mantener el sprite vertical
            NPC.spriteDirection = NPC.velocity.X > 0 ? 1 : -1; // Cambiar dirección en el eje X

        }

       
        private void CheckSecondStage()
        {
            // Activar segunda fase cuando la vida sea menor o igual al 50%
            if (NPC.life <= NPC.lifeMax / 3 && !SecondStage)
            {
                SecondStage = true;
                NPC.netUpdate = true; // Sincronizar en multiplayer
            }
        }

        private void TriggerScreenShake()
        {
            // Efecto de sacudida de pantalla como Deerclops
            PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, Main.rand.NextVector2CircularEdge(1f, 1f), 10f, 10f, 20, 1000f, "TerryPhase2");
            Main.instance.CameraModifiers.Add(modifier);
        }


        private void SpawnMinions()
        {
            if (SpawnedMinions)
            {
                return;
            }

            SpawnedMinions = true;

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }

            int count = MinionCount();
            var entitySource = NPC.GetSource_FromAI();


            MinionMaxHealthTotal = 0;
            // Crear minions y configurar sus propiedades
            for (int i = 0; i < count; i++)
            {
                NPC minionNPC = NPC.NewNPCDirect(entitySource, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<TerryClone>(), NPC.whoAmI);
                if (minionNPC.whoAmI == Main.maxNPCs)
                    continue; // spawn failed due to spawn cap

                TerryClone minion = (TerryClone)minionNPC.ModNPC;
                minion.ParentIndex = NPC.whoAmI; // Let the minion know who the "parent" is
                minion.PositionOffset = i / (float)count;

                MinionMaxHealthTotal += minionNPC.lifeMax;

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: minionNPC.whoAmI);
                }
            }
            // sync MinionMaxHealthTotal
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
            }
        }


        



        private void DoFirstStage(Player player)
        {

            

            // Disparar proyectiles en la primera fase

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                shootTimer++;
                int fireRate = 120; // Disparar cada 2 segundos (120 ticks)

                if (shootTimer >= fireRate)
                {
                    shootTimer = 0;

                    Vector2 projectileDirection = Vector2.Normalize(player.Center - NPC.Center) * 10f;
                    int projectileType = ModContent.ProjectileType<TerryProjectile>();
                    int damage = 20; // Daño del proyectil

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, projectileDirection, projectileType, damage, 1f, Main.myPlayer);
                }
            }

            // Comportamiento de movimiento en la primera fase
            float speed = 7f; // Velocidad de movimiento
            float inertia = 20f; // Suavizado del movimiento

            // Moverse hacia el jugador
            Vector2 direction = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero);
            Vector2 moveTo = direction * speed;
            NPC.velocity = (NPC.velocity * (inertia - 1) + moveTo) / inertia;

            // Hacer visible al jefe en la primera fase
            NPC.alpha = 0; // Asegurar que el NPC sea visible

            // Rotación del NPC
            NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;


        }


        private void DoSecondStage(Player player)
        {

            //if (NPC.life < NPC.lifeMax * 0.5f)
            //{
            //    ApplySecondStageBuffImmunities();
            //}

            // Comportamiento de movimiento en la segunda fase
            //float speed = 8f; // Velocidad de movimiento (más rápido)
            //float inertia = 10f; // Suavizado del movimiento (más ágil)

            // Moverse hacia el jugador de manera más agresiva
            //Vector2 direction = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero);
            //Vector2 moveTo = direction * speed;
            //NPC.velocity = (NPC.velocity * (inertia - 1) + moveTo) / inertia;

            // Rotación del NPC
            //NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
            //------------------------------------------------------------------------------------------------------------------------movimientoi 2

            if (NPC.life < NPC.lifeMax * 0.5f)
            {
                ApplySecondStageBuffImmunities();
            }

            float speed = 8f; // Velocidad base depende de la fase
            float acceleration = 0.8f;

            // Movimiento básico (hacia el jugador)
            Vector2 targetPosition = player.Center;
            Vector2 direction = targetPosition - NPC.Center;
            
            direction.Normalize();
            




            // Detecta cambio de dirección y aplica aceleración
            if (direction.X * NPC.velocity.X < 0 || direction.Y * NPC.velocity.Y < 0)
            {
                NPC.velocity += direction * acceleration;
            }
            else
            {
                NPC.velocity = Vector2.Lerp(NPC.velocity, direction * speed, 0.1f);
            }

            // Efecto de llorar sangre
            SpawnBloodParticles();



        }
        
        private void SpawnBloodParticles()
        {
            // Efecto de llorar sangre
            if (Main.rand.NextBool(3)) // Frecuencia de partículas
            {
                Vector2 position = NPC.Center + new Vector2(Main.rand.Next(-NPC.width / 2, NPC.width / 2), -NPC.height / 2);
                Vector2 velocity = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(1f, 3f));
                Dust dust = Dust.NewDustPerfect(position, DustID.Blood, velocity, 0, default, 2f);
                dust.noGravity = false;
            }
        }
        private void ApplySecondStageBuffImmunities()
        {
            if (NPC.buffImmune[BuffID.OnFire])
            {
                return;
            }
            // Halfway through stage 2, this boss becomes immune to the OnFire buff.
            // This code will only run once because of the !NPC.buffImmune[BuffID.OnFire] check.
            // If you make a similar check for just a life percentage in a boss, you will need to use a bool to track if the corresponding code has run yet or not.
            NPC.BecomeImmuneTo(BuffID.OnFire);

            // Finally, this boss will clear all the buffs it currently has that it is now immune to. ClearImmuneToBuffs should not be run on multiplayer clients, the server has authority over buffs.
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ClearImmuneToBuffs(out bool anyBuffsCleared);

                if (anyBuffsCleared)
                {
                    // Since we cleared some fire related buffs, spawn some smoke to communicate that the fire buffs have been extinguished.
                    // This example is commented out because it would require a ModPacket to manually sync in order to work in multiplayer.
                    /* for (int g = 0; g < 8; g++) {
						Gore gore = Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, default, Main.rand.Next(61, 64), 1f);
						gore.scale = 1.5f;
						gore.velocity += new Vector2(1.5f, 0).RotatedBy(g * MathHelper.PiOver2);
					}*/
                }
            }

            // Spawn a ring of dust to communicate the change.
            for (int loops = 0; loops < 2; loops++)
            {
                for (int i = 0; i < 50; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                    Dust d = Dust.NewDustPerfect(NPC.Center, DustID.BlueCrystalShard, speed * 10 * (loops + 1), Scale: 1.5f);
                    d.noGravity = true;
                }
            }
        }
        



    }
}