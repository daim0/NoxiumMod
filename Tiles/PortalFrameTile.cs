using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Tiles
{
    class PortalFrameTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Width = 5;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            Main.tileWaterDeath[Type] = false;
            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.newTile.Origin = new Point16(0, 2 - 1);
            TileObjectData.addTile(Type);
            disableSmartCursor = true;
        }
        /*TODO:

        Aight well I have no idea how to do this, but when the tile is placed a portal must spawn above it, portal sprite is on the discord, it is animated
        This portal would be right clickable and once right clicked a UI menu will pop out.
        Portal must be aligned with the middle of the tile and despawn once the tile is broken
        I supposed it could be an npc? Idk whatever works for the portal.
         */
    }
}
