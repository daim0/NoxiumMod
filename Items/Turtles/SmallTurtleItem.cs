using NoxiumMod.Projectiles.Turtles;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Turtles
{
	public class SmallTurtleItem : TurtleItem
	{
		public override int MaxArea => 200;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Small Turtle");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();

			item.width = 26;
			item.height = 16;
			item.shoot = ModContent.ProjectileType<SmallTurtleProjectile>();
		}
	}
}
