using NoxiumMod.Projectiles.Wooden;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Items.Weapons.Wood
{
	class PureWoodSpear : ModItem
	{

		public override void SetDefaults()
		{
			item.damage = 7;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 31;
			item.useTime = 31;
			item.shootSpeed = 3.3f;
			item.knockBack = 5f;
			item.width = 48;
			item.height = 48;
			item.scale = 1f;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(silver: 1);

			item.melee = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.autoReuse = false;

			item.UseSound = SoundID.Item1;
			item.shoot = ProjectileType<PureWoodSpearP>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 8);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
