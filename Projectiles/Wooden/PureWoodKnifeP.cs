using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Projectiles.Wooden
{
	class PureWoodKnifeP : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 26;
			projectile.CloneDefaults(ProjectileID.ThrowingKnife);
			aiType = ProjectileID.ThrowingKnife;
		}
	}
}
