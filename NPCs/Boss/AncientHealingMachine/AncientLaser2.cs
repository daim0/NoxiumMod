using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ID;

namespace NoxiumMod.NPCs.Boss.AncientHealingMachine
{
	public class AncientLaser2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Laser");
		}

		public override void SetDefaults()
		{
			projectile.width = 28;
			projectile.height = 30;
			projectile.hostile = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft = 410;
			this.cooldownSlot = 1;
		}
		
		private int Timer = 0;
		private int desiredFrame = 0;

		public override void AI()
		{
			Timer++;
			if(Timer >= 8)
			{
				Timer = 0;
				desiredFrame++;
			}
			if(desiredFrame >= 3)
			{
				desiredFrame = 0;
			}
			
			Vector2? vector = null;
			if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
			{
				projectile.velocity = -Vector2.UnitY;
			}
			if (Main.npc[(int)projectile.ai[1]].active && Main.npc[(int)projectile.ai[1]].type == mod.NPCType("AncientHealingMachine"))
			{
				Vector2 value = new Vector2(-75f, 32f);
				Vector2 value2 = new Vector2(Main.npc[(int)projectile.ai[1]].Center.X + 75f, Main.npc[(int)projectile.ai[1]].Center.Y + 32f);
				Vector2 value3 = Utils.Vector2FromElipse(Main.npc[(int)projectile.ai[1]].localAI[0].ToRotationVector2(), value * Main.npc[(int)projectile.ai[1]].localAI[1]);
				projectile.position = value2 + value3 - new Vector2((float)projectile.width, (float)projectile.height) / 2f;
			}
			if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
			{
				projectile.velocity = -Vector2.UnitY;
			}
			if (projectile.localAI[0] == 0f)
			{
				Main.PlaySound(SoundID.Zombie, (int)projectile.position.X, (int)projectile.position.Y, 104, 1f, 0f);
			}
			float num = 1f;
			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] >= 180f)
			{
				projectile.Kill();
				return;
			}
			projectile.scale = (float)Math.Sin((double)(projectile.localAI[0] * 3.14159274f / 180f)) * 10f * num;
			if (projectile.scale > num)
			{
				projectile.scale = num;
			}
			float num2 = projectile.velocity.ToRotation();
			num2 += projectile.ai[0];
			projectile.rotation = num2 - 1.57079637f;
			projectile.velocity = num2.ToRotationVector2();
			float num3 = 3f;
			float num4 = (float)projectile.width;
			Vector2 samplingPoint = projectile.Center;
			if (vector != null)
			{
				samplingPoint = vector.Value;
			}
			float[] array = new float[(int)num3];
			Collision.LaserScan(samplingPoint, projectile.velocity, num4 * projectile.scale, 2400f, array);
			float num5 = 0f;
			int num6;
			for (int i = 0; i < array.Length; i = num6 + 1)
			{
				num5 += array[i];
				num6 = i;
			}
			num5 /= num3;
			float amount = 0.5f;
			projectile.localAI[1] = MathHelper.Lerp(projectile.localAI[1], num5, amount);
			if (Main.rand.Next(4) == 0)
			{
				float num7 = projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.57079637f;
				float num8 = (float)Main.rand.NextDouble() * 2f + 2f;	
				int num9 = Dust.NewDust(projectile.Center, 0, 0, 219, projectile.velocity.X, projectile.velocity.Y, 0, default(Color), 1f);
				Main.dust[num9].noGravity = true;
				Main.dust[num9].scale = 1.15f;
			}
			
			DelegateMethods.v3_1 = new Vector3(0.3f, 0.65f, 0.7f);
			Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], (float)projectile.width * projectile.scale, new Utils.PerLinePoint(DelegateMethods.CastLight));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (projectile.velocity == Vector2.Zero)
			{
				return false;
			}
			Texture2D texture2D = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserAss");
			Texture2D texture = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserBody");
			Texture2D texture2 = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserAss");
			
			if(desiredFrame == 0)
			{
				texture = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserBody");
				texture2 = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserAss");
				texture2D = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserAss");
			}
			if(desiredFrame == 1)
			{
				texture = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserBody2");
				texture2 = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserAss2");
				texture2D = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserAss2");
			}
			if(desiredFrame == 2)
			{
				texture = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserBody3");
				texture2 = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserAss3");
				texture2D = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserAss3");
			}
			
			
			float num = projectile.localAI[1];
			Color color = new Color(255, 255, 255, 0) * 0.9f;
			SpriteBatch spriteBatch2 = Main.spriteBatch;
			Texture2D texture3 = texture2D;
			Vector2 position = projectile.Center - Main.screenPosition;
			spriteBatch2.Draw(texture3, position, null, color, projectile.rotation, texture2D.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
			num -= (float)(texture2D.Height / 2 + texture2.Height) * projectile.scale;
			Vector2 value = projectile.Center;
			value += projectile.velocity * projectile.scale * (float)texture2D.Height / 2f;
			if (num > 0f)
			{
				float num2 = 0f;
				Rectangle value2 = new Rectangle(0, 16 * (projectile.timeLeft / 3 % 5), texture.Width, 16);
				while (num2 + 1f < num)
				{
					if (num - num2 < (float)value2.Height)
					{
						value2.Height = (int)(num - num2);
					}
					Main.spriteBatch.Draw(texture, value - Main.screenPosition, new Rectangle?(value2), color, projectile.rotation, new Vector2((float)(value2.Width / 2), 0f), projectile.scale, SpriteEffects.None, 0f);
					num2 += (float)value2.Height * projectile.scale;
					value += projectile.velocity * (float)value2.Height * projectile.scale;
					value2.Y += 16;
					if (value2.Y + value2.Height > texture.Height)
					{
						value2.Y = 0;
					}
				}
			}
			SpriteBatch spriteBatch3 = Main.spriteBatch;
			Texture2D texture4 = texture2;
			Vector2 position2 = value - Main.screenPosition;
			spriteBatch3.Draw(texture4, position2, null, color, projectile.rotation, texture2.Frame(1, 1, 0, 0).Top(), projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 velocity = projectile.velocity;
			Utils.PlotTileLine(projectile.Center, projectile.Center + velocity * projectile.localAI[1], (float)projectile.width * projectile.scale, new Utils.PerLinePoint(DelegateMethods.CutTiles));
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (projHitbox.Intersects(targetHitbox))
			{
				return new bool?(true);
			}
			float num = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], 22f * projectile.scale, ref num))
			{
				return new bool?(true);
			}
			return new bool?(false);
		}
	}
}
