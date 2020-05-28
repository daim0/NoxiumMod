using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace NoxiumMod.Systems.Plants
{
	public abstract class PlantEntity : ModTileEntity
	{
		protected TileObjectData ObjectData { get; private set; }

		public bool hasBeenWatered;

		public override bool ValidTile(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			return tile != null && tile.active() && tile.type == GetPlant().Type && tile.frameX == 0 && tile.frameY == 0;
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			TileObjectData data = TileObjectData.GetTileData(type, style);

			if (Main.netMode != NetmodeID.MultiplayerClient)
				return Place(i - data.Origin.X, j - data.Origin.Y);

			NetMessage.SendTileRange(Main.myPlayer, i - data.Origin.X, j - data.Origin.Y, data.Width, data.Height);
			NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i - data.Origin.X, j - data.Origin.Y, Type);

			ObjectData = TileObjectData.GetTileData(Main.tile[Position.X, Position.Y]);

			return -1;
		}

		public override void OnNetPlace()
		{
			ObjectData = TileObjectData.GetTileData(Main.tile[Position.X, Position.Y]);
		}

		public override void Update()
		{
			if (Main.dayTime && Main.time == 1)
				OnDayBeginning();
		}

		public virtual void OnDayBeginning()
		{
			if (!hasBeenWatered)
				PlantHelper.Kill(Position.X, Position.Y, ObjectData);
			else
			{
				Tile tile = Framing.GetTileSafely(Position); //you could probably add more safety checks if you want to be extra giga secure, but we assume RandomUpdate only calls valid tiles here
				int fullFrameWidth = ObjectData.Width * (ObjectData.CoordinateWidth + ObjectData.CoordinatePadding); //the width of a full frame of our multitile in pixels. We get this by multiplying the size of 1 full frame with padding by the width of our tile in tiles.

				if (tile.frameY == 0 && tile.frameX % fullFrameWidth == 0 && PlantHelper.CurrentStage(Position.X, Position.Y) != Stage.Grown)
					PlantHelper.ProgressWithEffects(Position, ObjectData);
			}

			hasBeenWatered = false;
		}

		public override TagCompound Save()
		{
			return new TagCompound
			{
				[nameof(hasBeenWatered)] = hasBeenWatered
			};
		}

		public override void Load(TagCompound tag)
		{
			hasBeenWatered = tag.GetBool(nameof(hasBeenWatered));
		}

		public override void NetSend(BinaryWriter writer, bool lightSend)
		{
			writer.Write(hasBeenWatered);
		}

		public override void NetReceive(BinaryReader reader, bool lightReceive)
		{
			hasBeenWatered = reader.ReadBoolean();
		}

		public abstract Plant GetPlant();
	}

	public abstract class PlantEntity<T> : PlantEntity where T : Plant
	{
		public override Plant GetPlant()
		{
			return ModContent.GetInstance<T>();
		}
	}
}