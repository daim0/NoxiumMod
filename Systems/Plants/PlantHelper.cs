using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ObjectData;

namespace NoxiumMod.Systems.Plants
{
	public static class PlantHelper
	{
		public static Stage CurrentStage(int i, int j)
		{
			return CurrentStage(Main.tile[i, j]);
		}

		public static Stage CurrentStage(Tile tile)
		{
			if (tile != null && tile.active())
			{
				TileObjectData data = TileObjectData.GetTileData(tile); //grabs the TileObjectData associated with our tile. So we dont have to use as many magic numbers

				int fullFrameWidth = data.Width * (data.CoordinateWidth + data.CoordinatePadding); //the width of a full frame of our multitile in pixels. We get this by multiplying the size of 1 full frame with padding by the width of our tile in tiles.

				return (Stage)(tile.frameX / (fullFrameWidth)); //return the x frame / the full frame width * the amount of stages cast to the Stage enum
			}

			return Stage.Planted;
		}

		public static void Progress(int i, int j, TileObjectData data)
		{
			if (data != null)
			{
				for (int x = 0; x < data.Width; x++) //this for loop iterates through every COLUMN of the multitile, starting on the top-left.
				{
					for (int y = 0; y < data.Height; y++) //this for loop iterates through every ROW of the multitile, starting on the top-left.
					{
						//These 2 for loops together iterate through every specific tile in the multitile, allowing you to move each one's frame
						Tile targetTile = Main.tile[i + x, j + y]; //find the tile we are targeting by adding the offsets we find via the for loops to the coordinates of the top-left tile.
						targetTile.frameY = (short)(data.Width * (data.CoordinateWidth + data.CoordinatePadding) * 4); //adds the width of the frame to that specific tile's frame. this should push it forward by one full frame of your multitile sprite. cast to short because vanilla.
					}
				}
			}
		}

		public static void Kill(int i, int j, TileObjectData data)
		{
			if (data != null)
			{
				for (int x = 0; x < data.Width; x++) //this for loop iterates through every COLUMN of the multitile, starting on the top-left.
				{
					for (int y = 0; y < data.Height; y++) //this for loop iterates through every ROW of the multitile, starting on the top-left.
					{
						//These 2 for loops together iterate through every specific tile in the multitile, allowing you to move each one's frame
						Tile targetTile = Main.tile[i + x, j + y]; //find the tile we are targeting by adding the offsets we find via the for loops to the coordinates of the top-left tile.

						targetTile.frameY += (short)(data.Width * (data.CoordinateWidth + data.CoordinatePadding)); //adds the width of the frame to that specific tile's frame. this should push it forward by one full frame of your multitile sprite. cast to short because vanilla.
					}
				}
			}
		}

		public static void Water<T>(int i, int j) where T : Plant
		{
			PlantEntity<T> plantEntity = (PlantEntity<T>)TileEntity.ByPosition[new Point16(i, j)];
			plantEntity.hasBeenWatered = true;
		}

		public static void ProgressWithEffects(Point16 pos, TileObjectData data, int soundType = SoundID.Grass, int dustType = DustID.Grass)
		{
			Progress(pos.X, pos.Y, data);

			Main.PlaySound(soundType, pos.X * 16 + data.Width / 2, pos.Y * 16 + data.Height / 2); //play grass sound at tile pos converted to pixel pos

			//Spawn some grass dust at the tiles position
			Dust dust = Dust.NewDustDirect(new Vector2(pos.X * 16 + data.Width / 2, pos.Y * 16 + data.Height / 2), 16, 16, dustType);

			dust.noGravity = false; //make the dust fall
			dust.fadeIn = 0.3f; //make the dust fade in
		}
	}
}