using Terraria;
using Terraria.ModLoader;

namespace NoxiumMod.Projectiles
{
	public class PeriwumProjectile_Small : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 4;
			DisplayName.SetDefault("PeriwumProjectile_Small");
		}

		public override void SetDefaults()
		{
			projectile.scale = 1f;
			projectile.extraUpdates = 0;
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.magic = true;
		}
	}
}