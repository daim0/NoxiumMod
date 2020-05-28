using NoxiumMod.Projectiles.Throwing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Items.Weapons.Throwing
{
	internal class ManacrystalJavelin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mana Healing Javelin");
			Tooltip.SetDefault("Shoots a Manacrystal Javelin." + "\nHas a chance to summon a tiny mana star on the highest peak of its trajectory." + "\nHas a chance to summon a tiny heart once it hits an enemy or tile.");
		}

		public override void SetDefaults()
		{
			item.shootSpeed = 10f;
			item.damage = 10;
			item.knockBack = 5f;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useAnimation = 24;
			item.useTime = 24;
			item.width = 18;
			item.height = 42;
			item.maxStack = 1;
			item.rare = ItemRarityID.Green;

			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.thrown = true;

			item.UseSound = SoundID.Item1;
			item.value = Item.sellPrice(silver: 30);
			item.shoot = ProjectileType<ManacrystalJavelinP>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ManaCrystal, 3);
			recipe.AddIngredient(ItemID.LifeCrystal, 3);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}