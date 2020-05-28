using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Tiles.Plasma
{
	internal class PlasmaSandstone : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileValue[Type] = 410;
			Main.tileMerge[Type][TileType<PlasmaSand>()] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Plasma Sandstone");
			AddMapEntry(new Color(172, 153, 185), name);

			dustType = 84;
			drop = ItemType<Items.Placeable.Plasma.PlasmaSandstoneItem>();
			soundType = SoundID.Tink;
			soundStyle = 1;
		}
	}
}