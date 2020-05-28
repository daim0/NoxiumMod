using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Projectiles
{
	public class CrystalCannonProj : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 24;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.penetrate = 1;
			projectile.light = 0.65f;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.extraUpdates = 1;
			this.aiType = 14;
		}

		public override void AI()
		{
			int greenDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 61, 0f, 0f, 0, default, 1f);
			Main.dust[greenDust].noGravity = true;
			Main.dust[greenDust].scale = 0.6f;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item10, projectile.position);
			Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
			for (int i = 0; i < 5; i++)
			{
				int greenDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 61, 0f, 0f, 0, default, 1f);
				Main.dust[greenDust].noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int distance = Math.Max(target.width, target.height) / 2;

			Projectile.NewProjectile(target.Center.X + distance + 20, target.Center.Y - distance - 20, 0, 0, mod.ProjectileType("CrystalCannonHitProj"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f); // top right

			Projectile.NewProjectile(target.Center.X + distance + 20, target.Center.Y + distance + 20, 0, 0, mod.ProjectileType("CrystalCannonHitProj"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f); // bottom right

			Projectile.NewProjectile(target.Center.X - distance - 10, target.Center.Y + distance + 20, 0, 0, mod.ProjectileType("CrystalCannonHitProj"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f); // bottom left

			Projectile.NewProjectile(target.Center.X - distance - 10, target.Center.Y - distance - 20, 0, 0, mod.ProjectileType("CrystalCannonHitProj"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f); // top left
		}
	}
}