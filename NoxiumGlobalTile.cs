using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod
{
	public class NoxiumGlobalTile : GlobalTile
	{
		public override bool Drop(int i, int j, int type)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient && !WorldGen.noTileActions && !WorldGen.gen)
			{
				if (type == TileID.Trees && Main.tile[i, j + 1].type == TileID.Grass) // Checking if the tree is planted on grass (Forest)
				{
					if (Main.rand.Next(6) == 0) // 1 in 6 chance
						Item.NewItem(i * 16, (j - 5) * 16, 32, 32, mod.ItemType("Apple")); // Drop your apple here
				}
			}

			return base.Drop(i, j, type);
		}
	}
}