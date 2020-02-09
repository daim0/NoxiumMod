using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

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
			item.damage = 4;
			item.crit = 1;
			item.ranged = true;
			item.width = 16;
			item.height = 16;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 5f;
			item.value = Item.buyPrice(0, 1, 69, 0);
			item.rare = 4;
			item.UseSound = SoundID.Item38;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("CrystalCannonProj");
			item.shootSpeed = 15f;
		}
	}
}
