using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Weapons.Crystal
{
	class CrystalCannon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Cannon");
			Tooltip.SetDefault("Bruh, I can't belive you took the time out of your day to read this tooltip.");
		}

		public override void SetDefaults()
		{
			item.damage = 40;
			item.crit = 1;
			item.ranged = true;
			item.width = 16;
			item.height = 16;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 5f;
			item.value = Item.buyPrice(0, 1, 69, 0); //nice
			item.rare = 5;
			item.UseSound = SoundID.Item38;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("CrystalCannonProj");
			item.shootSpeed = 15f;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2?(new Vector2(-21f, -1f));
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 value = Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
			if (Collision.CanHit(position, 0, 0, position + value, 0, 0))
			{
				position += value;
			}
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
			return false;
		}
	}
}
