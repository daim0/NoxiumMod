using Terraria;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Armor.Chartreum
{
	[AutoloadEquip(EquipType.Head)]
	public class ChartreumChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ChartreumChestplate");
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
