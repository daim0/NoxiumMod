using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Weapons.Crystal
{
	internal class CrystalShuriken : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 78;
			item.height = 78;
			item.damage = 45;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useAnimation = 25;
			item.useTime = 15;
			item.shootSpeed = 15f;
			item.knockBack = 5f;
			item.scale = 1f;
			item.rare = ItemRarityID.Pink;
			item.value = Item.sellPrice(silver: 300);

			item.melee = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.autoReuse = true;

			item.UseSound = SoundID.Item1;
			item.shoot = mod.ProjectileType("CrystalBoomer");
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[item.shoot] < 5;
		}
	}
}