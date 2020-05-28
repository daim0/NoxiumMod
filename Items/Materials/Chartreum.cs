using Terraria;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Materials
{
	internal class Chartreum : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 26;
			item.maxStack = 999;
			item.value = Item.buyPrice(silver: 5);
		}
	}
}