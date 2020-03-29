using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Items.Placeable
{
	class OculumOreItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oculum Ore");
			ItemID.Sets.SortingPriorityMaterials[item.type] = 58;
		}

		public override void SetDefaults()
		{
			item.useStyle = 1;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.maxStack = 999;
			item.consumable = true;
			item.createTile = TileType<Tiles.OculumOre>();
			item.width = 18;
			item.height = 16;
			item.value = 300;
		}
	}
}
