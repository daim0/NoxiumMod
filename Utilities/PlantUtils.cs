using Terraria;
using Terraria.ObjectData;

namespace NoxiumMod.Utilities
{
	public static class PlantUtils
	{
		public static Stage CurrentStage(int i, int j, byte amountOfStages) => CurrentStage(Main.tile[i, j], amountOfStages);

		public static Stage CurrentStage(Tile tile, byte amountOfStages)
		{
			TileObjectData data = TileObjectData.GetTileData(tile); //grabs the TileObjectData associated with our tile. So we dont have to use as many magic numbers

			int fullFrameWidth = data.Width * (data.CoordinateWidth + data.CoordinatePadding); //the width of a full frame of our multitile in pixels. We get this by multiplying the size of 1 full frame with padding by the width of our tile in tiles.

			return (Stage)(tile.frameX / (fullFrameWidth)); //return the x frame / the full frame width * the amount of stages cast to the Stage enum
		}
	}

	public enum Stage
	{
		Planted,
		Growing,
		Grown
	}
}