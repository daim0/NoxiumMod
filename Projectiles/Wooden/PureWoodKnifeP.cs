using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

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
