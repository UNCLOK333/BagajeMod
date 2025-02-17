﻿using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Microsoft.Xna.Framework.Input.Keys;
using BagajeMod.Content.Tiles.Furniture.Paintings;

namespace BagajeMod.Content.Items.Placeable.Furniture.Paintings
{
    public class DeidadDurmienteI : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Placeables";
        public const int DropInt = 100;

        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 32;
            Item.maxStack = 9999;

            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.consumable = true;
            Item.value = Item.buyPrice(0, 2, 0, 0); ;
            Item.rare = ItemRarityID.White;

            Item.createTile = ModContent.TileType<DeidadDurmienteC>();
        }
    }
}