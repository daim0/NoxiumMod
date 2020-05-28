using NoxiumMod.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Items.Armor.Oculum
{
	[AutoloadEquip(EquipType.Body)]
	internal class OculumChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("+10% minion damage");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 16;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.defense = 5;
		}

		public override void UpdateEquip(Player player)
		{
			player.minionDamage += 0.1f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemType<OculumSlate>(), 25);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}