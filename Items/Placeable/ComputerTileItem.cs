using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Items.Placeable
{
	public class ComputerTileItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Computer");
			Tooltip.SetDefault("Used to program turtles");
		}

		public override void SetDefaults()
		{
			item.useStyle = 1;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.maxStack = 99;
			item.consumable = true;
			item.createTile = TileType<Tiles.ComputerTile>();
			item.width = 24;
			item.height = 16;
			item.value = 800;
		}
	}
}
