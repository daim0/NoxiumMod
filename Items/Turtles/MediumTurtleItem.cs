using NoxiumMod.Projectiles.Turtles;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Turtles
{
	public class MediumTurtleItem : TurtleItem
	{
		public override int MaxArea => 400;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Medium Turtle");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();

			item.width = 34;
			item.height = 22;
			item.shoot = ModContent.ProjectileType<MediumTurtleProjectile>();
		}
	}
}
