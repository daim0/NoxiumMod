using Microsoft.Xna.Framework;
using NoxiumMod.Items.Weapons.Throwing;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Projectiles.Throwing
{
	internal class ManacrystalJavelinP : ModProjectile
	{
		private int timer;

		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 42;
			projectile.friendly = true;
			projectile.thrown = true;
			projectile.scale = 0.92f;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Main.rand.NextBool(4))
			{
				Item.NewItem(projectile.getRect(), ItemType<TinyHeart>());
			}
			return true;
		}

		public override void AI()
		{
			Lighting.AddLight(projectile.Center, Main.DiscoColor.ToVector3());

			timer++;

			if (timer >= 45)
			{
				const float velXmult = 0.98f;
				const float velYmult = 0.35f;
				timer = 45;
				projectile.velocity.X *= velXmult;
				projectile.velocity.Y += velYmult;
			}

			projectile.rotation = projectile.velocity.ToRotation() + (MathHelper.Pi / 2);

			if (Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.height, projectile.width, 12, projectile.velocity.X * .2f, projectile.velocity.Y * .2f, 200, Scale: 1.2f);
				dust.velocity += projectile.velocity * 0.3f;
				dust.velocity *= 0.2f;
			}
			if (Main.rand.NextBool(4))
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.height, projectile.width, 15, 0, 0, 254, Scale: 0.3f);
				dust.velocity += projectile.velocity * 0.5f;
				dust.velocity *= 0.5f;
			}
			if (projectile.velocity.Y >= 0 && projectile.oldVelocity.Y < 0)
			{
				if (Main.rand.NextBool(4))
				{
					Item.NewItem(projectile.getRect(), ItemType<TinyStar>());
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(4))
			{
				Item.NewItem(projectile.getRect(), ItemType<TinyHeart>());
			}
		}
	}
}