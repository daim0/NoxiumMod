/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoxiumMod.Systems.Plants
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

			//Used for the tle entity
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<StrawberryTileEntity>().Hook_AfterPlacement, -1, 0, false);

			TileObjectData.addTile(Type); //Adds the data to this type of tile. Make sure you call this after setting everything else.

			AddMapEntry(new Color(212, 34, 52)); //the color of your tile on the map.

			dustType = DustID.Grass; //the dust your tile gives off when its broken.
			disableSmartCursor = true; //Reccomended for multitiles.
		}

		//REMOVE LATER. USED FOR TESTING PURPOSES
		public override bool NewRightClick(int i, int j)
		{
			Tile tile = Main.tile[i, j]; //you could probably add more safety checks if you want to be extra giga secure, but we assume RandomUpdate only calls valid tiles here
			TileObjectData data = TileObjectData.GetTileData(tile); //grabs the TileObjectData associated with our tile. So we dont have to use as many magic numbers
			int fullFrameWidth = data.Width * (data.CoordinateWidth + data.CoordinatePadding); //the width of a full frame of our multitile in pixels. We get this by multiplying the size of 1 full frame with padding by the width of our tile in tiles.

			if (tile.frameY == 0 && tile.frameX % fullFrameWidth == 0)
				PlantUtils.Kill2x2(i, j, data, fullFrameWidth);

			return true;
		}

		//RandomUpdate is vanilla's shitty ass way of handling having the entire world loaded at once. a bunch of tiles update every tick at pure random. thanks redcode.
		public override void RandomUpdate(int i, int j)
		{
			Tile tile = Main.tile[i, j]; //you could probably add more safety checks if you want to be extra giga secure, but we assume RandomUpdate only calls valid tiles here
			TileObjectData data = TileObjectData.GetTileData(tile); //grabs the TileObjectData associated with our tile. So we dont have to use as many magic numbers
			int fullFrameWidth = data.Width * (data.CoordinateWidth + data.CoordinatePadding); //the width of a full frame of our multitile in pixels. We get this by multiplying the size of 1 full frame with padding by the width of our tile in tiles.

			if (tile.frameY == 0 && tile.frameX % fullFrameWidth == 0 && PlantUtils.CurrentStage(i, j) != Stage.Grown) //this checks to make sure this is only the top-left tile. We only want one tile to do all the growing for us, and top-left is the standard. otherwise each tile in the multitile ticks on its own due to stupid poopoo redcode. this also checks if the current stage isnt the fully grown stage to stop occurences of tiles disapearing after they grow to the invisible stage after grown
			{
				if (UpdateChance) //a random check here can slow growing as much as you want.
				{
					PlantUtils.Progress(i, j, data, fullFrameWidth);

					Main.PlaySound(SoundID.Grass, i * 16, j * 16); //play grass sound at tile pos converted to pixel pos

					//Spawn some grass dust at the tiles position
					Dust dust = Main.dust[Dust.NewDust(new Vector2(i * 16 + 16, j * 16 + 16), 16, 16, DustID.Grass)];

					dust.noGravity = false; //make the dust fall
					dust.fadeIn = 0.3f; //make the dust fade in
				}
			}
		}

		public override bool Drop(int i, int j)
		{
			Tile tile = Main.tile[i, j]; // a variable to make our code look cleaner

			TileObjectData data = TileObjectData.GetTileData(tile); //grabs the TileObjectData associated with our tile. So we dont have to use as many magic numbers
			int fullFrameWidth = data.Width * (data.CoordinateWidth + data.CoordinatePadding); //the width of a full frame of our multitile in pixels. We get this by multiplying the size of 1 full frame with padding by the width of our tile in tiles.

			if (PlantUtils.CurrentStage(i, j) == Stage.Grown && tile.frameY == 0 && tile.frameX % fullFrameWidth == 0) //Check if the current stage is fully grown and if this is the top left tile
				Item.NewItem(i * 16, j * 16, 16, 16, ModContent.ItemType<Strawberry>(), Main.rand.Next(2, 5)); //spawn item in stacks of 2-4

			return false;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Main.PlaySound(SoundID.Grass, i * 16, j * 16); //play grass sound at tile pos converted to pixel pos
	}

	public class StrawberryTileEntity : ModTileEntity
	{
		/// <summary>
		/// If the strawberry plant has been watered or not on the current day/
		/// </summary>
		public bool HasBeenWatered { get; private set; } = false;

		public Tile StrawberryPlant { get; private set; } = null;

		public int Position.X { get; private set; } = -1;

		public int J { get; private set; } = -1;

		public TileObjectData ObjectData { get; private set; } = null;

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			TileObjectData data = TileObjectData.GetTileData(type, style);

			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				Position.X = i - data.Origin.X;
				J = j - data.Origin.Y;
				StrawberryPlant = Main.tile[Position.X, J];

				return Place(i - data.Origin.X, j - data.Origin.Y);
			}

			NetMessage.SendTileRange(Main.myPlayer, i - data.Origin.X, j - data.Origin.Y, data.Width, data.Height);
			NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i - data.Origin.X, j - data.Origin.Y, Type);

			return -1;
		}
		public override bool ValidTile(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			return tile != null && tile.active() && tile.type == ModContent.TileType<StrawberryPlantTile>() && tile.frameX == 0 && tile.frameY == 0;
		}

		public override void Update()
		{
			if (StrawberryPlant != null && Position.X != -1 && J != -1 && ObjectData != null)
			{
				if (Main.dayTime && Main.time == 1)
				{
					if (!HasBeenWatered)
						PlantUtils.Kill2x2(Position.X, J, ObjectData, 36);

					HasBeenWatered = false;
				}

				if (!HasBeenWatered)
					PlantUtils.Progress(Position.X, J, ObjectData, (36 * 3) / ((int)PlantUtils.CurrentStage(Position.X, J) + 1));
				else
				{
					Tile tile = Main.tile[Position.X, J]; //you could probably add more safety checks if you want to be extra giga secure, but we assume RandomUpdate only calls valid tiles here
					TileObjectData data = TileObjectData.GetTileData(tile); //grabs the TileObjectData associated with our tile. So we dont have to use as many magic numbers
					int fullFrameWidth = data.Width * (data.CoordinateWidth + data.CoordinatePadding); //the width of a full frame of our multitile in pixels. We get this by multiplying the size of 1 full frame with padding by the width of our tile in tiles.

					if (tile.frameY == 0 && tile.frameX % fullFrameWidth == 0 && PlantUtils.CurrentStage(Position.X, J) != Stage.Grown) //this checks to make sure this is only the top-left tile. We only want one tile to do all the growing for us, and top-left is the standard. otherwise each tile in the multitile ticks on its own due to stupid poopoo redcode. this also checks if the current stage isnt the fully grown stage to stop occurences of tiles disapearing after they grow to the invisible stage after grown
					{
						PlantUtils.Progress(Position.X, J, data, fullFrameWidth);

						Main.PlaySound(SoundID.Grass, Position.X * 16, J * 16); //play grass sound at tile pos converted to pixel pos

						//Spawn some grass dust at the tiles position
						Dust dust = Main.dust[Dust.NewDust(new Vector2(Position.X * 16 + 16, J * 16 + 16), 16, 16, DustID.Grass)];

						dust.noGravity = false; //make the dust fall
						dust.fadeIn = 0.3f; //make the dust fade in

					}
				}
			}
		}
	}

	public class EEPlantEntity : ModTileEntity
	{
		public bool UpdateFlag { get; set; }
		public ModTile ModTileInstance { get; private set; }
		public Func<int, int, bool> ValidPlantHook { get; set; }
		public Action UpdateHook { get; private set; }
		public Point16 TileSize { get; private set; }

		public EEPlantEntity(ModTile modTile, Point16 tileSize, Action updateHook)
		{
			ModTileInstance = modTile;
			ValidPlantHook = null;
			UpdateHook = updateHook;
			TileSize = tileSize;
			UpdateFlag = true;
		}

		public override void Update() => UpdateHook();

		public void SetDefaults()
		{
			Main.tileFrameImportant[ModTileInstance.Type] = true; //Tells the game that the frame of this tile cannot be randomized
			Main.tileLavaDeath[ModTileInstance.Type] = true;
			Main.tileWaterDeath[ModTileInstance.Type] = true;

			//Sets the appropriate TileObjectData for a 2x2 multitile.
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);

			TileObjectData.newTile.Width = TileSize.X; //width in tiles
			TileObjectData.newTile.Height = TileSize.Y; //height in tiles
			List<int> list = new List<int>();
			for (int i = 0; i < TileSize.X; i++)
				list.Add(16);
			TileObjectData.newTile.CoordinateHeights = list.ToArray(); //height of each tile frame in the multitile complex in pixels
			TileObjectData.newTile.UsesCustomCanPlace = true; //Tells the game that this tile is placed as a multitile for the purpose of createTile in items.
			TileObjectData.newTile.CoordinateWidth = 16; //width of each tile frame in the multitile complex in pixels
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

			//Used for the tle entity
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(.Hook_AfterPlacement, -1, 0, false);

			TileObjectData.addTile(ModTileInstance.Type); //Adds the data to this type of tile. Make sure you call this after setting everything else.

			////////////////////			ModTileInstance.AddMapEntry(new Color(212, 34, 52)); //the color of your tile on the map.

			ModTileInstance.dustType = DustID.Grass; //the dust your tile gives off when its broken.
			ModTileInstance.disableSmartCursor = true; //Reccomended for multitiles.
		}

		public bool Drop(int i, int j, int itemType, int amount = 1)
		{
			Tile tile = Main.tile[i, j]; // a variable to make our code look cleaner

			TileObjectData data = TileObjectData.GetTileData(tile); //grabs the TileObjectData associated with our tile. So we dont have to use as many magic numbers
			int fullFrameWidth = data.Width * (data.CoordinateWidth + data.CoordinatePadding); //the width of a full frame of our multitile in pixels. We get this by multiplying the size of 1 full frame with padding by the width of our tile in tiles.

			if (PlantUtils.CurrentStage(i, j) == Stage.Grown && tile.frameY == 0 && tile.frameX % fullFrameWidth == 0) //Check if the current stage is fully grown and if this is the top left tile
				Item.NewItem(i + (TileSize.X / 2) * 16, j + (TileSize.Y / 2) * 16, 16, 16, itemType, amount); //spawn item in stacks of 2-4

			return false;
		}

		public void KillMultiTile(int i, int j) => Main.PlaySound(SoundID.Grass, i + (TileSize.X / 2) * 16, j + (TileSize.Y / 2) * 16); //play grass sound at tile pos converted to pixel pos

		public void Grow(int i, int j)
		{
			Tile tile = Main.tile[i, j]; //you could probably add more safety checks if you want to be extra giga secure, but we assume RandomUpdate only calls valid tiles here
			TileObjectData data = TileObjectData.GetTileData(tile); //grabs the TileObjectData associated with our tile. So we dont have to use as many magic numbers
			int fullFrameWidth = data.Width * (data.CoordinateWidth + data.CoordinatePadding); //the width of a full frame of our multitile in pixels. We get this by multiplying the size of 1 full frame with padding by the width of our tile in tiles.

			if (tile.frameY == 0 && tile.frameX % fullFrameWidth == 0 && PlantUtils.CurrentStage(i, j) != Stage.Grown) //this checks to make sure this is only the top-left tile. We only want one tile to do all the growing for us, and top-left is the standard. otherwise each tile in the multitile ticks on its own due to stupid poopoo redcode. this also checks if the current stage isnt the fully grown stage to stop occurences of tiles disapearing after they grow to the invisible stage after grown
			{
				if (UpdateFlag) //a random check here can slow growing as much as you want.
				{
					PlantUtils.Progress(i, j, data, fullFrameWidth);

					Main.PlaySound(SoundID.Grass, i + (TileSize.X / 2) * 16, j + (TileSize.Y / 2) * 16); //play grass sound at tile pos converted to pixel pos

					//Spawn some grass dust at the tiles position
					Dust dust = Main.dust[Dust.NewDust(new Vector2(i + (TileSize.X / 2) * 16, j + (TileSize.Y / 2) * 16), 16, 16, DustID.Grass)];

					dust.noGravity = false; //make the dust fall
					dust.fadeIn = 0.3f; //make the dust fade in
				}
			}
		}

		public override bool ValidTile(int i, int j)
		{
			if (ValidPlantHook != null)
				return ValidPlantHook(i, j);

			Tile tile = Main.tile[i, j];
			return tile != null && tile.active() && tile.type == Type && tile.frameX == 0 && tile.frameY == 0;
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			TileObjectData data = TileObjectData.GetTileData(type, style);

			if (Main.netMode != NetmodeID.MultiplayerClient)
				return Place(i - data.Origin.X, j - data.Origin.Y);

			NetMessage.SendTileRange(Main.myPlayer, i - data.Origin.X, j - data.Origin.Y, data.Width, data.Height);
			NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i - data.Origin.X, j - data.Origin.Y, Type);

			return -1;
		}
	}

	public class TestPlant : ModTile
	{
		public EEPlantEntity Plant { get; private set; }

		protected TestPlant()
		{
			Plant = new EEPlantEntity(this, new Point16(2, 2), Update);
		}

		public override void SetDefaults() => Plant.SetDefaults();

		public override bool Drop(int i, int j) => Plant.Drop(i, j, ItemID.DirtBlock);

		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Plant.KillMultiTile(i, j);

		public void Update() { }
	}
	public class PlantEntity : ModTileEntity
	{
		/// <summary>
		/// If the strawberry plant has been watered or not on the current day/
		/// </summary>
		public bool HasBeenWatered { get; private set; } = false;

		public Tile StrawberryPlant { get; private set; } = null;

		public int Position.X { get; private set; } = -1;

		public int J { get; private set; } = -1;

		public TileObjectData ObjectData { get; private set; } = null;

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			TileObjectData data = TileObjectData.GetTileData(type, style);

			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				Position.X = i - data.Origin.X;
				J = j - data.Origin.Y;
				StrawberryPlant = Main.tile[Position.X, J];

				return Place(i - data.Origin.X, j - data.Origin.Y);
			}

			NetMessage.SendTileRange(Main.myPlayer, i - data.Origin.X, j - data.Origin.Y, data.Width, data.Height);
			NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i - data.Origin.X, j - data.Origin.Y, Type);

			return -1;
		}

		public override bool ValidTile(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			return tile != null && tile.active() && tile.type == ModContent.TileType<StrawberryPlantTile>() && tile.frameX == 0 && tile.frameY == 0;
		}
	}
}*/