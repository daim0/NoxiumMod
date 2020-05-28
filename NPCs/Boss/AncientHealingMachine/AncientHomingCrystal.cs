using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.NPCs.Boss.AncientHealingMachine
{
	public class AncientHomingCrystal : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Homing Crystal");
		}

		public override void SetDefaults()
		{
			projectile.width = 26;
			projectile.height = 56;
			projectile.hostile = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 110;
		}

		public override void AI()
		{
			projectile.ai[1] += 1f;
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			float num = 70f;
			float scaleFactor = 15f;
			float num2 = 50f;
			if (projectile.alpha > 0)
			{
				projectile.alpha -= 10;
			}
			if (projectile.alpha < 0)
			{
				projectile.alpha = 0;
			}

			int num3 = (int)projectile.ai[0];
			if (num3 >= 0 && Main.player[num3].active && !Main.player[num3].dead)
			{
				if (projectile.Distance(Main.player[num3].Center) > num2)
				{
					Vector2 vector = projectile.DirectionTo(Main.player[num3].Center);
					if (vector.HasNaNs())
					{
						vector = Vector2.UnitY;
					}
					projectile.velocity = (projectile.velocity * (num - 1f) + vector * scaleFactor) / num;
					return;
				}
			}
			else
			{
				if (projectile.timeLeft > 30)
				{
					projectile.timeLeft = 30;
				}
				if (projectile.ai[0] != -1f)
				{
					projectile.ai[0] = -1f;
					projectile.netUpdate = true;
					return;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
			Main.PlaySound(SoundID.Item38, projectile.position);
			for (int i = 0; i < 8; i++)
			{
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 61, projectile.oldVelocity.X * 1.1f, projectile.oldVelocity.Y * 1.1f, 0, default, 1f);
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 60, projectile.oldVelocity.X * 1.1f, projectile.oldVelocity.Y * 1.1f, 0, default, 1f);
			}
		}
	}
}