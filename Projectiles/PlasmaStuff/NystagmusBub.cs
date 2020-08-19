using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace NoxiumMod.Projectiles.PlasmaStuff
{
    class NystagmusBub : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.timeLeft = Main.rand.Next(150,220);
            projectile.magic = true;
            projectile.damage = 5;
        }
        public override void AI()
        {
            Vector2 floatypos = new Vector2((float)Math.Cos(Main.GlobalTime / 1f) * .25f, (float)Math.Sin(Main.GlobalTime / 1.37f) * .25f);
            projectile.velocity = floatypos + new Vector2(0, -1);
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 36; i++)
            {
                float angle = MathHelper.ToRadians(10 * i);
                Vector2 vector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                Dust dust = Dust.NewDustPerfect(projectile.Center + (vector * 10), 240, vector * 3f);
                dust.noGravity = true;

            }
        }
    }
}
