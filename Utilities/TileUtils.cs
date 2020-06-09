using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NoxiumMod.Utilities
{
    public static class TileUtils
    {
        public static T GetTileEntity<T>(int i, int j) where T : ModTileEntity
        {
            return GetTileEntity<T>(new Point16(i, j));
        }

        public static T GetTileEntity<T>(Point16 position) where T : ModTileEntity
        {
            if (TileEntity.ByPosition.TryGetValue(TileTopLeft(position), out TileEntity te))
                return (T)te;

            return null;
        }

        public static Point16 TileTopLeft(Point16 position)
        {
            if (position.X >= 0 && position.X <= Main.maxTilesX && position.Y >= 0 && position.Y <= Main.maxTilesY)
            {
                Tile tile = Main.tile[position.X, position.Y];

                int fX = 0;
                int fY = 0;

                if (tile != null)
                {
                    TileObjectData data = TileObjectData.GetTileData(tile.type, 0);

                    if (data != null)
                    {
                        fX = tile.frameX % (18 * data.Width) / 18;
                        fY = tile.frameY % (18 * data.Height) / 18;
                    }
                }

                return new Point16(position.X - fX, position.Y - fY);
            }

            return Point16.NegativeOne;
        }
        public static void PlaceMultitile(Point16 position, int type, int style = 0)
        {
            TileObjectData data = TileObjectData.GetTileData(type, style); //magic numbers and uneccisary params begone!

            if (position.X + data.Width > Main.maxTilesX || position.X < 0) return; //make sure we dont spawn outside of the world!
            if (position.Y + data.Height > Main.maxTilesY || position.Y < 0) return;

            int xVariants = 0;
            int yVariants = 0;
            if (data.StyleHorizontal) xVariants = Main.rand.Next(data.RandomStyleRange);
            else yVariants = Main.rand.Next(data.RandomStyleRange);

            for (int x = 0; x < data.Width; x++) //generate each column
            {
                for (int y = 0; y < data.Height; y++) //generate each row
                {
                    Tile tile = Framing.GetTileSafely(position.X + x, position.Y + y); //get the targeted tile
                    tile.type = (ushort)type; //set the type of the tile to our multitile

                    tile.frameX = (short)((x + data.Width * xVariants) * (data.CoordinateWidth + data.CoordinatePadding)); //set the X frame appropriately
                    tile.frameY = (short)((y + data.Height * yVariants) * (data.CoordinateHeights[y] + data.CoordinatePadding)); //set the Y frame appropriately
                    tile.active(true); //activate the tile
                }
            }
        }
    }
}