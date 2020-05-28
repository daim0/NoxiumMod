using NoxiumMod.Items.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Projectiles
{
	public class OculumsEyeProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oculum's Eye");
			Main.projFrames[projectile.type] = 10;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
			drawOriginOffsetY = 2;
		}

		public sealed override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 50;
			projectile.tileCollide = true;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.minionSlots = 1f;
			projectile.penetrate = -1;
		}

		public override bool? CanCutTiles()
		{
			return true;
		}

		public override bool MinionContactDamage()
		{
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<OculumsVisionBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<OculumsVisionBuff>()))
			{
				projectile.timeLeft = 2;
			}

			if (projectile.velocity.Y == 0 && projectile.oldVelocity.Y == 0)
			{
				projectile.frameCounter++;
				if (projectile.frameCounter % 5 == 0)
				{
					projectile.frame++;
					if (projectile.frame >= Main.projFrames[projectile.type])
					{
						projectile.frame = 0;
					}
				}
			}
			projectile.velocity.Y += 9f / 60f;
		}
	}
}