using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Projectiles
{
	public class ChartreumBladeMainProj : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 12;
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
			int greenDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 61, 0f, 0f, 0, default(Color), 1f);
			Main.dust[greenDust].noGravity = true;
			Main.dust[greenDust].scale = 0.5f;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item10, projectile.position);
			Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
			for (int i = 0; i < 5; i++)
			{
				int greenDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 61, 0f, 0f, 0, default(Color), 1f);
				Main.dust[greenDust].noGravity = true;
			}
		}
	}
}
