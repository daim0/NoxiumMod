using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Tiles.EnchantedForest
{
	public class EnchantedGrassTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileMergeDirt[Type] = true;

			TileID.Sets.Grass[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;

			SetModTree(new EnchantedTreeTile());

			AddMapEntry(new Color(50, 170, 250));

			drop = ItemID.DirtBlock;
			soundType = SoundID.Dig;

			// TODO: maybe a sparkle dust or something?
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (fail && Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].hammer == 0)
				Main.tile[i, j].type = TileID.Dirt;
		}
	}
}
