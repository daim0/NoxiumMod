using Terraria.ModLoader;

namespace NoxiumMod.Projectiles
{
	internal class CrystalBoomer : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 78;
			projectile.height = 78;
			projectile.aiStyle = 3;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = 10;
			projectile.extraUpdates = 1;
			projectile.tileCollide = false;
		}
	}
}