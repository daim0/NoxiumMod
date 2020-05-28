using Microsoft.Xna.Framework;
using NoxiumMod.Items.Weapons.Plasma;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Projectiles.PlasmaStuff
{
	internal class BeetleEyePro : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 10f;
			ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 300f;
			ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 13f;
		}

		public override void SetDefaults()
		{
			projectile.extraUpdates = 0;
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = 99;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.scale = 1f;
		}

		public override void AI()
		{
			if (BeetleYoyo.ShootProjectiles == true)
			{
				for (int d = 0; d < 40; d++)
				{
					Dust.NewDust(projectile.position, projectile.width, projectile.height, 8, 0f, 0f, 150, Color.Aquamarine, 1.5f);
				}
			}
		}
	}
}