using NoxiumMod.Items.Plants;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NoxiumMod.Systems.Plants
{
	public abstract class Plant : ModTile
	{
		public abstract int[] CoordinateHeights { get; }
		public abstract Point16 TileSize { get; }
		public abstract TileObjectData CopyData { get; }
		public abstract PlantEntity Entity { get; }
		public virtual int DustType { get; } = DustID.Grass;

		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true; //Tells the game that the frame of this tile cannot be randomized
			Main.tileLavaDeath[Type] = true;
			Main.tileWaterDeath[Type] = true;

			//Sets the appropriate TileObjectData for a 2x2 multitile.
			TileObjectData.newTile.CopyFrom(CopyData);

			TileObjectData.newTile.Width = TileSize.X; //width in tiles
			TileObjectData.newTile.Height = TileSize.Y; //height in tiles
			TileObjectData.newTile.CoordinateHeights = CoordinateHeights; //height of each tile frame in the multitile complex in pixels
			TileObjectData.newTile.UsesCustomCanPlace = true; //Tells the game that this tile is +placed as a multitile for the purpose of createTile in items.
			TileObjectData.newTile.CoordinateWidth = 16; //width of each tile frame in the multitile complex in pixels
			TileObjectData.newTile.Origin = new Point16(0, TileObjectData.newTile.Height - 1); //where the tile is placed from for the purpose of createTile in items. (1, 1) would make the tile place from the top left of the bottom right tile instead

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

			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed; //wheather this can be placed in water
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed; //wheather this can be placed in lava

			//Used for the tle entity
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Entity.Hook_AfterPlacement, -1, 0, false);

			TileObjectData.addTile(Type); //Adds the data to this type of tile. Make sure you call this after setting everything else.

			////////////////////			ModTileInstance.AddMapEntry(new Color(212, 34, 52)); //the color of your tile on the map.

			dustType = DustType; //the dust your tile gives off when its broken.
			disableSmartCursor = true; //Reccomended for multitiles.
		}

		public override bool Drop(int i, int j)
		{
			Tile tile = Main.tile[i, j]; // a variable to make our code look cleaner

			TileObjectData data = TileObjectData.GetTileData(tile); //grabs the TileObjectData associated with our tile. So we dont have to use as many magic numbers
			int fullFrameWidth = data.Width * (data.CoordinateWidth + data.CoordinatePadding); //the width of a full frame of our multitile in pixels. We get this by multiplying the size of 1 full frame with padding by the width of our tile in tiles.

			if (PlantHelper.CurrentStage(i, j) == Stage.Grown && tile.frameY == 0 && tile.frameX % fullFrameWidth == 0) //Check if the current stage is fully grown and if this is the top left tile
				Item.NewItem(i * 16, j * 16, 16, 16, ModContent.ItemType<Strawberry>(), Main.rand.Next(2, 5)); //spawn item in stacks of 2-4

			return false;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Main.PlaySound(SoundID.Grass, i * 16, j * 16); //play grass sound at tile pos converted to pixel pos
		}
	}

	public abstract class Plant<T> : Plant where T : PlantEntity
	{
		public override PlantEntity Entity => ModContent.GetInstance<T>();
	}
}