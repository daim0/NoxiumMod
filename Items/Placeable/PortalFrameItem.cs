using NoxiumMod.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Placeable
{
	public class PortalFrameItem : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 20;

			item.maxStack = 99;
			item.value = 750;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<PortalFrameTile>();
			item.placeStyle = 0;
		}
	}
}