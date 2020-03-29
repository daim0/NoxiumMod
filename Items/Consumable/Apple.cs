
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Consumable
{
	class Apple : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 24;
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.maxStack = 30;
			item.consumable = true;
			item.healLife = 20;
			item.potion = true;
			item.value = Item.buyPrice(copper: 10);
		}
	}
}
