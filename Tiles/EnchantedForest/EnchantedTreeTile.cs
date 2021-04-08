using Microsoft.Xna.Framework.Graphics;
using NoxiumMod.Gores;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Tiles.EnchantedForest
{
	public class EnchantedTreeTile : ModTree
	{
		// TODO: do this
		public override int CreateDust() => 0;

		public override int GrowthFXGore() => ModContent.GetInstance<NoxiumMod>().GetGoreSlot<EnchantedTreeFX>();

		// TODO: do this?
		public override int DropWood() => ItemID.Wood;

		public override Texture2D GetTexture() => ModContent.GetInstance<NoxiumMod>().GetTexture("Tiles/EnchantedForest/EnchantedTreeTile");

		public override Texture2D GetTopTextures(int i, int j, ref int frame, ref int frameWidth, ref int frameHeight, ref int xOffsetLeft, ref int yOffset)
		{
			frameWidth = 118;
			frameHeight = 96;
			xOffsetLeft = 52;

			return ModContent.GetInstance<NoxiumMod>().GetTexture("Tiles/EnchantedForest/EnchantedTreeTileTops");
		}

		public override Texture2D GetBranchTextures(int i, int j, int trunkOffset, ref int frame)
			=> ModContent.GetInstance<NoxiumMod>().GetTexture("Tiles/EnchantedForest/EnchantedTreeTileBranches");
	}
}
