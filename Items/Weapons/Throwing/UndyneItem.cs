using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Weapons.Throwing
{
	class UndyneItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spear of Justice");
		}
		public override void SetDefaults()
		{
			item.shootSpeed = 10f;
			item.damage = 15;
			item.knockBack = 5f;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useAnimation = 12;
			item.useTime = 12;
			item.width = 64;
			item.height = 64;
			item.maxStack = 1;
			item.rare = ItemRarityID.Green;

			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.thrown = true;

			item.UseSound = SoundID.Item1;
			item.value = Item.sellPrice(silver: 100);
			item.shoot = ProjectileID.WoodenArrowFriendly;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			float Angle = MathHelper.ToRadians(Main.rand.NextFloat(0, 360));
			float distance = 190;
			float pX = distance * (float)Math.Cos(Angle) + Main.MouseWorld.X;
			float pY = distance * (float)Math.Sin(Angle) + Main.MouseWorld.Y;
			Vector2 thing = new Vector2(pX, pY);

			Projectile.NewProjectile(pX, pY, 0f, 0f, mod.ProjectileType("UndyneP"), damage, 1f, player.whoAmI);
			return false;

		}
	}
}
