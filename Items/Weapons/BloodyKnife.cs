using NoxiumMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Items.Weapons
{
	public class BloodyKnife : ModItem
	{
		public override void SetDefaults()
		{
			item.shootSpeed = 10f;
			item.damage = 10;
			item.knockBack = 5f;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useAnimation = 17;
			item.useTime = 17;
			item.width = 30;
			item.height = 30;
			item.rare = ItemRarityID.Pink;
			item.consumable = false;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.thrown = true;
			item.UseSound = SoundID.Item1;
			item.value = Item.sellPrice(silver: 5);
			item.shoot = ProjectileType<BloodyKnifeProjectile>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrimtaneBar, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();

		}
	}
}