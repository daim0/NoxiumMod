using Microsoft.Xna.Framework;
using NoxiumMod.Items.Weapons.Throwing;
using Terraria;
using Terraria.ModLoader;

namespace NoxiumMod.Projectiles.Throwing
{
	class KnifeEP : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 28;
			projectile.friendly = true;
			projectile.thrown = true;
			projectile.scale = 0.92f;
			projectile.tileCollide = false;
			projectile.penetrate = 1;
		}

		int Timer;
		int Timer2;
		Vector2 position;
		public override void AI()
		{

			Timer++;
			if (Timer <= 35)
			{
				float rotationsPerSecond = 2f;
				projectile.rotation += MathHelper.ToRadians(rotationsPerSecond * 6f);
				projectile.penetrate = -1;
			}
			else if (Timer >= 36 && Timer2 == 0)
			{
				projectile.velocity = Vector2.Zero;
				projectile.rotation = projectile.DirectionTo(Main.MouseWorld).ToRotation() + MathHelper.PiOver2;
				position = Main.MouseWorld;
				projectile.penetrate = -1;
				projectile.damage = 0;
			}
			if (KnifeExperiment.ActShoot && Timer >= 37)
			{
				Timer2++;
				projectile.penetrate = 1;
				if (Timer2 == 1)
				{
					projectile.velocity = projectile.DirectionTo(position) * 12f;
				}
				else if (Timer2 >= 150)
				{
					projectile.Kill();
				}
			}
		}
	}
}
