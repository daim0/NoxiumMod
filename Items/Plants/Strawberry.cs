using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoxiumMod.Items.Plants.Seeds;
using NoxiumMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Plants
{
	public class Strawberry : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Strawbs");
			Tooltip.SetDefault($"Mmmm.. tasty!\nUse item to heal 20 health and apply the regeneration buff for 10 seconds\nWhile hovering over the item in the inventory, press the seed key (C by Default) to convert strawberries to strawberry seeds.");
		}

		public override void SetDefaults()
		{
			item.Size = new Vector2(24); //set width and height to 24
			item.consumable = true; //make this item consume on use
			item.useTime = item.useAnimation = 20; //set the use animation and use time to 20
			item.useTurn = true; //make the player able to turn while using this item
			item.maxStack = 99; //make the max stack this item can stack up to 99
			item.healLife = 10; //heal 20 life on use
			item.useStyle = ItemUseStyleID.EatingUsing; //set the use style to the eating use style to match vanilla and look better
			item.potion = true; //give a heal cooldown when using this item
			item.buffType = BuffID.Regeneration; //apply the regn buff on use
			item.buffTime = new Time(10).Ticks; //for 10 seconds

			//CHANGE LATER
			item.value = Item.sellPrice(1); //the sell price in copper coins for this item
			item.rare = ItemRarityID.LightRed; //the rarity
		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			//a rectangle for this items width and height adjusted to the scale of the inventory. 
			//possibly devide width and height fiedls by 2 if position is the center
			Rectangle inventoryItemRect = new Rectangle((int)position.X, (int)position.Y, (int)(item.width * scale), (int)(item.height * scale));

			Player player = Main.player[item.owner]; //the player using this item

			//spriteBatch.Draw(Main.magicPixel, inventoryItemRect, null, Color.White); //Used for debugging purposes to draw the rectangle

			//a quick variable to make our code look cleaner
			Point mousePoint = new Point(Main.mouseX, Main.mouseY);

			//Cop vanillas way of holding a button to pick things up - NOT DONE YET
			if (inventoryItemRect.Contains(mousePoint) && player.GetModPlayer<NoxiumPlayer>().SeedKeyDown)
			{
				//remove the strawberry item from the inventory
				ItemUtils.RemoveItem(item);
				//put the seeds in the inventory
				ItemUtils.PutItemInInventory(player, ModContent.ItemType<StrawberrySeeds>());
			}
		}
	}
}