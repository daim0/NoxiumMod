using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Projectiles
{
	public class CrystalCannonHitProj : ModProjectile
	{
		public int Timer = 50;
		
		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.penetrate = 1;
			projectile.light = 0.65f;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.extraUpdates = 1;
			this.aiType = 14;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item10, projectile.position);
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 69, 0f, 0f, 0, default(Color), 1f);
			}
		}
		
		public override void AI()
		{	
			this.Timer--;
			if (this.Timer <= 10)
			{
				float num2 = base.projectile.Center.X;
				float num3 = base.projectile.Center.Y;
				float num4 = 250f;
				bool flag = false;
				for (int j = 0; j < 80; j++)
				{
					if (Main.npc[j].CanBeChasedBy(base.projectile, false) && Collision.CanHit(base.projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
					{
						float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
						float num6 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
						float num7 = Math.Abs(base.projectile.position.X + (float)(base.projectile.width / 2) - num5) + Math.Abs(base.projectile.position.Y + (float)(base.projectile.height / 2) - num6);
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
					float num8 = 24f;
					Vector2 vector = new Vector2(base.projectile.position.X + (float)base.projectile.width * 0.5f, base.projectile.position.Y + (float)base.projectile.height * 0.5f);
					float num9 = num2 - vector.X;
					float num10 = num3 - vector.Y;
					float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
					num11 = num8 / num11;
					num9 *= num11;
					num10 *= num11;
					base.projectile.velocity.X = (base.projectile.velocity.X * 20f + num9) / 21f;
					base.projectile.velocity.Y = (base.projectile.velocity.Y * 20f + num10) / 21f;
				}
			}
		}
	}
}
