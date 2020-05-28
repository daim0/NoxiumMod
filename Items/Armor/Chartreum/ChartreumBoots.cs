using Terraria;
using Terraria.ModLoader;

//This is a note left by goodpro712 - daimgamer has ensalved me send help
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

		public override void UpdateArmorSet(Player player)
		{
			player.rangedDamage += 0.05f;
			player.moveSpeed += 0.05f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Materials.Chartreum>(), 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}