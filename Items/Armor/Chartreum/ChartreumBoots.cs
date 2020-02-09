using Terraria;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Armor.Chartreum
{
	[AutoloadEquip(EquipType.Legs)]
	public class ChartreumBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ChartreumBoots");
		}

		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 26;
			item.defense = 8;
			item.value = 10000;
		}
	}
}
