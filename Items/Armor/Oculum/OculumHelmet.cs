using NoxiumMod.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Items.Armor.Oculum
{
	[AutoloadEquip(EquipType.Head)]
	internal class OculumHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("+1 max minions");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.defense = 2;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<OculumChestplate>() && legs.type == ItemType<OculumBoots>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Minions cause the On Fire debuff";
			player.GetModPlayer<NoxiumPlayer>().fireMinion = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.maxMinions += 1;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemType<OculumSlate>(), 15);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}