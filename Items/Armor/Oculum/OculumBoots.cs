using NoxiumMod.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Items.Armor.Oculum
{
	[AutoloadEquip(EquipType.Legs)]
	internal class OculumBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("+20 max mana");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 12;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.defense = 4;
		}

		public override void UpdateEquip(Player player)
		{
			player.statManaMax2 += 20;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemType<OculumSlate>(), 20);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}