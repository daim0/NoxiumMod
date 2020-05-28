using NoxiumMod.Projectiles.Throwing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Items.Weapons.Throwing
{
	internal class KnifeExperiment : ModItem
	{
		public static bool ActShoot = false;

		public override void SetDefaults()
		{
			item.shootSpeed = 10f;
			item.damage = 10;
			item.knockBack = 5f;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useAnimation = 13;
			item.useTime = 13;
			item.width = 12;
			item.height = 28;
			item.maxStack = 1;
			item.rare = ItemRarityID.Green;

			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.thrown = true;

			item.UseSound = SoundID.Item1;
			item.value = Item.sellPrice(silver: 30);
			item.shoot = ProjectileType<KnifeEP>();
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			//code basically from ea, modified to better fit the mechanics.
			if (player.altFunctionUse == 2)
			{
				ActShoot = true;
				item.shoot = ProjectileID.None;
			}
			else
			{
				ActShoot = false;
				item.shoot = ProjectileType<KnifeEP>();
			}
			return true;
		}
	}
}