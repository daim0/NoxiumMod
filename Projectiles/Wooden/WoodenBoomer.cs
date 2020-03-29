using Terraria.ModLoader;

namespace NoxiumMod.Projectiles.Wooden
{
	class WoodenBoomer : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 32;
			projectile.aiStyle = 3;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = 10;
			projectile.extraUpdates = 1;
			projectile.tileCollide = true;
		}
	}
}
