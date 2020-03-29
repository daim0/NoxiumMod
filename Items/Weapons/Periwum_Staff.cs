using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Weapons
{

	public class Periwum_Staff : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Periwum Staff");
			Tooltip.SetDefault("SlaveStaff xd");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}


		public override void SetDefaults()
		{
			item.damage = 20;
			item.magic = true;
			item.mana = 6;
			item.width = 40;
			item.height = 40;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shootSpeed = 16f;
			item.shoot = mod.ProjectileType("PeriwumProjectile");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 5;
			float rotation = MathHelper.ToRadians(72);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 72f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI, i);
			}
			return false;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[item.shoot] < 5;
		}
	}
}
