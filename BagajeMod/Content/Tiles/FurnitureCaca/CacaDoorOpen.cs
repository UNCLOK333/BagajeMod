using BagajeMod.Content.Dusts;
using BagajeMod.Content.Items.Placeable.Furniture;
using BagajeMod.Content.Items.Placeable.FurnitureCaca;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BagajeMod.Content.Tiles.FurnitureCaca
{
    public class CacaDoorOpen : ModTile
    {
        public override void SetStaticDefaults()
        {
            // Propiedades del bloque
            Main.tileFrameImportant[Type] = true; // Indica que los marcos (frames) de este bloque son importantes y deben manejarse cuidadosamente.
            Main.tileSolid[Type] = false; // El bloque no se considera sólido (los jugadores y NPCs pueden pasar a través de él).
            Main.tileLavaDeath[Type] = true; // El bloque se destruye si entra en contacto con lava.
            Main.tileNoSunLight[Type] = true; // Este bloque no permitirá que pase la luz del sol.
            TileID.Sets.HousingWalls[Type] = true; // Es necesario para que los bloques no sólidos cuenten como paredes en las condiciones de vivienda.
            TileID.Sets.HasOutlines[Type] = true; // Permite que este bloque tenga contornos visibles cuando el cursor lo señala.
            TileID.Sets.DisableSmartCursor[Type] = true; // Desactiva el uso del cursor inteligente sobre este bloque.
            TileID.Sets.CloseDoorID[Type] = ModContent.TileType<CacaDoorClosed>(); // Especifica el ID del bloque asociado cuando la puerta se cierra.

            // Agregar este bloque a la lista de objetos necesarios para contar como puerta en una vivienda.
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);

            DustType = ModContent.DustType<Sparkle>(); // Especifica el tipo de polvo (efecto visual) que se generará al interactuar con este bloque.
            AdjTiles = new int[] { TileID.OpenDoor }; // Indica que este bloque se comporta como una puerta abierta en términos de funcionalidad.

            // Los bloques usualmente generan automáticamente el objeto correspondiente al romperse, pero necesitamos registrar explícitamente
            // el objeto que se deja caer, ya que ExampleDoorClosed es la puerta colocada, no este bloque.
            RegisterItemDrop(ModContent.ItemType<CacaDoor>(), 0);

            // Nombres
            AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.Door")); // Agrega una entrada al mapa con el nombre "Door" y un color gris claro.

            // Configuración de colocación del bloque
            // TileID.OpenDoor tiene valores incorrectos en los anclajes y StyleMultiplier, así que no copiamos de él.
            TileObjectData.newTile.Width = 2; // Ancho de 2 bloques.
            TileObjectData.newTile.Height = 3; // Altura de 3 bloques.
            TileObjectData.newTile.Origin = new Point16(0, 0); // Define el punto de origen para colocar el bloque.
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 0); // El anclaje superior requiere un bloque sólido.
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0); // El anclaje inferior también requiere un bloque sólido.
            TileObjectData.newTile.UsesCustomCanPlace = true; // Permite personalizar las reglas de colocación.
            TileObjectData.newTile.LavaDeath = true; // El bloque se destruye con lava.
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 }; // Altura de los tres segmentos del sprite (16 píxeles cada uno).
            TileObjectData.newTile.CoordinateWidth = 16; // Ancho de cada segmento del sprite.
            TileObjectData.newTile.CoordinatePadding = 2; // Espaciado entre segmentos del sprite.
            TileObjectData.newTile.StyleHorizontal = true; // Los estilos están organizados horizontalmente en la hoja de sprites.
            TileObjectData.newTile.StyleMultiplier = 2; // Cada estilo ocupa 2 sprites horizontalmente.
            TileObjectData.newTile.StyleWrapLimit = 2; // El límite de estilos envueltos es 2, lo que significa que los estilos adicionales estarán en una nueva fila.
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceRight; // Por defecto, la puerta se coloca hacia la derecha.

            // Agrega alternativos (variaciones de colocación)
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(0, 1); // Punto de origen diferente para esta variación.
            TileObjectData.addAlternate(0);

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(0, 2); // Otra variación con origen diferente.
            TileObjectData.addAlternate(0);

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(1, 0);
            TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
            TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft; // Esta variación coloca la puerta hacia la izquierda.
            TileObjectData.addAlternate(1);

            // Continúa agregando más variaciones para cubrir todas las combinaciones posibles.
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(1, 1);
            TileObjectData.addAlternate(1);

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(1, 2);
            TileObjectData.addAlternate(1);

            // Finaliza la configuración del bloque
            TileObjectData.addTile(Type);
        }

        // Habilita la interacción inteligente con el bloque
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        // Controla la cantidad de polvo generado al interactuar con el bloque
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1; // Genera solo una partícula de polvo.
        }

        // Configura cómo se comporta el cursor al pasar sobre el bloque
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2; // Previene que el jugador arroje accidentalmente el objeto mientras interactúa.
            player.cursorItemIconEnabled = true; // Activa el icono del objeto en el cursor.
            player.cursorItemIconID = ModContent.ItemType<CacaDoor>(); // Muestra el icono del objeto asociado con la puerta.
        }
    }
}
