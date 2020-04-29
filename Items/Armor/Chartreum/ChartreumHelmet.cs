using Terraria;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Armor.Chartreum
{
	[AutoloadEquip(EquipType.Head)]
	public class ChartreumHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chartreum Helmet");
		}

		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 26;
			item.defense = 5;
			item.value = 10000;// will add later 
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "10% More speed and ranged damage";
			player.rangedDamage += 0.1f;
			{
				if (!(player.HeldItem.modItem?.item?.type == mod.ItemType("ChartreumSniper")))
					return;
			}
		}


		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("ChartreumChestplate") && legs.type == mod.ItemType("ChartreumBoots");
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

