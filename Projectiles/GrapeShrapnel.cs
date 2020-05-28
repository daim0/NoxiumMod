using Terraria.ModLoader;

namespace NoxiumMod.Projectiles
{
	public class GrapeShrapnel : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mini Grape");
		}

		public override void SetDefaults()
		{
			projectile.arrow = false;
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.ranged = true;
		}

		public override void AI()
		{
			projectile.velocity.Y = projectile.velocity.Y + 0.2f;
			if (projectile.velocity.Y > 16f)
			{
				projectile.velocity.Y = 32f;
			}
			projectile.rotation += 0.4f * projectile.direction;
		}
	}
}