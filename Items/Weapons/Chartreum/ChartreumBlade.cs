using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace NoxiumMod.Items.Weapons.Chartreum
{
	class ChartreumBlade : ModItem
	{

		public override void SetDefaults()
		{
			item.damage = 40;
			item.melee = true;
			item.width = 54;
			item.height = 58;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = 1;
			item.knockBack = 5;
			item.value = Item.buyPrice(gold: 1);
			item.rare = 4;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("ChartreumBladeMainProj");
			item.shootSpeed = 10f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float sideProjectiles = 2;
			float rotation = MathHelper.ToRadians(35f);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 35f;
			for (int i = 0; i < sideProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (sideProjectiles - 1))) * .2f;
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X * 2, perturbedSpeed.Y * 2, mod.ProjectileType("ChartreumBladeSideProj"), damage, knockBack, player.whoAmI);
			}

			Projectile.NewProjectile(position.X, position.Y, speedX / 2, speedY / 2, type, damage, knockBack, player.whoAmI);
			return false;
		}
	}
}
