namespace NoxiumMod.Projectiles.Turtles
{
	public class MediumTurtleProjectile : TurtleProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();

			projectile.width = 34;
			projectile.height = 22;
		}
	}
}
