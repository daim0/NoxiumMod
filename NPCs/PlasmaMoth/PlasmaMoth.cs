using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TestingTools.NPCs.PlasmaMoth
{
	public class PlasmaMoth : ModNPC
	{
		public override void SetStaticDefaults()
		{
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 144;
			npc.height = 174;
			npc.lifeMax = 6000;
			npc.damage = 40;
			npc.defense = 14;
			npc.aiStyle = -1;
			npc.knockBackResist = 0f;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit32;
		}

		private float Timer
		{
			get => npc.ai[0];
			set => npc.ai[0] = value;
		}

		private float CurrentAttack
		{
			get => npc.ai[1];
			set => npc.ai[1] = value;
		}

		private void Idle(Player target)
		{
			if (npc.ai[3] == 0)
				npc.ai[3] = 0.02f;
			Vector2 offset = target.Center + new Vector2((float)Math.Cos(npc.ai[2]) * 750, 50 + (float)Math.Sin(npc.ai[2]) * 375);
			Vector2 dest = offset - npc.Center;
			float sqrt = dest.Length();

			float dist = npc.Distance(offset);
			float maxSpeed = 5f + dist / 50;

			dest = offset + target.velocity * (dist / maxSpeed) - npc.Center;

			npc.velocity += dest * (maxSpeed / 10 / sqrt);

			npc.ai[2] += npc.ai[3];
			if (npc.ai[2] < 0.5 - Math.PI)
			{
				npc.ai[2] = (float)(0.5 - Math.PI);
				npc.ai[3] *= -1;
			}
			else if (npc.ai[2] > -0.5)
			{
				npc.ai[2] = -0.5f;
				npc.ai[3] *= -1;
			}

			sqrt = npc.velocity.Length();
			if (sqrt > maxSpeed)
			{
				npc.velocity /= sqrt;
				npc.velocity *= maxSpeed;
			}
			if (Timer >= 120 && Timer <= 300)
			{
				if (Timer % 20 == 0)
				{
					Vector2 pos = new Vector2(-80, -60);
					if (Timer % 40 == 20)
					{
						pos.X = 74;
					}
					Projectile.NewProjectile(npc.Center + pos, new Vector2(0, 10), ModContent.ProjectileType<Projectiles.PlasmaLaser>(), 20, 0f, Main.myPlayer);
				}
				npc.localAI[2] = 0;
			}
			if (++Timer >= 360)
			{
				CurrentAttack = 1;
				npc.velocity = Vector2.Zero;
				Timer = 0;
			}
		}

		private void Charge(Player target)
		{
			Vector2 dest = target.Center - npc.Center;
			if (Timer == 0)
			{
				Main.PlaySound(SoundID.Item105, npc.position);
			}
			if (Timer <= 36)
			{
				if (Timer < 25)
				{
					npc.ai[2] = (float)Math.Atan2(dest.Y, dest.X);
				}
				npc.velocity += new Vector2((float)Math.Cos(npc.ai[2]), (float)Math.Sin(npc.ai[2])) * (Timer * 0.2f - 2f);
			}
			if (Timer == 30)
			{
				Main.PlaySound(SoundID.Item109, npc.position).Pitch = -0.6f;
			}
			npc.localAI[2]++;
			if (++Timer >= 50)
			{
				CurrentAttack = 0;
				npc.ai[3] = 0.02f;
				Timer = 0;
			}
		}

		public override void AI()
		{
			npc.localAI[2]++;
			if (CurrentAttack >= 0)
			{
				if (!npc.HasValidTarget)
				{
					npc.TargetClosest(true);
				}
				if (!npc.HasValidTarget)
				{
					CurrentAttack = -1;
				}
			}
			if (CurrentAttack < 0)
			{
				npc.velocity.Y -= 1f;
				if (npc.timeLeft > 10)
				{
					npc.timeLeft = 10;
				}
				return;
			}
			Player target = Main.player[npc.target];
			switch (CurrentAttack)
			{
				case 0f:
					Idle(target);
					break;

				case 1f:
					Charge(target);
					break;
			}
		}

		internal static void DrawWingTrail(SpriteBatch spriteBatch, Vector2 pos, int index, float scale, bool flip)
		{
			Color color;
			if (index >= 6)
			{
				color = new Color(16, 135, 135) * 0.25f;
			}
			else if (index >= 3)
			{
				color = new Color(115, 189, 158) * 0.5f;
			}
			else
			{
				color = new Color(184, 252, 255) * 0.75f;
			}
			spriteBatch.Draw(ModContent.GetTexture("TestingTools/NPCs/PlasmaMoth/Trail"), pos, null, color, 0f, new Vector2(flip ? 120 : 118, 79), new Vector2(scale, 1.5f - scale / 2), flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 offset = Main.screenPosition - new Vector2(0, npc.gfxOffY);
			bool right = npc.direction > 0;
			SpriteEffects flip = right ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Vector2 wingOffset = offset - new Vector2(right ? -6 : 6, -12);
			for (int i = npc.oldPos.Length - 1; i >= 0; i--)
			{
				DrawWingTrail(spriteBatch, npc.oldPos[i] + npc.Size / 2 - wingOffset, i, 0.5f + (float)Math.Cos((npc.localAI[2] - i) / 4) / 2, right);
			}
			
			float time = 0.5f + (float)Math.Cos(npc.localAI[2] / 4) / 2;

			spriteBatch.Draw(ModContent.GetTexture("TestingTools/NPCs/PlasmaMoth/Wings"), npc.Center - wingOffset, null, Color.White, 0f, new Vector2(118, 79), new Vector2(time, 1.5f - time / 2), flip, 0f);
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - offset, null, Color.White, 0f, new Vector2(144, 174) / 2, 1f, flip, 0f);

			return false;
		}
	}
}
