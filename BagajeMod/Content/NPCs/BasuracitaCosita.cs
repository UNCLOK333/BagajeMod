using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.Utilities;
using BagajeMod.Content.Items.Placeable;
using BagajeMod.Content.Pets.FloppaPet;
using BagajeMod.Event;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework;

namespace BagajeMod.Content.NPCs
{
    public class BasuracitaCosita : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.damage = 20;
            NPC.width = 40;
            NPC.height = 40;
            NPC.defense = 10;
            NPC.lifeMax = 30;
            NPC.knockBackResist = 0.8f;
            NPC.value = Item.buyPrice(0, 0, 0, 80);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath15;
            NPC.behindTiles = true;
            //Banner = NPC.type;
            //BannerItem = ModContent.ItemType<>();

        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                new FlavorTextBestiaryInfoElement("Todas las especies tienen desperdicios, cuida el ambiente :D")
            });
        }

        public override void AI()
        {
            // Lógica personalizada de la IA
            // Puedes ajustar la velocidad, dirección y comportamiento según tus necesidades
            NPC.TargetClosest(true);
            Vector2 targetPosition = Main.player[NPC.target].Center;
            Vector2 direction = targetPosition - NPC.Center;
            direction.Normalize();
            float speed = 6f; // Velocidad de movimiento
            NPC.velocity = direction * speed;

            // Comportamiento de salto similar al Sea Urchin
            if (NPC.collideY)
            {
                NPC.velocity.Y = -10f; // Fuerza del salto
            }
        }

        //public override float SpawnChance(NPCSpawnInfo spawnInfo)
        //{
            //if (spawnInfo.PlayerSafe || spawnInfo.Player.ZoneBeach)
            //{
            //    return 0f;
            //}
            //return SpawnCondition.OverworldDayRain.Chance * 0.5f;
        //}
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
