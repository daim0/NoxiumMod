using NoxiumMod.Utilities;
using System;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace NoxiumMod.Dimensions
{
    class WaterTileRunner : TileRunner
    {
        public WaterTileRunner(Vector2 pos, Vector2 speed, Point16 hRange, Point16 vRange, double strength, int steps, ushort type, bool addTile, bool overRide) : base(pos, speed, hRange, vRange, strength, steps, type, addTile, overRide)
        {
        }
        public override void ChangeTile(Tile tile)
        {
            tile.active(false);
            tile.liquidType(0);

            tile.liquid = 255;
        }
    }
}
