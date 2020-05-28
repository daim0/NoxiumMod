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
    }
}