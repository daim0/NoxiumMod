using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace NoxiumMod.Projectiles.Throwing
{
	internal class LeadWaraxeP : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 48;
			projectile.height = 60;
			projectile.friendly = true;
			projectile.thrown = true;
			projectile.scale = 0.8f;
			projectile.tileCollide = true;
			projectile.penetrate = 1;
		}

		public override void AI()
		{
			float rotationsPerSecond = 3f;
			projectile.rotation += MathHelper.ToRadians(rotationsPerSecond * 6f);
			const float velXmult = 0.98f;
			const float velYmult = 0.25f;
			projectile.velocity.X *= velXmult;
			projectile.velocity.Y += velYmult;
		}
	}
}