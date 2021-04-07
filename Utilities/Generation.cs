using Terraria;

namespace NoxiumMod.Utilities
{
	public static class Generation
	{
		public const int SmallWorldWidth = 4200;
		public const int SmallWorldHeight = 1200;
		public const int MediumWorldWidth = (int)(SmallWorldWidth * 1.5f);
		public const int MediumWorldHeight = (int)(SmallWorldHeight * 1.5f);
		public const int LargeWorldWidth = SmallWorldWidth * 2;
		public const int LargeWorldHeight = SmallWorldHeight * 2;

		public static void HideUndergroundLayers()
		{
			Main.worldSurface = Main.maxTilesY - 42;
			Main.rockLayer = Main.maxTilesY;
		}

		public static void FillRectangle(int x, int y, int width, int height, ushort type)
		{
			for (int i = x; i < x + width; i++)
			{
				for (int j = y; j < y + height; j++)
				{
					Main.tile[i, j].active(true);
					Main.tile[i, j].type = type;
				}
			}
		}
	}
}
