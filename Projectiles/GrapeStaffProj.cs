using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace NoxiumMod.Projectiles
{
	public class GrapeStaffProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grape Ball");
		}

		public override void SetDefaults()
		{
			projectile.arrow = true;
			projectile.width = 15;
			projectile.height = 15;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 180;
		}

		public override void AI()
		{
			projectile.velocity.Y = projectile.velocity.Y + 0.2f;
			if (projectile.velocity.Y > 16f)
			{
				projectile.velocity.Y = 32f;
			}
			projectile.rotation += 0.4f * (float)projectile.direction;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.ai[1] != 0)
			{
				return true;
			}
			if (projectile.velocity.Y == 0)
			{
				projectile.velocity.X *= 0.97f;

			}
			if (projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f)
			{
				projectile.velocity.X = oldVelocity.X * -0.8f;
			}
			if (projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f)
			{
				projectile.velocity.Y = oldVelocity.Y * -0.8f;
			}
			return false;
		}
		public override void Kill(int timeLeft)
		{
			if (projectile.ai[1] == 0)
			{
				for (int i = 0; i < 3; i++)
				{
					Vector2 velocity = new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-10, -8));
					Projectile.NewProjectile(projectile.position, velocity, mod.ProjectileType("GrapeShrapnel"), 13, 5, projectile.owner);
				}
			}
		}
	}
}
