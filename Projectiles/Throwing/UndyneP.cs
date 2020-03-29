using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace NoxiumMod.Projectiles.Throwing
{
	class UndyneP : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 38;
			projectile.height = 90;
			projectile.friendly = true;
			projectile.thrown = true;
			projectile.scale = 0.92f;
			projectile.tileCollide = false;
			projectile.timeLeft = 255;
			projectile.penetrate = -1;
			projectile.alpha = 0;
		}
		int Timer;

		Vector2 position;
		public override void AI()
		{
			projectile.alpha++;
			Timer++;
			if (Timer <= 40)
			{
				float rotationsPerSecond = 2f;
				projectile.rotation += MathHelper.ToRadians(rotationsPerSecond * 6f);
				position = Main.MouseWorld;
				Lighting.AddLight(projectile.Center, 0.1f, 0.23f, 0.62f);
			}
			else if (Timer <= 70)
			{
				projectile.rotation = projectile.DirectionTo(position).ToRotation() + MathHelper.PiOver2;

			}
			else if (Timer <= 71)
			{
				projectile.velocity = projectile.DirectionTo(position) * 9f;
				int num;
				for (int i = 0; i < 3; i = num + 1)
				{
					int smolWaterWorks = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 253, projectile.velocity.X, projectile.velocity.Y, 100, Color.Blue, 0.45f);
					Main.dust[smolWaterWorks].position = (Main.dust[smolWaterWorks].position + projectile.Center) / 2f;
					Main.dust[smolWaterWorks].noGravity = true;
					Dust sWW = Main.dust[smolWaterWorks];
					sWW.velocity *= 0.85f;
					num = i;
				}

			}
			else if (Timer >= 72)
			{
				if (projectile.alpha <= 195)
				{
					int num;
					for (int i = 0; i < 2; i = num + 1)
					{

						Lighting.AddLight(projectile.Center, 0.3f, 0.56f, 1.21f);
						int waterWorks = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 252, projectile.velocity.X, projectile.velocity.Y, 115, default(Color), 1f);
						switch (i)
						{
							case 0:
								Main.dust[waterWorks].position = (Main.dust[waterWorks].position + projectile.Center * 5f) / 6f;
								break;
							case 1:
								Main.dust[waterWorks].position = (Main.dust[waterWorks].position + (projectile.Center + projectile.velocity / 2f) * 5f) / 6f;
								break;
						}
						Dust wW = Main.dust[waterWorks];
						wW.velocity *= 0.1f;
						Main.dust[waterWorks].noGravity = true;
						Main.dust[waterWorks].fadeIn = 1f;
						Main.dust[waterWorks].scale = 0.74f;
						num = i;
					}
				}

			}
		}
	}
}
