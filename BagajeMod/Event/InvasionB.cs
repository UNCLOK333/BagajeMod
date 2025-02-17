using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BagajeMod.Content.NPCs;
using System.Linq;

namespace BagajeMod.Event
{
    public class InvasionBSystem : ModSystem
    {
        public static bool EventIsOngoing;
        public static int AccumulatedKillPoints;
        public const int NeededKills = 100; // Número de enemigos requeridos para completar el evento

        public static Dictionary<int, int> PossibleEnemies = new()
        {
            { ModContent.NPCType<BolbolinSangriento>(), 1 }, // Enemigo 1 otorga 1 punto
            { ModContent.NPCType<BergamascoPusneto>(), 1 }, // Enemigo 2 otorga 1 punto
            { ModContent.NPCType<Bungol>(), 2 }, // Enemigo 3 otorga 2 puntos
            { ModContent.NPCType<BocadinPrinceso>(), 5 } // Minijefe otorga 5 puntos
        };

        public override void PreUpdateInvasions()
        {
            if (EventIsOngoing)
            {
                // Actualiza la barra de progreso
                Main.invasionProgress = AccumulatedKillPoints;
                Main.invasionProgressMax = NeededKills;
                Main.invasionProgressIcon = ModContent.NPCType<BolbolinSangriento>(); // Ícono personalizado para la barra
                Main.invasionProgressNearInvasion = true;

                // Verifica si el evento ha terminado
                if (AccumulatedKillPoints >= NeededKills)
                {
                    EventIsOngoing = false;
                    Main.invasionProgressNearInvasion = false;
                    Main.NewText("¡La invasión B ha sido derrotada!", 175, 75, 255);
                }
            }
        }

        public static void TryStartEvent()
        {
            if (EventIsOngoing || !NPC.downedBoss3) // Verifica si el evento ya está en curso o si Skeletron no ha sido derrotado
                return;

            EventIsOngoing = true;
            AccumulatedKillPoints = 0;
            Main.invasionSize = NeededKills;
            Main.invasionType = -1; // Tipo de invasión personalizada
            Main.invasionProgress = 0;
            Main.invasionProgressMax = NeededKills;
            Main.invasionProgressIcon = ModContent.NPCType<Bungol>();
            Main.invasionProgressNearInvasion = true;

            Main.NewText("¡Una invasión de criaturas B se aproxima!", 175, 75, 255);

            // Sincroniza el evento en multijugador
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }

        public static void OnEnemyKill(NPC npc)
        {
            if (EventIsOngoing && PossibleEnemies.ContainsKey(npc.type))
            {
                AccumulatedKillPoints += PossibleEnemies[npc.type];
                Main.invasionSize = NeededKills - AccumulatedKillPoints;

                // Sincroniza la progresión en multijugador
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.InvasionProgressReport);
                }
            }
        }

        public override void PostUpdateWorld()
        {
            if (EventIsOngoing)
            {
                SpawnEnemies();
            }
        }

        private void SpawnEnemies()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead)
                {
                    if (Main.rand.NextBool(50)) // Ajusta este valor para cambiar la frecuencia
                    {
                        int spawnX = (int)(player.Center.X + (Main.rand.NextBool() ? -950 : 950)); // Izquierda o derecha
                        int spawnY = (int)(player.Center.Y + Main.rand.Next(-200, 200)); // Altura aleatoria

                        // Verifica si la posición de generación es válida
                        if (!WorldGen.SolidTile(spawnX / 16, spawnY / 16))
                        {
                            int enemyType = Main.rand.Next(PossibleEnemies.Keys.ToArray());

                            int npcIndex = NPC.NewNPC(null, spawnX, spawnY, enemyType);
                            if (Main.netMode == NetmodeID.Server && npcIndex < Main.maxNPCs)
                            {
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcIndex);
                            }
                        }
                    }
                }
            }
        }
    }
}