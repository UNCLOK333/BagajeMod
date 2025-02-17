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
    public class BocadinPrinceso : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // Número de frames de animación del NPC
            Main.npcFrameCount[NPC.type] = 8;
        }

        // Método para configurar propiedades básicas del NPC
        public override void SetDefaults()
        {
            // Número de slots que ocupa el NPC (afecta la cantidad de NPCs que pueden spawnear)
            NPC.npcSlots = 2f;

            NPC.defDamage = 250; // Daño base del NPC
            NPC.damage = NPC.defDamage;

            // Daño que inflige el NPC
            NPC.damage = 250;

            // Estilo de IA (comportamiento similar a la Tortuga Gigante)
            NPC.aiStyle = NPCAIStyleID.GiantTortoise;

            // Tamaño del NPC (ancho y alto)
            NPC.width = 46;
            NPC.height = 32;

            // Defensa del NPC
            NPC.defense = 32;

            // Vida máxima del NPC
            NPC.lifeMax = 200;

            // Resistencia al retroceso (0 = nada, 1 = inmune)
            NPC.knockBackResist = 0.2f;

            // Tipo de animación (usando la animación de la Tortuga Gigante)
            AnimationType = NPCID.GiantTortoise;

            // Valor en monedas al ser derrotado
            NPC.value = Item.buyPrice(0, 0, 20, 0);

            // Sonido al ser golpeado
            NPC.HitSound = SoundID.NPCHit24;

            // El NPC tiene gravedad
            NPC.noGravity = false;

            // Banner y banner item asociado al NPC
            //Banner = NPC.type;
            //BannerItem = ModContent.ItemType<>();

        }

        // Método para configurar la entrada del NPC en el Bestiario
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                // Condiciones de spawn (jungla y jungla subterránea)
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle,

                // Texto de descripción en el Bestiario
                new FlavorTextBestiaryInfoElement("El mas insano de las criaturas B, sera mejor no encontrarlo en tu camino")
            });
        }

        // Método para manejar la lógica de IA del NPC
        public override void AI()
        {
            // Establece el daño base del NPC
            NPC.damage = NPC.defDamage;

            // Aumenta el daño si está en la fase de embestida
            if (NPC.ai[0] == 3f)
            {
                NPC.damage = (int)Math.Round(NPC.defDamage * 1.4);
            }
        }

        // Método que se ejecuta cuando el NPC es golpeado
        public override void HitEffect(NPC.HitInfo hit)
        {
            // Generar partículas de sangre al ser golpeado
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }

            // Generar más partículas y gore al morir
            if (NPC.life <= 0)
            {
                // Reproducir sonido de muerte predeterminado
                SoundEngine.PlaySound(SoundID.NPCDeath1, NPC.Center);

                // Generar partículas de sangre
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }

                
            }
        }


        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            // Verificar si el jugador está en la jungla
            bool enJungla = spawnInfo.Player.ZoneJungle;

            // Probabilidad base de aparición
            float probabilidadBase = 0.001f; // 0.01%

            // Si el jugador está en la jungla, aumentar la probabilidad
            if (enJungla)
            {
                probabilidadBase = 0.05f; // 0.5%
            }

            // Devolver la probabilidad de aparición
            return probabilidadBase;
        }



        // Método que se ejecuta cuando el NPC golpea a un jugador
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            // Aplicar sangrado al jugador si el daño es mayor a 0
            if (hurtInfo.Damage > 0)
                target.AddBuff(BuffID.Bleeding, 180, true);
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
