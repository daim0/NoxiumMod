using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Projectiles.PlasmaStuff
{
    class MicroBubble2 : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.friendly = true;
            projectile.timeLeft = 200;
        }
        public override void AI()
        {
            Vector2 floatypos = new Vector2((float)Math.Cos(Main.GlobalTime / 1f) * .25f, (float)Math.Sin(Main.GlobalTime / 1.37f) * .1f);
            projectile.velocity = floatypos + new Vector2(0, -1);
        }
    }
}
