using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

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
    }
}
