using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
namespace NoxiumMod.Items.Weapons.Chartreum
{
	public class ChartreumBow : ModItem
	{
		public override void SetDefaults()
		{
			item.damage = 25;
			item.ranged = true;
			item.width = 35;
			item.height = 60;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 5;
			item.value = 13;
			item.rare = 4;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 16f;
			item.useAmmo = AmmoID.Arrow;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(4, 0);
		}

		public override float UseTimeMultiplier(Player player)
		{
			for (int i = 15; i < 20; i++)
			{

			}
			return base.UseTimeMultiplier(player);
		}
	}
}