using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Weapons.Wood
{
	class PureWoodBow : ModItem
	{
		public override void SetDefaults()
		{
			item.damage = 3;
			item.ranged = true;
			item.width = 16;
			item.height = 36;
			item.useTime = 27;
			item.useAnimation = 27;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 1;
			item.value = 5;
			item.rare = 1;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 6f;
			item.useAmmo = AmmoID.Arrow;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(1, 1);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 12);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
