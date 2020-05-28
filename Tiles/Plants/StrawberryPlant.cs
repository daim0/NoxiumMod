using NoxiumMod.Systems.Plants;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace NoxiumMod.Tiles.Plants
{
	public class StrawberryPlantEntity : PlantEntity<StrawberryPlant>
	{

	}

	public class StrawberryPlant : Plant<StrawberryPlantEntity>
	{
		public override int[] CoordinateHeights => new int[] { 16, 16 };

		public override Point16 TileSize => new Point16(2, 2);

		public override TileObjectData CopyData => TileObjectData.Style2x2;
	}
}