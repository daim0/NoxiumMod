using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace NoxiumMod.Utilities
{
	public static class ItemUtils
	{
		public static void PutItemInInventory(Player player, int type, int stack = 1, bool itemText = true, byte prefixGiven = 0)
		{
			//Check if there is already an item of the type provided and if so, add to its stack and return
			for (int i = 0; i < player.inventory.Length; i++) //loop through the player inventory
			{
				Item inventoryItem = player.inventory[i]; //Get the item at the currrent iriteration of the loop

				//check if its stack is bigger than 0 (if there is an item in that slot)
				if (inventoryItem.stack <= 0 || inventoryItem.type != type || inventoryItem.stack >= inventoryItem.maxStack)
					continue; //go to the next iriteration in the loop

				//If still in the iriteration, add to the stack
				inventoryItem.stack += stack;
				return; //return to stop other code
			}

			Item item = new Item();
			item.SetDefaults(type, false);

			Item getItem = player.GetItem(player.whoAmI, item);

			if (getItem.stack > 0)
			{
				int spawnItem = Item.NewItem(player.position, player.width, player.height, type, stack, false, prefixGiven, true);

				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.SyncItem, -1, -1, null, spawnItem, 1f);  //send data with the 21 as the message type
					return;
				}
			}
			else
			{
				item.position = player.Center - (new Vector2(item.width, item.height) / 2); //set the items position to the players position
				item.active = true; //make the item active
				item.stack = stack; //set the respective stack
				item.prefix = prefixGiven; //set the respective prefix

				if (itemText)
					ItemText.NewText(item, 0, false, false);
			}
		}

		public static void RemoveItem(Item item, int stackDeduction = 1)
		{
			if (item.stack > stackDeduction)
				item.stack -= stackDeduction;
			else
				item.TurnToAir();
		}
	}
}