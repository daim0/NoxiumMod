using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace NoxiumMod.Projectiles
{
	class GelKnifeProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = 2;
			projectile.timeLeft = 300;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.ai[1] == 2)
			{
				return true;
			}

			/*if (projectile.soundDelay == 0)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/ballbounce").WithVolume(.7f));
            }
            projectile.soundDelay = 10;*/


			if (projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f)
			{
				projectile.velocity.X = oldVelocity.X * -0.9f;
			}
			if (projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f)
			{
				projectile.velocity.Y = oldVelocity.Y * -0.9f;
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{

		}
		public override void AI()

		{

			if (projectile.ai[0] == 0)

			{

				//Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/throwball").WithVolume(.7f));

				projectile.ai[0] = 1;

				projectile.ai[1] = 1;


				projectile.netUpdate = true;

			}


			if (projectile.owner == Main.myPlayer && projectile.timeLeft <= 3)

			{



				projectile.tileCollide = false;


				projectile.alpha = 255;


				projectile.position.X = projectile.position.X + (float)(projectile.width / 2);

				projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);

				projectile.width = 250;

				projectile.height = 250;

				projectile.position.X = projectile.position.X - (float)(projectile.width / 2);

				projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

			}

			else

			{


			}

			projectile.ai[0] += 1f;

			if (projectile.ai[0] > 5f)

			{

				projectile.ai[0] = 10f;


				if (projectile.velocity.Y == 0f && projectile.velocity.X != 0f)

				{

					projectile.velocity.X = projectile.velocity.X * 0.97f;


					{

						projectile.velocity.X = projectile.velocity.X * 0.99f;

					}

					if ((double)projectile.velocity.X > -0.01 && (double)projectile.velocity.X < 0.01)

					{

						projectile.velocity.X = 0f;

						projectile.netUpdate = true;

					}

				}

				projectile.velocity.Y = projectile.velocity.Y + 0.2f;

			}


			projectile.rotation += projectile.velocity.X * 0.06f;

			return;

		}
	}
}

