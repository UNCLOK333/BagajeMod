using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BagajeMod.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework;

namespace BagajeMod.Content.Tiles
{
    internal class CacAntorcha : ModTile
    {
        // Asset para la textura de la llama de la antorcha.
        private Asset<Texture2D> flameTexture;

        // Método para establecer las propiedades estáticas del tile.
        public override void SetStaticDefaults()
        {
            // Propiedades del tile.
            Main.tileLighted[Type] = true; // El tile emite luz.
            Main.tileFrameImportant[Type] = true; // El frame del tile es importante para su renderizado.
            Main.tileSolid[Type] = false; // El tile no es sólido.
            Main.tileNoAttach[Type] = true; // No se puede adjuntar a otros tiles.
            Main.tileNoFail[Type] = true; // No falla al colocarse.
            Main.tileWaterDeath[Type] = true; // Se destruye al entrar en contacto con agua.
            TileID.Sets.FramesOnKillWall[Type] = true; // Mantiene su frame al destruir la pared detrás.
            TileID.Sets.DisableSmartCursor[Type] = true; // Desactiva el cursor inteligente para este tile.
            TileID.Sets.DisableSmartInteract[Type] = true; // Desactiva la interacción inteligente.
            TileID.Sets.Torch[Type] = true; // Se comporta como una antorcha.

            DustType = ModContent.DustType<Sparkle>(); // Tipo de polvo que emite.
            AdjTiles = new int[] { TileID.Torches }; // Tiles con los que se comporta de manera similar.

            // Se añade al conjunto de tiles que cuentan como antorchas para las necesidades de las habitaciones.
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

            // Configuración de colocación del tile.
            TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.Torches, 0));
            /*  Esto es lo que se copia de los datos de las antorchas:
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleTorch);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
            TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
            TileObjectData.newAlternate.AnchorAlternateTiles = new int[] { 124, 561, 574, 575, 576, 577, 578 };
            TileObjectData.addAlternate(1);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
            TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
            TileObjectData.newAlternate.AnchorAlternateTiles = new int[] { 124, 561, 574, 575, 576, 577, 578 };
            TileObjectData.addAlternate(2);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
            TileObjectData.newAlternate.AnchorWall = true;
            TileObjectData.addAlternate(0);
            */

            // Este código añade propiedades específicas al estilo 1, utilizado por ExampleWaterTorch.
            // Permite que el tile se coloque en líquidos.
            TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
            TileObjectData.newSubTile.LinkedAlternates = true;
            TileObjectData.newSubTile.WaterDeath = false;
            TileObjectData.newSubTile.LavaDeath = false;
            TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
            TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
            TileObjectData.addSubTile(1);

            // Se añade el tile al juego.
            TileObjectData.addTile(Type);

            // Configuración del mapa.
            AddMapEntry(new Color(200, 200, 200), Language.GetText("ItemName.Torch"));

            // Carga de la textura de la llama.
            flameTexture = ModContent.Request<Texture2D>(Texture + "_Flame");
        }

        // Método que se ejecuta cuando el jugador pasa el cursor sobre el tile.
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2; // Evita que el jugador lance el objeto accidentalmente.
            player.cursorItemIconEnabled = true; // Muestra el ícono del objeto en el cursor.

            // Determina el ítem que se muestra en el cursor según el estilo del tile.
            int style = TileObjectData.GetTileStyle(Main.tile[i, j]);
            player.cursorItemIconID = TileLoader.GetItemDropFromTypeAndStyle(Type, style);
        }

        // Método que determina la suerte que aporta la antorcha al jugador.
        //public override float GetTorchLuck(Player player)
        //{
            // Se llama cuando hay una ExampleTorch cerca del jugador.
            // Devuelve 1f para buena suerte, -1f para mala suerte, o valores intermedios para efectos menores.
            //bool inExampleUndergroundBiome = player.InModBiome<ExampleUndergroundBiome>();
            //return inExampleUndergroundBiome ? 1f : -0.1f; // Da máxima suerte en el bioma personalizado, de lo contrario, una pequeña penalización.
        //}

        // Método que determina la cantidad de polvo que emite la antorcha al romperse.
        public override void NumDust(int i, int j, bool fail, ref int num) => num = Main.rand.Next(1, 3);

        // Método que modifica la luz emitida por la antorcha.
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];

            // Si la antorcha está encendida
            if (tile.TileFrameX < 66)
            {
                // Establece el color de la luz a rosa
                r = 1.0f; // Componente roja
                g = 0.5f; // Componente verde
                b = 0.8f; // Componente azul
            }
        }


        // Método que ajusta la posición de dibujo del tile.
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            // This code slightly lowers the draw position if there is a solid tile above, so the flame doesn't overlap that tile. Terraria torches do this same logic.
            offsetY = 0;

            if (WorldGen.SolidTile(i, j - 1))
            {
                offsetY = 4;
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            var tile = Main.tile[i, j];

            if (!TileDrawing.IsVisible(tile))
            {
                return;
            }

            // The following code draws multiple flames on top our placed torch.

            int offsetY = 0;

            if (WorldGen.SolidTile(i, j - 1))
            {
                offsetY = 4;
            }

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(uint)i); // Don't remove any casts.
            Color color = new Color(100, 100, 100, 0);
            int width = 20;
            int height = 20;
            int frameX = tile.TileFrameX;
            int frameY = tile.TileFrameY;
            int style = TileObjectData.GetTileStyle(Main.tile[i, j]);
            if (style == 1)
            {
                // ExampleWaterTorch should be a bit greener.
                color.G = 255;
            }

            for (int k = 0; k < 7; k++)
            {
                float xx = Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
                float yy = Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;

                spriteBatch.Draw(flameTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + xx, j * 16 - (int)Main.screenPosition.Y + offsetY + yy) + zero, new Rectangle(frameX, frameY, width, height), color, 0f, default, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
