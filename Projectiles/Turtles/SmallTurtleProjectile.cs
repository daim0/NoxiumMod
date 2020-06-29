namespace NoxiumMod.Projectiles.Turtles
{
	public class SmallTurtleProjectile : TurtleProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();

			projectile.width = 26;
			projectile.height = 16;
		}
	}
}
