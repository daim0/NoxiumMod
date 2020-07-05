using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NoxiumMod.Items.Buffs;


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
            projectile.minion = true;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.netImportant = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 18000;
            projectile.minionSlots = 1;
            projectile.aiStyle = -1;
        }
        public override bool MinionContactDamage()
        {
            return true;
        }
        public override void AI()
        {
            projectile.rotation = projectile.velocity.X * 0.05f;
            projectile.spriteDirection = -projectile.direction;

            Player player = Main.player[projectile.owner];

            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<SentientTetherBuff>());
            }
            if (player.HasBuff(ModContent.BuffType<SentientTetherBuff>()))
            {
                projectile.timeLeft = 2;
            }

            float distanceFromTarget = 700f;
            Vector2 targetCenter = projectile.position;
            bool foundTarget = false;

            float speed = 8f;
            float inertia = 20f;

            Vector2 pp = player.Center - projectile.Center;

            if (Main.myPlayer == player.whoAmI && pp.Length() > 2000f)
            {
                projectile.position = player.Center;
                projectile.velocity *= 0.1f;
                projectile.netUpdate = true;
            }

            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, projectile.Center);
                if (between < 2000f)
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }
            if (!foundTarget)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy())
                    {
                        float between = Vector2.Distance(npc.Center, projectile.Center);
                        bool closest = Vector2.Distance(projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
                        bool closeThroughWall = between < 100f;
                        if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }
            projectile.friendly = foundTarget;

            if(foundTarget)
            {
                if (distanceFromTarget > 40f)
                {
                    Vector2 direction = targetCenter - projectile.Center;
                    direction.Normalize();
                    direction *= speed;
                    projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
                }
            }
            else
            {
                if (pp.Length() > 600f)
                {
                    speed = 20f;
                    inertia = 60f;
                }
                else
                {
                    speed = 12f;
                    inertia = 80f;
                }
                if (pp.Length() > 20f)
                {
                    pp.Normalize();
                    pp *= speed;
                    projectile.velocity = (projectile.velocity * (inertia - 1) + pp) / inertia;
                }
                else if (projectile.velocity == Vector2.Zero)
                {
                    projectile.velocity.X = -0.15f;
                    projectile.velocity.Y = -0.05f;
                }
            }
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
                distToProj.Normalize();
                distToProj *= 9f;
                center += distToProj;
                distToProj = playerCenter - center;
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
