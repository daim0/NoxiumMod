using System;
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
			Main.projFrames[projectile.type] = 4;
			DisplayName.SetDefault("PeriwumProjectile");
		}

		public override void SetDefaults()
		{
			base.projectile.scale = 1f;
			base.projectile.extraUpdates = 0;
			base.projectile.width = 14;
			base.projectile.height = 14;
			base.projectile.friendly = true;
			base.projectile.penetrate = -1;
			base.projectile.tileCollide = false;
			base.projectile.magic = true;
		}

		public override void AI() 
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 8) 
			{
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % 3;
			}

			Player player = Main.player[projectile.owner];

			double distance = 110; //how far the projectile circle is from the player
			double degree = (double)projectile.ai[1] + (72 * projectile.ai[0]);
			double radius = degree * (Math.PI / 180);
			
			projectile.position.X = player.Center.X - (int)(Math.Cos(radius) * distance) - projectile.width / 2;
			projectile.position.Y = player.Center.Y - (int)(Math.Sin(radius) * distance) - projectile.height / 2;
			projectile.ai[1] += 4f; // How fast it circles the player
	    }
	}
}
