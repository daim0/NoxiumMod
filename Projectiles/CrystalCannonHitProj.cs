using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Projectiles
{
	public class CrystalCannonHitProj : ModProjectile
	{
		public int Timer = 50;

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.penetrate = 1;
			projectile.light = 0.65f;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.extraUpdates = 1;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			return true;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item10, projectile.position);
		}

		public override void AI()
		{
			if (!projectile.tileCollide && Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
			{
				projectile.tileCollide = true;
				projectile.Kill();
			}
			this.Timer--;
			if (this.Timer <= 10)
			{
				projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

				float num2 = projectile.Center.X;
				float num3 = projectile.Center.Y;
				float num4 = 350f;
				bool flag = false;
				for (int j = 0; j < 160; j++)
				{
					if (Main.npc[j].CanBeChasedBy(projectile, false) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
					{
						float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
						float num6 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
						float num7 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num5) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num6);
						if (num7 < num4)
						{
							num4 = num7;
							num2 = num5;
							num3 = num6;
							flag = true;
						}
					}
				}
				if (flag)
				{
					float num8 = 26f;
					Vector2 vector = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num9 = num2 - vector.X;
					float num10 = num3 - vector.Y;
					float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
					num11 = num8 / num11;
					num9 *= num11;
					num10 *= num11;
					projectile.velocity.X = (projectile.velocity.X * 20f + num9) / 21f;
					projectile.velocity.Y = (projectile.velocity.Y * 20f + num10) / 21f;
				}
				else
				{
					projectile.Kill();
				}
				int greenDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 61, 0f, 0f, 0, default, 1f);
				Main.dust[greenDust].noGravity = true;
				Main.dust[greenDust].scale = 0.8f;
			}
			else
			{
				projectile.rotation += 0.2f * (float)projectile.direction;
				projectile.ai[0] += 1f;
				projectile.localAI[0] += 1f;
			}
		}
	}
}