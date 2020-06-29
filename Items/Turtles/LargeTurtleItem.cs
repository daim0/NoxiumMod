using NoxiumMod.Projectiles.Turtles;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Turtles
{
	public class LargeTurtleItem : TurtleItem
	{
		public override int MaxArea => 800;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Large Turtle");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();

			item.width = 46;
			item.height = 22;
			item.shoot = ModContent.ProjectileType<LargeTurtleProjectile>();
		}
	}
}
