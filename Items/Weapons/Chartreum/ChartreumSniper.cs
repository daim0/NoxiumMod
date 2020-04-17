
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Weapons.Chartreum
{
	public class ChartreumSniper : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Gun.");
		}

		public override void SetDefaults()
		{
			item.damage = 30;
			item.ranged = true;
			item.width = 96;
			item.height = 30;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 8;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item11;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 16f;
			item.useAmmo = AmmoID.Bullet;
			item.crit = 30;
		}
	}
}
