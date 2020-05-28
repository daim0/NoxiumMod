using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Weapons.Throwing
{
	internal class TinyHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiny Heart");
			Tooltip.SetDefault("You shouldn't be able to read this");
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 10;
			item.height = 10;
			item.rare = ItemRarityID.Blue;
			item.scale = 2f;
		}

		public override bool OnPickup(Player player)
		{
			NoxiumPlayer modPlayer = player.GetModPlayer<NoxiumPlayer>();
			item.active = false;
			Main.PlaySound(SoundID.Item3, player.Center);
			Player p = Main.player[item.owner];
			p.HealEffect(2, true);
			return false;
		}

		public override bool ItemSpace(Player player)
		{
			return true;
		}
	}
}