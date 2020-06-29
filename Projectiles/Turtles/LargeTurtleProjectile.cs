namespace NoxiumMod.Projectiles.Turtles
{
	public class LargeTurtleProjectile : TurtleProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();

			projectile.width = 46;
			projectile.height = 28;
		}
	}
}
