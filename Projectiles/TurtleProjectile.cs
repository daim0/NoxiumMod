using System;
using Terraria;
using Terraria.ModLoader;

namespace NoxiumMod.Projectiles
{
	public class TurtleProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 42;
			projectile.height = 26;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.aiStyle = -1;
		}

		public override void AI()
		{
			while (!Main.tile[(int)(projectile.position.X / 16), (int)((projectile.position.Y + projectile.height + 1) / 16)].active())
				projectile.position.Y++;

			projectile.position.X = (int)Math.Floor(projectile.position.X);
		}
	}
}
