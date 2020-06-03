using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace NoxiumMod.Projectiles.Summons
{
    class SentientTether : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[base.projectile.type] = 1;
            ProjectileID.Sets.MinionSacrificable[base.projectile.type] = true;
            ProjectileID.Sets.Homing[base.projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ZephyrFish);
            projectile.width = 22;
            projectile.height = 18;
            aiType = ProjectileID.ZephyrFish;
            projectile.minion = true;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.netImportant = true;
            aiType = ProjectileID.ZephyrFish;
            projectile.penetrate = -1;
            projectile.timeLeft = 18000;
            projectile.minionSlots = 1;
        }
        public override void AI()
        {
            bool flag64 = projectile.type == mod.ProjectileType("SentientTether");
            Player player = Main.player[projectile.owner];
            if (flag64)
            {
                if (player.dead)
                    player.GetModPlayer<NoxiumPlayer>().SentientTetherMinion = false;

                if (player.GetModPlayer<NoxiumPlayer>().SentientTetherMinion)
                    projectile.timeLeft = 2;

            }
            projectile.spriteDirection = -projectile.direction;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 playerCenter = Main.player[projectile.owner].MountedCenter;
            Vector2 center = projectile.Center;
            Vector2 distToProj = playerCenter - projectile.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            while (distance > 30f && !float.IsNaN(distance))
            {
                distToProj.Normalize();                 //get unit vector
                distToProj *= 9f;                      //speed = 24
                center += distToProj;                   //update draw position
                distToProj = playerCenter - center;    //update distance
                distance = distToProj.Length();
                Color drawColor = lightColor;

                //Draw chain
                spriteBatch.Draw(mod.GetTexture("Projectiles/Summons/SentientTetherChain"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, 6, 8), drawColor, projRotation,
                    new Vector2(6 * 0.5f, 8 * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}
