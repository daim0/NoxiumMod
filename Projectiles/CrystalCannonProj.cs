using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Projectiles
{
	public class CrystalCannonProj : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 38;
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

		

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item10, projectile.position);
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 69, 0f, 0f, 0, default(Color), 1f);
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
            int distance = 20 + Math.Max(target.width, target.height);

            Projectile.NewProjectile(target.Center.X + distance, target.Center.Y - distance, 0, 0, mod.ProjectileType("CrystalCannonHitProj"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f); // top right
			
			Projectile.NewProjectile(target.Center.X + distance, target.Center.Y + distance, 0, 0, mod.ProjectileType("CrystalCannonHitProj"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f); // bottom right
			
			Projectile.NewProjectile(target.Center.X - distance, target.Center.Y + distance, 0, 0, mod.ProjectileType("CrystalCannonHitProj"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f); // bottom left
			
			Projectile.NewProjectile(target.Center.X - distance, target.Center.Y - distance, 0, 0, mod.ProjectileType("CrystalCannonHitProj"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f); // top left
		}
	}
}
