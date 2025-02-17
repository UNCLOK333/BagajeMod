using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using System.Drawing;
using System.Collections.Generic;
using Terraria.WorldBuilding;
using Terraria.IO;

namespace BagajeMod.Content.Tiles
{
    public class SerHumanoChamuscadoOre : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true; // Marca este bloque como un mineral.
            Main.tileSpelunker[Type] = true; // Resalta el mineral con poción de espeleólogo.
            Main.tileOreFinderPriority[Type] = 410; // Prioridad para el detector de metales.
            Main.tileShine2[Type] = true; // Modifica ligeramente el color de brillo.
            Main.tileShine[Type] = 975; // Frecuencia de partículas brillantes (valor mayor = menos frecuente).
            Main.tileMergeDirt[Type] = true; // Permite fusionarse con tierra.
            Main.tileSolid[Type] = true; // Es un bloque sólido.
            Main.tileBlockLight[Type] = true; // Bloquea la luz.

            // Configura propiedades visuales y de interacción.
            DustType = 5; // Tipo de partículas al picar.
            HitSound = SoundID.NPCHit18; // Sonido al golpear.
                                    // MineResist = 4f; // Resistencia al minado (comentado en el ejemplo).
            MinPick = 54; // Pico mínimo requerido (comentado en el ejemplo).
        }

    }

    public class SerHumanoChamuscadoOreSystem : ModSystem
    {
        public static LocalizedText ExampleOrePassMessage { get; private set; }
        public static LocalizedText BlessedWithExampleOreMessage { get; private set; }

        public override void SetStaticDefaults()
        {
            // Configura mensajes localizados.
            ExampleOrePassMessage = Mod.GetLocalization($"WorldGen.{nameof(ExampleOrePassMessage)}");
            BlessedWithExampleOreMessage = Mod.GetLocalization($"WorldGen.{nameof(BlessedWithExampleOreMessage)}");
        }

        // Inserta la generación del mineral en el proceso de creación del mundo.
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 1, new SerHumanoChamuscadoOrePass("Example Mod Ores", 237.4298f));
            }
        }
    }

    public class SerHumanoChamuscadoOrePass : GenPass
    {
        public SerHumanoChamuscadoOrePass(string name, float loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = SerHumanoChamuscadoOreSystem.ExampleOrePassMessage.Value;

            // Definir el rango vertical para la generación (justo por encima del Underworld)
            int lowerBound = (int)(Main.UnderworldLayer - 100); // 100 bloques arriba del Inframundo
            int upperBound = Main.UnderworldLayer; // Límite superior del Inframundo

            // Genera manchas de mineral en la franja definida
            for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 6E-05); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX); // Coordenada horizontal aleatoria
                int y = WorldGen.genRand.Next(lowerBound, upperBound); // Coordenada vertical en la franja

                // Genera la mancha de mineral
                WorldGen.TileRunner(
                    x,
                    y,
                    WorldGen.genRand.Next(3, 6),
                    WorldGen.genRand.Next(2, 6),
                    ModContent.TileType<SerHumanoChamuscadoOre>()
                );
            }
        }
    }
}