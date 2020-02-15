using Terraria;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Armor.Chartreum
{
	[AutoloadEquip(EquipType.Body)]
	public class ChartreumChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chartreum Chestplate");
		}

		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 26;
			item.defense = 9;
			item.value = 10000;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.rangedDamage += 0.05f;
			player.rangedCrit += 5;
		}
	}
}
