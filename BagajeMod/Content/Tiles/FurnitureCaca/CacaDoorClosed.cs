using BagajeMod.Content.Dusts;
using BagajeMod.Content.Items.Placeable.Furniture; // Importa los objetos relacionados con muebles del mod.
using Microsoft.Xna.Framework; // Proporciona herramientas para trabajar con colores, vectores, etc.
using Terraria; // Contiene el núcleo del juego Terraria.
using Terraria.GameContent.ObjectInteractions; // Permite manejar interacciones con objetos como puertas.
using Terraria.ID; // Incluye identificadores para varios objetos, tiles, etc.
using Terraria.Localization; // Maneja la localización de textos para diferentes idiomas.
using Terraria.ModLoader; // Herramientas para crear mods en Terraria.
using Terraria.ObjectData; // Maneja configuraciones de tiles y su comportamiento.

namespace BagajeMod.Content.Tiles.FurnitureCaca
{
    // Puede que hayas notado que ExampleDoorClosed.png tiene 3 copias del mismo sprite. Esto permite una variación aleatoria.
    // Cada vez que una puerta se cierra, la lógica del juego selecciona al azar un sprite diferente para cada uno de los 3 tiles.
    // La variación aleatoria debe ser muy sutil. Cada tile se randomiza de forma independiente.
    // Todas las puertas hacen esto, pero esta funcionalidad puede ignorarse repitiendo el mismo sprite 3 veces, como en este ejemplo.
    public class CacaDoorClosed : ModTile
    {
        public override void SetStaticDefaults()
        {
            // Propiedades del tile
            Main.tileFrameImportant[Type] = true; // Indica que los frames del tile son importantes (evita mezclas incorrectas).
            Main.tileBlockLight[Type] = true; // El tile bloquea la luz.
            Main.tileSolid[Type] = true; // El tile es sólido.
            Main.tileNoAttach[Type] = true; // No permite que se adjunten otros tiles.
            Main.tileLavaDeath[Type] = true; // El tile se destruye al contacto con lava.
            TileID.Sets.NotReallySolid[Type] = true; // No se considera un tile completamente sólido (como las puertas cerradas).
            TileID.Sets.DrawsWalls[Type] = true; // Permite dibujar paredes detrás del tile.
            TileID.Sets.HasOutlines[Type] = true; // Habilita el contorno resaltado del tile al interactuar con él.
            TileID.Sets.DisableSmartCursor[Type] = true; // Desactiva el uso del cursor inteligente sobre este tile.
            TileID.Sets.OpenDoorID[Type] = ModContent.TileType<CacaDoorOpen>(); // Define el tile correspondiente cuando la puerta está abierta.

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor); // Añade este tile a los requisitos de una habitación válida como puerta.

            DustType = ModContent.DustType<Sparkle>(); // Define el tipo de partículas generadas al interactuar con el tile.
            AdjTiles = new int[] { TileID.ClosedDoor }; // Define que este tile es equivalente a una puerta cerrada estándar.

            // Nombres
            AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.Door")); // Añade un nombre y color al tile en el mapa.

            // Configuración de colocación del tile
            // Además de copiar de las plantillas TileObjectData, los modders pueden copiar de tipos específicos de tiles. CopyFrom no copia datos secundarios del tile, por lo que no se copiarán propiedades específicas como la inmunidad a la lava de las puertas de obsidiana.
            TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.ClosedDoor, 0));
            /* Esto es lo que se copia del tile ClosedDoor
			TileObjectData.newTile.Width = 1; // Ancho del tile: 1 bloque.
			TileObjectData.newTile.Height = 3; // Altura del tile: 3 bloques.
			TileObjectData.newTile.Origin = new Point16(0, 0); // Origen del tile (esquina superior izquierda).
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0); // Requiere un tile sólido arriba.
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0); // Requiere un tile sólido abajo.
			TileObjectData.newTile.UsesCustomCanPlace = true; // Usa lógica personalizada para colocación.
			TileObjectData.newTile.LavaDeath = true; // Puede ser destruido por la lava.
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 }; // Altura de cada frame (16 píxeles por bloque).
			TileObjectData.newTile.CoordinateWidth = 16; // Ancho de cada frame (16 píxeles).
			TileObjectData.newTile.CoordinatePadding = 2; // Espaciado entre frames.
			TileObjectData.newTile.StyleHorizontal = false; // No permite estilos horizontales.
			TileObjectData.newTile.StyleWrapLimit = 36; // Número máximo de estilos permitidos antes de envolver.
			TileObjectData.newTile.StyleLineSkip = 3; // Permite que las 3 variaciones del sprite se interpreten correctamente.
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile); // Crea una configuración alternativa para el tile.
			TileObjectData.newAlternate.Origin = new Point16(0, 1); // Configura el origen de la segunda variante.
			TileObjectData.addAlternate(0); // Añade la variante al tile.
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile); // Configura una tercera variante.
			TileObjectData.newAlternate.Origin = new Point16(0, 2); // Configura el origen de la tercera variante.
			TileObjectData.addAlternate(0); // Añade la tercera variante al tile.
			*/
            TileObjectData.addTile(Type); // Registra el tile con esta configuración.
        }

        // Permite la interacción inteligente (Smart Cursor) con este tile.
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true; // Siempre permite el uso del cursor inteligente.
        }

        // Controla la cantidad de partículas generadas al interactuar con el tile.
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1; // Genera 1 partícula, independientemente del resultado.
        }

        // Maneja el comportamiento del mouse al pasar sobre el tile.
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer; // Obtiene al jugador local.
            player.noThrow = 2; // Previene que el jugador tire accidentalmente objetos mientras interactúa con el tile.
            player.cursorItemIconEnabled = true; // Activa el icono del objeto en el cursor.
            player.cursorItemIconID = ModContent.ItemType<Items.Placeable.FurnitureCaca.CacaDoor>(); // Muestra el ícono del objeto asociado a este tile.
        }
    }
}
