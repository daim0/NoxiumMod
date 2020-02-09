
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;

namespace NoxiumMod.Projectiles
{
	public class PeriwumProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 4;
			Main.projPet[projectile.type] = true;
			DisplayName.SetDefault("PeriwumProjectile");
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
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
			int distance = 120;
			float angle = projectile.ai[1] + 72 * projectile.ai[0];
			Vector2 offset = Vector2.UnitX * distance;
			offset = offset.RotatedBy(MathHelper.ToRadians(angle)) - projectile.Size;
			projectile.position = player.Center + offset;
			projectile.ai[1] += 4f;
        }
	}	
}
