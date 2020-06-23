using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace NoxiumMod.UI.Computer
{
	public class ComputerItemSlot : UIElement
	{
		internal Item item;
		internal Func<Item, bool> validItemFunc;
		internal static Texture2D slotTexture;

		internal int slotWidth;
		internal int slotHeight;
		internal int slotFrame;

		internal bool Empty => item.IsAir;

		public ComputerItemSlot(Func<Item, bool> validItemFunc = null)
		{
			this.validItemFunc = validItemFunc;

			item = new Item();
			item.SetDefaults(0);
		}

		public override void OnInitialize()
		{
			slotTexture = NoxiumMod.noxiumInstance.GetTexture("UI/Computer/ComputerItemSlot");

			slotWidth = slotTexture.Width;
			slotHeight = slotTexture.Height / 10;

			Width.Set(slotWidth, 0f);
			Height.Set(slotHeight, 0f);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			float oldScale = Main.inventoryScale;

			if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
			{
				Main.LocalPlayer.mouseInterface = true;

				if (validItemFunc == null || validItemFunc(Main.mouseItem) || Main.mouseItem.IsAir)
					ItemSlot.Handle(ref item, ItemSlot.Context.BankItem);
			}

			Texture2D itemTexture = Main.itemTexture[item.type];
			Vector2 itemDrawPosition = GetDimensions().Center() - itemTexture.Size() / 2;

			spriteBatch.Draw(slotTexture, GetDimensions().Position(), new Rectangle(0, slotHeight * slotFrame, slotWidth, slotHeight), Color.White);
			spriteBatch.Draw(itemTexture, itemDrawPosition, null, Color.White);
			Main.inventoryScale = oldScale;
		}
	}
}