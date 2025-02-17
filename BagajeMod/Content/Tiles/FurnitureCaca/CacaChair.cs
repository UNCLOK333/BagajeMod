using BagajeMod.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BagajeMod.Content.Tiles.FurnitureCaca
{
    public class CacaChair : ModTile
    {
        public const int NextStyleHeight = 40; // Calculado sumando todas las alturas de coordenadas (CoordinateHeights) más el padding de coordenadas (CoordinatePaddingFix.Y) aplicado a todas, y agregando 2.

        public override void SetStaticDefaults()
        {
            // Propiedades básicas del objeto
            Main.tileFrameImportant[Type] = true; // Define que la orientación del marco del tile es importante.
            Main.tileNoAttach[Type] = true; // Este tile no puede adjuntarse a otros.
            Main.tileLavaDeath[Type] = true; // El tile se destruye si entra en contacto con lava.
            TileID.Sets.HasOutlines[Type] = true; // Habilita un contorno interactivo cuando se selecciona con el cursor.
            TileID.Sets.CanBeSatOnForNPCs[Type] = true; // Permite que los NPC puedan sentarse en este tile.
            TileID.Sets.CanBeSatOnForPlayers[Type] = true; // Permite que los jugadores puedan sentarse en este tile.
            TileID.Sets.DisableSmartCursor[Type] = true; // Desactiva el uso del cursor inteligente para este tile.

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair); // Cuenta este tile como una silla para determinar si una habitación es adecuada para NPCs.

            DustType = ModContent.DustType<Sparkle>(); // Define el tipo de partículas (polvo) que genera este objeto.
            AdjTiles = new int[] { TileID.Chairs }; // Asocia este tile con las sillas estándar del juego.

            // Nombres y color en el minimapa
            AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.Chair"));

            // Configuración para la colocación
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2); // Usa un estilo predeterminado de 1x2 tiles.
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 }; // Define las alturas de los marcos en píxeles.
            TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, 2); // Ajusta el espacio vertical entre las coordenadas del marco.
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft; // Coloca el objeto inicialmente mirando hacia la izquierda.

            // Configuración para alternar estilos
            TileObjectData.newTile.StyleWrapLimit = 2; // Determina cuántos estilos pueden agruparse verticalmente.
            TileObjectData.newTile.StyleMultiplier = 2; // Multiplicador para determinar el rango de estilos.
            TileObjectData.newTile.StyleHorizontal = true; // Permite organizar los estilos horizontalmente.

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile); // Copia las propiedades de colocación.
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight; // Alterna el objeto para que mire hacia la derecha.
            TileObjectData.addAlternate(1); // Define que el segundo estilo de textura se utiliza cuando se coloca hacia la derecha.
            TileObjectData.addTile(Type); // Finaliza la configuración del tile.
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3; // Genera menos partículas si la colocación del objeto falla.
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance); // Permite interacción inteligente si el jugador está cerca.
        }

        public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info)
        {
            // Modifica la información del estado de descanso cuando un jugador o NPC se sienta.
            Tile tile = Framing.GetTileSafely(i, j);

            info.TargetDirection = -1; // Inicialmente, el jugador o NPC mira hacia la izquierda.
            if (tile.TileFrameX != 0)
            {
                info.TargetDirection = 1; // Cambia la dirección a la derecha si el objeto está colocado mirando hacia la derecha.
            }

            // Define la posición del ancla, que es la parte inferior del objeto.
            info.AnchorTilePosition.X = i; // La silla solo ocupa 1 tile de ancho, por lo que no necesita ajustes especiales.
            info.AnchorTilePosition.Y = j;

            // Ajusta la posición del ancla si el marco corresponde a la parte superior del objeto.
            if (tile.TileFrameY % NextStyleHeight == 0)
            {
                info.AnchorTilePosition.Y++; // Mueve el ancla hacia abajo en 1 tile si es necesario.
            }
        }

        public override bool RightClick(int i, int j)
        {
            // Lógica al hacer clic derecho sobre el objeto.
            Player player = Main.LocalPlayer;

            if (player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
            { // Evita que la interacción se dispare desde una distancia lejana.
                player.GamepadEnableGrappleCooldown(); // Configura un breve enfriamiento en los controles de mando.
                player.sitting.SitDown(player, i, j); // Activa la animación de sentarse.
            }

            return true;
        }

        public override void MouseOver(int i, int j)
        {
            // Lógica al pasar el cursor sobre el objeto.
            Player player = Main.LocalPlayer;

            if (!player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
            { // Asegura que la interacción solo se muestre si el jugador está cerca.
                return;
            }

            player.noThrow = 2; // Desactiva la posibilidad de lanzar objetos mientras se interactúa con este objeto.
            player.cursorItemIconEnabled = true; // Activa el ícono del cursor.
            player.cursorItemIconID = ModContent.ItemType<Items.Placeable.FurnitureCaca.CacaChair>(); // Muestra el ícono de la silla.

            if (Main.tile[i, j].TileFrameX / 18 < 1)
            {
                player.cursorItemIconReversed = true; // Invierte el ícono si el marco del tile está en una posición específica.
            }
        }
    }
}
