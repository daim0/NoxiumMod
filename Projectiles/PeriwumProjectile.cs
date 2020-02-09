
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Projectiles
{
	public class PeriwumProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("PeriwumProjectile");
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			projectile.timeLeft = 600;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

		public override void AI()
        {
			Player player = Main.player[projectile.owner];

			double distance = 110; //how far the projectile circle is from the player
			double degree = (double)projectile.ai[1] + (72 * projectile.ai[0]);
			double radius = degree * (Math.PI / 180);
			
			projectile.position.X = player.Center.X - (int)(Math.Cos(radius) * distance) - projectile.width / 2;
			projectile.position.Y = player.Center.Y - (int)(Math.Sin(radius) * distance) - projectile.height / 2;
			projectile.ai[1] += 4f; // Changes how fast the projectile circles the player
			
			if (++projectile.frameCounter >= 4)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 4)
				{
					projectile.frame = 0;
				}
			}
		}
		

	}
}
