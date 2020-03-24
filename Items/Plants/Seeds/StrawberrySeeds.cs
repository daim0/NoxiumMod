using Microsoft.Xna.Framework;
using NoxiumMod.Tiles.Plants;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Plants.Seeds
{
	public class StrawberrySeeds : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seeds for the Strawbs");
			Tooltip.SetDefault("Mmmm... planty!");
		}

		public override void SetDefaults()
		{
			item.Size = new Vector2(36, 28); //set width to 36 and height to 28
			item.consumable = true; //make this item consume on use
			item.useTime = item.useAnimation = 20; //set the use animation and use time to 20
			item.useTurn = true; //make the player able to turn while using this item
			item.autoReuse = true; //Make this item able to hold and re use.
			item.maxStack = 99; //make the max stack this item can stack up to 99
			item.useStyle = ItemUseStyleID.SwingThrow; //set the use style to the swing throw use style to match vanilla and look better
			item.createTile = ModContent.TileType<StrawberryPlantTile>();

			//CHANGE LATER
			item.value = Item.sellPrice(1); //the sell price in copper coins for this item
			item.rare = ItemRarityID.LightRed; //the rarity
		}
	}
}