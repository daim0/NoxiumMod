using Microsoft.Xna.Framework;
using NoxiumMod.Items.Plants;
using NoxiumMod.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NoxiumMod.Tiles.Plants
{
	public class StrawberryPlantTile : ModTile
	{
		public bool UpdateChance { get; private set; }

		public override void SetDefaults()
		{
			UpdateChance = Main.rand.Next(0) == 0; //Set the random chance to use later on

			Main.tileFrameImportant[Type] = true; //Tells the game that the frame of this tile cannot be randomized

			//Sets the appropriate TileObjectData for a 2x2 multitile.
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);

			TileObjectData.newTile.Width = 2; //width in tiles
			TileObjectData.newTile.Height = 2; //height in tiles
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 }; //height of each tile frame in the multitile complex in pixels
			TileObjectData.newTile.UsesCustomCanPlace = true; //Tells the game that this tile is placed as a multitile for the purpose of createTile in items.
			TileObjectData.newTile.CoordinateWidth = 16; //width of each tile frame in the multitile complex in pixels
			TileObjectData.newTile.CoordinatePadding = 2; //spacing between each frame in pixels
			TileObjectData.newTile.Origin = new Point16(0, 1); //where the tile is placed from for the purpose of createTile in items. (1, 1) would make the tile place from the top left of the bottom right tile instead

			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				TileID.Grass,
				TileID.Dirt
			};

			TileObjectData.newTile.AnchorAlternateTiles = new int[]
			{
				TileID.ClayPot,
				TileID.PlanterBox
			};

			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed; //weather this can be placed in water
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed; //weather this can be placed in lava

			TileObjectData.addTile(Type); //Adds the data to this type of tile. Make sure you call this after setting everything else.

			AddMapEntry(new Color(212, 34, 52)); //the color of your tile on the map.

			dustType = DustID.Grass; //the dust your tile gives off when its broken.
			disableSmartCursor = true; //Reccomended for multitiles.
		}

		//REMOVE LATER. USED FOR TESTING PURPOSES
		public override bool NewRightClick(int i, int j)
		{
			RandomUpdate(i, j); //calls random update growing the plant to the next stage

			return true;
		}

		//RandomUpdate is vanilla's shitty ass way of handling having the entire world loaded at once. a bunch of tiles update every tick at pure random. thanks redcode.
		public override void RandomUpdate(int i, int j)
		{
			Tile tile = Main.tile[i, j]; //you could probably add more safety checks if you want to be extra giga secure, but we assume RandomUpdate only calls valid tiles here
			TileObjectData data = TileObjectData.GetTileData(tile); //grabs the TileObjectData associated with our tile. So we dont have to use as many magic numbers
			int fullFrameWidth = data.Width * (data.CoordinateWidth + data.CoordinatePadding); //the width of a full frame of our multitile in pixels. We get this by multiplying the size of 1 full frame with padding by the width of our tile in tiles.

			if (tile.frameY == 0 && tile.frameX % fullFrameWidth == 0 && PlantUtils.CurrentStage(i, j, 3) != Stage.Grown) //this checks to make sure this is only the top-left tile. We only want one tile to do all the growing for us, and top-left is the standard. otherwise each tile in the multitile ticks on its own due to stupid poopoo redcode. this also checks if the current stage isnt the fully grown stage to stop occurences of tiles disapearing after they grow to the invisible stage after grown
			{
				if (UpdateChance) //a random check here can slow growing as much as you want.
				{
					for (int x = 0; x < data.Width; x++) //this for loop iterates through every COLUMN of the multitile, starting on the top-left.
					{
						for (int y = 0; y < data.Height; y++) //this for loop iterates through every ROW of the multitile, starting on the top-left.
						{
							//These 2 for loops together iterate through every specific tile in the multitile, allowing you to move each one's frame
							Tile targetTile = Main.tile[i + x, j + y]; //find the tile we are targeting by adding the offsets we find via the for loops to the coordinates of the top-left tile.

							targetTile.frameX += (short)fullFrameWidth; //adds the width of the frame to that specific tile's frame. this should push it forward by one full frame of your multitile sprite. cast to short because vanilla.

							Main.PlaySound(SoundID.Grass, i * 16, j * 16); //play grass sound at tile pos converted to pixel pos

							//Spawn some grass dust at the tiles position
							Dust dust = Main.dust[Dust.NewDust(new Vector2(i * 16 + 16, j * 16 + 16), 16, 16, DustID.Grass)];

							dust.noGravity = false; //make the dust fall
							dust.fadeIn = 0.3f; //make the dust fade in
						}
					}
				}
			}
		}

		public override bool Drop(int i, int j)
		{
			Tile tile = Main.tile[i, j]; // a variable to make our code look cleaner

			TileObjectData data = TileObjectData.GetTileData(tile); //grabs the TileObjectData associated with our tile. So we dont have to use as many magic numbers
			int fullFrameWidth = data.Width * (data.CoordinateWidth + data.CoordinatePadding); //the width of a full frame of our multitile in pixels. We get this by multiplying the size of 1 full frame with padding by the width of our tile in tiles.

			if (PlantUtils.CurrentStage(i, j, 3) == Stage.Grown && tile.frameY == 0 && tile.frameX % fullFrameWidth == 0) //Check if the current stage is fully grown and if this is the top left tile
				Item.NewItem(i * 16, j * 16, 16, 16, ModContent.ItemType<Strawberry>(), Main.rand.Next(2, 5)); //spawn item in stacks of 2-4

			return false;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Main.PlaySound(SoundID.Grass, i * 16, j * 16); //play grass sound at tile pos converted to pixel pos
	}
}