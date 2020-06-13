using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using NoxiumMod.Utilities;
using Terraria.DataStructures;

namespace NoxiumMod.Dimensions
{
    class GenCloud
    {
        public static void GenIsland(Point topCentre, int size, int type)
        {
            for (int i = -size / 2; i < size / 2; ++i)
            {
                int repY = (size / 2) - (Math.Abs(i));
                int offset = repY / 5;
                repY += WorldGen.genRand.Next(4);
                for (int j = -offset; j < repY; ++j)
                {
                    WorldGen.PlaceTile(topCentre.X + i, topCentre.Y + j, type);
                }
            }
        }
        public static void makeOvalFlatTop(Vector2 startingPoint, int width, int height, int type)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (OvalCheck((int)(startingPoint.X + width / 2), (int)(startingPoint.Y + height / 2), i + (int)startingPoint.X, j + (int)startingPoint.Y, (int)(width * .5f), (int)(height * .5f)))
                        WorldGen.PlaceTile(i + (int)startingPoint.X, j + (int)startingPoint.Y, type);

                    if (i == width / 2 && j == height / 2)
                    {
                        WorldGen.TileRunner(i + (int)startingPoint.X, j + (int)startingPoint.Y + 2, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(10, 20), type, true, 0f, 0f, false, true);
                        //TileRunner cocksucker = new TileRunner(new Vector2(i + (int)startingPoint.X, j + (int)startingPoint.Y + 2),new Vector2(0,0),new Point16(0,0), new Point16(0,0), WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(10, 20), (ushort)type, true, true);
                        //cocksucker.Start();
                    }
                }
            }
            for (int i = 0; i < width; i++)
            {
                for (int j = -6; j < height / 2 - 2; j++)
                {
                    Tile tile = Framing.GetTileSafely(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                    if (tile.type == type)
                        WorldGen.KillTile(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                }
            }
        }
        private static bool OvalCheck(int h, int k, int x, int y, int a, int b)
        {
            double p = Math.Pow(x - h, 2) / Math.Pow(a, 2)
                    + Math.Pow(y - k, 2) / Math.Pow(b, 2);

            return p < 1 ? true : false;
        }
    }
}
