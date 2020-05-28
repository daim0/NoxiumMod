using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NoxiumMod.Tiles.Plasma
{
	internal class HugeTree : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.newTile.Height = 16;
			TileObjectData.newTile.Width = 12;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 };
			Main.tileWaterDeath[Type] = false;
			TileObjectData.newTile.WaterDeath = false;
			TileObjectData.addTile(Type);
			disableSmartCursor = true;
			mineResist = 10f;
			minPick = 1000;
		}
	}
}