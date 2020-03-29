using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Projectiles.Wooden
{
	class PureWoodArrowP : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 24;
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			aiType = ProjectileID.WoodenArrowFriendly;
		}
	}
}
