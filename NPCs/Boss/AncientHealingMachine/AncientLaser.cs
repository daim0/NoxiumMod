using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.NPCs.Boss.AncientHealingMachine
{
	public class AncientLaser : ModProjectile
	{
		private int Timer = 0;

		private int desiredFrame = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Laser");
		}

		public override void SetDefaults()
		{
			projectile.width = 28;
			projectile.height = 30;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			this.cooldownSlot = 1;
		}

		public override void AI()
		{
			Timer++;
			if (Timer >= 8)
			{
				Timer = 0;
				desiredFrame++;
			}
			if (desiredFrame >= 3)
			{
				desiredFrame = 0;
			}

			Vector2? vector = null;
			if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
			{
				projectile.velocity = -Vector2.UnitY;
			}

			if (Main.npc[(int)projectile.ai[1]].active && Main.npc[(int)projectile.ai[1]].type == mod.NPCType("AncientHealingMachine")) //This thing makes it... spawn on the right NPC and in the right position on the NPC?
			{
				Vector2 bsSpawning = new Vector2(-75f, 32f);
				Vector2 bsSpawningBoo = new Vector2(Main.npc[(int)projectile.ai[1]].Center.X - 75f, Main.npc[(int)projectile.ai[1]].Center.Y + 32f);
				Vector2 bsSpawningSucks = Utils.Vector2FromElipse(Main.npc[(int)projectile.ai[1]].localAI[0].ToRotationVector2(), bsSpawning * Main.npc[(int)projectile.ai[1]].localAI[1]);
				projectile.position = bsSpawningBoo + bsSpawningSucks - new Vector2((float)projectile.width, (float)projectile.height) / 2f;
			}

			if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
			{
				projectile.velocity = -Vector2.UnitY;
			}
			if (projectile.localAI[0] == 0f)
			{
				Main.PlaySound(SoundID.Zombie, (int)projectile.position.X, (int)projectile.position.Y, 88, 1f, 0f);
			}
			float num = 1f;
			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] >= 180f)
			{
				projectile.Kill();
				return;
			}
			projectile.scale = (float)Math.Sin((double)(projectile.localAI[0] * 3.141f / 180f)) * 10f * num;
			if (projectile.scale > num)
			{
				projectile.scale = num;
			}
			float num2 = projectile.velocity.ToRotation();
			num2 += projectile.ai[0];
			projectile.rotation = num2 - 1.57f;
			projectile.velocity = num2.ToRotationVector2();
			float num3 = 3f;
			float num4 = (float)projectile.width;
			Vector2 pointWhereItDoesDaDing = projectile.Center;
			if (vector != null)
			{
				pointWhereItDoesDaDing = vector.Value;
			}
			float[] array = new float[(int)num3];
			Collision.LaserScan(pointWhereItDoesDaDing, projectile.velocity, num4 * projectile.scale, 2400f, array);
			float num5 = 0f;
			int num6;
			for (int i = 0; i < array.Length; i = num6 + 1)
			{
				num5 += array[i];
				num6 = i;
			}
			num5 /= num3;
			float ppMeter = 0.5f;
			projectile.localAI[1] = MathHelper.Lerp(projectile.localAI[1], num5, ppMeter);
			if (Main.rand.Next(3) == 0)
			{
				Vector2 value43 = projectile.velocity.RotatedBy(1.5707963705062866) * ((float)Main.rand.NextDouble() - 0.5f) * projectile.width;
				int num813 = Dust.NewDust(value43 - Vector2.One * 4f, 8, 8, 219, 0f, 0f, 100, default, 1.5f);
				Dust dust3 = Main.dust[num813];
				dust3.velocity *= 0.5f;
				Main.dust[num813].velocity.Y = 0f - Math.Abs(Main.dust[num813].velocity.Y);
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
			Texture2D texture2D = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaser");
			Texture2D texture = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserBody");
			Texture2D texture2 = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserAss");

			if (desiredFrame == 0) // weirdchamp "animation"
			{
				texture = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserBody");
				texture2 = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserAss");
			}
			if (desiredFrame == 1)
			{
				texture = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserBody2");
				texture2 = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserAss2");
			}
			if (desiredFrame == 2)
			{
				texture = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserBody3");
				texture2 = mod.GetTexture("NPCs/Boss/AncientHealingMachine/AncientLaserAss3");
			}

			Color color = new Color(210, 255, 230, 0);
			float num = projectile.localAI[1];
			SpriteBatch spriteBatch2 = Main.spriteBatch;
			Texture2D anotherSpriteBatch = texture2D;
			Vector2 position = projectile.Center - Main.screenPosition;
			spriteBatch2.Draw(anotherSpriteBatch, position, null, color, projectile.rotation, texture2D.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
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
			if (projHitbox.Intersects(targetHitbox)) //daim should be the shut.
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