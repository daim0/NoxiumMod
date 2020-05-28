using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace NoxiumMod.Projectiles.Wooden
{
	class PureWoodSpearP : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.aiStyle = 19;
			projectile.penetrate = -1;
			projectile.scale = 1.3f;
			projectile.alpha = 0;

			projectile.hide = true;
			projectile.ownerHitCheck = true;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.friendly = true;
		}
		public float MovementFactor
		{
			get => projectile.ai[0];
			set => projectile.ai[0] = value;
		}
		public override void AI()
		{
			Player projOwner = Main.player[projectile.owner];

			Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
			projectile.direction = projOwner.direction;
			projOwner.heldProj = projectile.whoAmI;
			projOwner.itemTime = projOwner.itemAnimation;
			projectile.position.X = ownerMountedCenter.X - projectile.width / 2;
			projectile.position.Y = ownerMountedCenter.Y - projectile.height / 2;

			if (!projOwner.frozen)
			{
				if (MovementFactor == 0f)
				{
					MovementFactor = 3f;
					projectile.netUpdate = true;
				}
				if (projOwner.itemAnimation < projOwner.itemAnimationMax / 2)
				{
					MovementFactor -= 2.4f;
				}
				else
				{
					MovementFactor += 2.1f;
				}
			}
			projectile.position += projectile.velocity * MovementFactor;

			if (projOwner.itemAnimation == 0)
			{
				projectile.Kill();
			}

			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
			if (projectile.spriteDirection == -1)
			{
				projectile.rotation -= MathHelper.ToRadians(90f);
			}
		}
	}
}

