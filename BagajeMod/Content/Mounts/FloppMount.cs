using Microsoft.Xna.Framework; // Librería para vectores, colores y otras utilidades gráficas.
using Microsoft.Xna.Framework.Graphics; // Librería para manejar texturas y gráficos.
using ReLogic.Content; // Librería para cargar y manejar contenido (como texturas).
using System; // Funcionalidades estándar de C# (e.g., Math, DateTime).
using System.Collections.Generic; // Permite manejar colecciones como listas y diccionarios.
using System.Linq; // Funciones para manipular colecciones.
using Terraria; // Contiene clases principales de Terraria.
using Terraria.DataStructures; // Estructuras de datos utilizadas en Terraria.
using Terraria.ID;
using Terraria.ModLoader; // Base para crear mods en Terraria.
using BagajeMod.Content.Buffs;

namespace BagajeMod.Content.Mounts // Espacio de nombres para la clase "ExampleMount".
{
    public class FloppMount : ModMount
    {
        // Configura las propiedades estáticas de la montura (parámetros globales).
        public override void SetStaticDefaults()
        {
            // Movimiento
            MountData.jumpHeight = 8; // Altura máxima de salto.
            MountData.acceleration = 0.19f; // Velocidad con la que acelera.
            MountData.jumpSpeed = 4f; // Velocidad inicial al saltar.
            MountData.blockExtraJumps = false; // Permite usar doble salto (si se tiene).
            MountData.constantJump = true; // Mantén presionado para saltar constantemente.
            MountData.heightBoost = 20; // Distancia del jugador al suelo.
            MountData.fallDamage = 0f; // Reducción del daño por caída.
            MountData.runSpeed = 11f; // Velocidad al correr.
            MountData.dashSpeed = 8f; // Velocidad al impulsarse.
            MountData.flightTimeMax = 600; // No puede volar.

            // Misceláneo
            MountData.fatigueMax = 0; // Sin fatiga.
            MountData.buff = ModContent.BuffType<FloppMountBuff>(); // Asocia un buff a la montura.

            // Efectos visuales
            //MountData.spawnDust = ModContent.DustType<Dusts.Sparkle>(); // Polvo al montar/desmontar.

            // Animaciones
            MountData.totalFrames = 12;
            MountData.heightBoost = 20;
            int[] array = new int[MountData.totalFrames];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = 15;
            }
            MountData.playerYOffsets = array;
            //MountData.playerHeadOffset = MountData.heightBoost;
            MountData.xOffset = 1;
            MountData.yOffset = 0;
            MountData.playerHeadOffset = 0;
            MountData.bodyFrame = 3;

            // Datos de animaciones específicas (correr, volar, estar parado, etc.).
            MountData.standingFrameCount = 1;
            MountData.standingFrameDelay = 12;
            MountData.standingFrameStart = 0;

            MountData.runningFrameCount = 5;
            MountData.runningFrameDelay = 20;
            MountData.runningFrameStart = 1;

            MountData.flyingFrameCount = 4;
            MountData.flyingFrameDelay = 7;
            MountData.flyingFrameStart = 7;

            MountData.inAirFrameCount = 1;
            MountData.inAirFrameDelay = 11;
            MountData.inAirFrameStart = 8;

            MountData.idleFrameCount = 1;
            MountData.idleFrameDelay = 10;
            MountData.idleFrameStart = 0;
            MountData.idleFrameLoop = true;

            MountData.swimFrameCount = MountData.inAirFrameCount;
            MountData.swimFrameDelay = MountData.inAirFrameDelay;
            MountData.swimFrameStart = MountData.inAirFrameStart;



            // Configuración de texturas.
            if (!Main.dedServ)
            { // Si no es un servidor dedicado.
                MountData.textureWidth = MountData.backTexture.Width() + 20; // Ancho de textura.
                MountData.textureHeight = MountData.backTexture.Height(); // Alto de textura.
            }
        }

        // Efectos visuales y comportamiento.
        public override void UpdateEffects(Player player)
        {
            // Genera polvo si se mueve rápido.
            if (Math.Abs(player.velocity.X) > 4f)
            {
                Rectangle rect = player.getRect();
                Dust.NewDust(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, ModContent.DustType<Dusts.Sparkle>());
            }
        }

        // Configuración inicial al montar.
        public override void SetMount(Player player, ref bool skipDust)
        {
            // Genera un efecto visual personalizado al montar.
            if (!Main.dedServ)
            {
                for (int i = 0; i < 16; i++)
                {
                    Dust.NewDustPerfect(player.Center + new Vector2(80, 0).RotatedBy(i * Math.PI * 2 / 16f), MountData.spawnDust);
                }
                skipDust = true; // Omite el polvo estándar.
            }
        }
    }
}
