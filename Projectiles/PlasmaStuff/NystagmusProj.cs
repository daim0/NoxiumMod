
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Projectiles.PlasmaStuff
{
    class NystagmusProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 5f;
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 250f;
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 13f;
        }

        public override void SetDefaults()
        {
            projectile.extraUpdates = 0;
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 99;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.scale = 1f;
        }
        private int ShootTimer;
        public override void PostAI()
        {
            ShootTimer++;
            if(ShootTimer == 90)
            {
                ShootTimer = 0;
                float wantedVelocity = 7f;
                Vector2 unit = Vector2.UnitX * wantedVelocity;
                for (int i = 0; i < 36; i++)
                {
                    float angle = MathHelper.ToRadians(10 * i);
                    Vector2 vector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    Dust dust = Dust.NewDustPerfect(projectile.Center + (vector * 40), 30, vector * 8f);
                    dust.noGravity = true;
                }
                for (int i = 0; i < 3; i++)
                    Projectile.NewProjectile(projectile.Center + new Vector2(Main.rand.Next(-50, 50), Main.rand.Next(-20,20)), unit, ModContent.ProjectileType<NystagmusBub>(), 4, 0, projectile.owner);
            }
        }
    }
}