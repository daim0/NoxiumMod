using Terraria;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Armor.Chartreum
{
	[AutoloadEquip(EquipType.Head)]
	public class ChartreumHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ChartreumHelmet");
		}

		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 26;
			item.defense = 8;
			item.value = 20000;
		}
			

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("ChartreumChestplate") && legs.type == mod.ItemType("ChartreumBoots");
		}
	}
}

