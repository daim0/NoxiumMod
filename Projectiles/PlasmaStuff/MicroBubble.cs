using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace NoxiumMod.Projectiles.PlasmaStuff
{
    class MicroBubble : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.timeLeft = 200;
        }
        public override void AI()
        {
            Vector2 floatypos = new Vector2((float)Math.Cos(Main.GlobalTime / 1f) * .25f, (float)Math.Sin(Main.GlobalTime / 1.37f) * .25f);
            projectile.velocity = floatypos + new Vector2(0, -1);
        }
    }
}
