
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Tiles.Plasma
{
    class HugeTree : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
            TileObjectData.newTile.Height = 16;
            TileObjectData.newTile.Width = 12;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 ,16 };
            Main.tileWaterDeath[Type] = false;
            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.addTile(Type);
            disableSmartCursor = true;
            mineResist = 10f;
            minPick = 1000;
        }
    }
}
