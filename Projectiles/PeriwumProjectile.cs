using System;
using Terraria;
using Terraria.ModLoader;

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
			projectile.scale = 1f;
			projectile.extraUpdates = 0;
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.magic = true;
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
			for (int i = 0; i < 200; i++)
			{
				float shootToX = Main.npc[i].position.X + (float)Main.npc[i].width * 0.5f - projectile.Center.X;
				float shootToY = Main.npc[i].position.Y + (float)Main.npc[i].height * 0.5f - projectile.Center.Y;
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, shootToX, shootToY, ModContent.ProjectileType<PeriwumProjectile_Small>(), 31, projectile.knockBack, Main.myPlayer);
			}
		}
	}
}