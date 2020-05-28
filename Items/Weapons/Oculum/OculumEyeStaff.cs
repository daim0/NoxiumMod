using Microsoft.Xna.Framework;
using NoxiumMod.Items.Buffs;
using NoxiumMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Weapons.Oculum
{
	public class OculumEyeStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oculum's Eye Staff");
			Tooltip.SetDefault("Summons a mysterious visionary to fight for you");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true;
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 11;
			item.knockBack = 3f;
			item.mana = 12;
			item.width = 34;
			item.height = 34;
			item.useTime = 28;
			item.useAnimation = 28;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.buyPrice(0, 0, 29, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item44;
			item.noMelee = true;
			item.summon = true;
			item.buffType = ModContent.BuffType<OculumsVisionBuff>();
			item.shoot = ModContent.ProjectileType<OculumsEyeProj>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (Collision.SolidCollision(Main.MouseWorld - new Vector2(24, 48) / 2f, 24, 48))
			{
				player.ClearBuff(ModContent.BuffType<OculumsVisionBuff>());
			}
			player.AddBuff(item.buffType, 2);
			position = Main.MouseWorld;
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("OculumSlate"), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}