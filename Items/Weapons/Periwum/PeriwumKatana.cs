using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace NoxiumMod.Items.Weapons.Periwum
{
	class PeriwumKatana : ModItem
	{

		public override void SetDefaults()
		{
			item.damage = 50;
			item.melee = true;
			item.width = 58;
			item.height = 62;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 6;
			item.value = Item.buyPrice(gold: 1);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		int timesHit = 0;
		public override bool AltFunctionUse(Player player)
		{
			if (timesHit == 3)
			{
				return true;
			}
			else return false;
		}
		public override bool CanUseItem(Player player)
		{
			//code basically from ea, modified to better fit the mechanics.
			if (player.altFunctionUse == 2)
			{
				timesHit = 0;
				if (player.direction == 1)
				{
					item.noMelee = true;
					item.noUseGraphic = true;
					player.velocity.X += 13f;
					item.useStyle = ItemUseStyleID.HoldingOut;
				}
				if (player.direction == -1)
				{
					item.noMelee = true;
					item.noUseGraphic = true;
					player.velocity.X -= 13f;
					item.useStyle = ItemUseStyleID.HoldingOut;
				}
				player.GetModPlayer<NoxiumPlayer>().KatanaDash = true;
				player.GetModPlayer<NoxiumPlayer>().dashTimer = 60;
			}
			else
			{
				item.noMelee = false;
				item.noUseGraphic = false;
				item.useStyle = ItemUseStyleID.SwingThrow;
			}

			return base.CanUseItem(player);
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			timesHit++;
			if (timesHit >= 3)
			{
				timesHit = 3;
			}
			if (timesHit == 1)
			{
				for (int d = 0; d < 40; d++)
				{
					Dust.NewDust(player.position, player.width, player.height, 8, 0f, 0f, 150, Color.LightPink, 1.5f);
				}
			}
			if (timesHit == 2)
			{
				for (int d = 0; d < 40; d++)
				{
					Dust.NewDust(player.position, player.width, player.height, 8, 0f, 0f, 150, Color.Aquamarine, 1.5f);
				}
			}
			if (timesHit == 3)
			{
				Main.PlaySound(SoundID.NPCHit53.WithVolume(.69f).WithPitchVariance(1.3f));
				for (int d = 0; d < 40; d++)
				{
					Dust.NewDust(player.position, player.width, player.height, 8, 0f, 0f, 150, Color.Magenta, 1.5f);
				}
			}
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.Periwum>(), 10);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
    }
}
