using System;
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
    public class BuppyGup : ModNPC
    {
        public ref float AttackState => ref NPC.ai[0];
        public ref float AttackTimer => ref NPC.ai[1];
        public Player Target => Main.player[NPC.target];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Rotation = MathHelper.Pi
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }

        public override void SetDefaults()
        {
            NPC.width = 46;
            NPC.height = 22;
            NPC.aiStyle = AIType = -1;

            NPC.damage = 58;
            NPC.lifeMax = 50;
            NPC.defense = 6;
            NPC.knockBackResist = 1f;


            NPC.value = Item.buyPrice(0, 0, 3, 65);
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            //Banner = NPC.type;
            //BannerItem = ModContent.ItemType<>();


        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("Buppy Gup Gup Guppy Bup")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || !spawnInfo.Player.ZoneRain || spawnInfo.Player.ZoneForest )
            {
                return 0f;
            }
            return SpawnCondition.OverworldDayRain.Chance * 1f;
        }

        public override void AI()
        {
            // Encuentra al jugador más cercano
            NPC.TargetClosest(false);

            // Determina la dirección ideal basada en la velocidad horizontal
            int idealDirection = (NPC.velocity.X > 0).ToDirectionInt();
            NPC.spriteDirection = idealDirection;

            // Controla el comportamiento del NPC basado en el estado de ataque
            switch ((int)AttackState)
            {
                // Estado 0: Moverse hacia una posición sobre el jugador
                case 0:
                    // Calcula la posición de destino (arriba y a un lado del jugador)
                    Vector2 flyDestination = Target.Center + new Vector2((Target.Center.X < NPC.Center.X).ToDirectionInt() * 400f, -240f);

                    // Calcula la velocidad ideal para moverse hacia el destino
                    Vector2 idealVelocity = NPC.DirectionTo(flyDestination) * 10f; // Usa DirectionTo en lugar de SafeDirectionTo

                    // Suaviza el movimiento del NPC
                    NPC.velocity = (NPC.velocity * 29f + idealVelocity) / 29f;
                    NPC.velocity = NPC.velocity.MoveTowards(idealVelocity, 1.5f);

                    // Rota el NPC en la dirección del movimiento
                    NPC.rotation = NPC.velocity.ToRotation() + (NPC.spriteDirection > 0).ToInt() * MathHelper.Pi;

                    // Cambia al siguiente estado si está cerca del destino o si el temporizador es mayor a 150 frames
                    if (NPC.WithinRange(flyDestination, 40f) || AttackTimer > 150f)
                    {
                        AttackState = 1f; // Cambia al estado 1
                        NPC.velocity *= 0.65f; // Reduce la velocidad
                        NPC.netUpdate = true; // Sincroniza en multijugador
                    }
                    break;

                // Estado 1: Frenar y mirar al jugador
                case 1:
                    // Ajusta la dirección del sprite para mirar al jugador
                    NPC.spriteDirection = (Target.Center.X > NPC.Center.X).ToDirectionInt();

                    // Reduce gradualmente la velocidad
                    NPC.velocity *= 0.97f;
                    NPC.velocity = NPC.velocity.MoveTowards(Vector2.Zero, 0.25f);

                    // Rota el NPC para mirar al jugador
                    NPC.rotation = NPC.rotation.AngleTowards(NPC.AngleTo(Target.Center) + (NPC.spriteDirection > 0).ToInt() * MathHelper.Pi, 0.2f);

                    // Velocidad de carga (puedes ajustar este valor)
                    float chargeSpeed = 11.5f;

                    // Cambia al siguiente estado si la velocidad es lo suficientemente baja
                    if (NPC.velocity.Length() < 1.25f)
                    {
                        // Reproduce un sonido de carga
                        SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, NPC.Center);

                        // Genera partículas de polvo para un efecto visual
                        for (int i = 0; i < 36; i++)
                        {
                            Dust dust = Dust.NewDustPerfect(NPC.Center, DustID.TintableDust);
                            dust.velocity = (MathHelper.TwoPi * i / 36f).ToRotationVector2() * 6f;
                            dust.scale = 1.1f;
                            dust.noGravity = true;
                        }

                        AttackState = 2f; // Cambia al estado 2
                        AttackTimer = 0f; // Reinicia el temporizador
                        NPC.velocity = NPC.DirectionTo(Target.Center) * chargeSpeed; // Carga hacia el jugador
                        NPC.netUpdate = true; // Sincroniza en multijugador
                    }
                    break;

                // Estado 2: Cargar hacia el jugador
                case 2:
                    // Velocidad de giro durante la carga
                    float angularTurnSpeed = MathHelper.Pi / 300f;

                    // Calcula la velocidad ideal hacia el jugador
                    idealVelocity = NPC.DirectionTo(Target.Center);

                    // Gira el NPC hacia el jugador
                    Vector2 leftVelocity = NPC.velocity.RotatedBy(-angularTurnSpeed);
                    Vector2 rightVelocity = NPC.velocity.RotatedBy(angularTurnSpeed);

                    // Usa una implementación manual de AngleBetween
                    float leftAngle = MathHelper.WrapAngle(leftVelocity.ToRotation() - idealVelocity.ToRotation());
                    float rightAngle = MathHelper.WrapAngle(rightVelocity.ToRotation() - idealVelocity.ToRotation());

                    if (Math.Abs(leftAngle) < Math.Abs(rightAngle))
                        NPC.velocity = leftVelocity;
                    else
                        NPC.velocity = rightVelocity;

                    // Rota el NPC en la dirección del movimiento
                    NPC.rotation = NPC.velocity.ToRotation() + (NPC.spriteDirection > 0).ToInt() * MathHelper.Pi;

                    // Cambia al estado 0 después de 50 frames
                    if (AttackTimer > 50f)
                    {
                        AttackState = 0f; // Cambia al estado 0
                        AttackTimer = 0f; // Reinicia el temporizador
                        NPC.velocity = Vector2.Lerp(NPC.velocity, -Vector2.UnitY * 8f, 0.14f); // Movimiento hacia arriba
                        NPC.netUpdate = true; // Sincroniza en multijugador
                    }
                    break;
            }

            // Incrementa el temporizador de ataque
            AttackTimer++;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 5)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
                    NPC.frame.Y = 0;
            }
        }
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
