using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using NoxiumMod.Items.Weapons.Throwing;

namespace NoxiumMod.Projectiles.Throwing
{
    class UndyneP : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 38;
            projectile.height = 90;
            projectile.friendly = true;
            projectile.thrown = true;
            projectile.scale = 0.92f;
            projectile.tileCollide = false;
            projectile.timeLeft = 400;
            projectile.penetrate = -1;
            projectile.alpha = 0;
        }
        int Timer;

        Vector2 position;
        public override void AI()
        {
            projectile.alpha++:
            Timer++;
            if (Timer <= 40)
            {
                float rotationsPerSecond = 2f;
                projectile.rotation += MathHelper.ToRadians(rotationsPerSecond * 6f);
                position = Main.MouseWorld;
            } else if(Timer <= 70)
            {
                projectile.rotation = projectile.DirectionTo(position).ToRotation() + MathHelper.PiOver2;
            }
            else if (Timer <= 71)
            {
                projectile.velocity = projectile.DirectionTo(position) * 9f;
            }
        }
    }
}
