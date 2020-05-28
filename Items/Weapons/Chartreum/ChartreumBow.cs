using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace NoxiumMod.Items.Weapons.Chartreum
{
	public class ChartreumBow : ModItem
	{
		public override void SetDefaults()
		{
			item.damage = 25;
			item.ranged = true;
			item.width = 50;
			item.height = 82;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 5;
			item.value = 13;
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.shoot = ProjectileID.PurificationPowder;
			item.shootSpeed = 16f;
			item.useAmmo = AmmoID.Arrow;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(4, 0);
		}

		public override float UseTimeMultiplier(Player player)
		{
			for (int i = 15; i < 20; i++)
			{

			}
			return base.UseTimeMultiplier(player);
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.Chartreum>(), 8);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
    }
}
