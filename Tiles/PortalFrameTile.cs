using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoxiumMod.Items.Placeable;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NoxiumMod.Tiles
{
	public class PortalFrameTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileWaterDeath[Type] = false;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Width = 5;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.WaterDeath = false;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<PortalFrameTE>().Hook_AfterPlacement, -1, 0, true);
			TileObjectData.addTile(Type);

			disableSmartCursor = true;
		}

		// TODO: Killing the tile should disable the UI but this requires tracking :/
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
			=> Item.NewItem(i * 16, j * 16, 16, 48, ModContent.ItemType<PortalFrameItem>());

		public override void DrawEffects(int x, int y, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
		{
			if (Main.tile[x, y].type == Type && Main.tile[x, y].frameX == 36 && Main.tile[x, y].frameY == 0)
			{
				if (nextSpecialDrawIndex < Main.specX.Length)
				{
					Main.specX[nextSpecialDrawIndex] = x;
					Main.specY[nextSpecialDrawIndex] = y;
					nextSpecialDrawIndex++;
				}
			}
		}

		public override void SpecialDraw(int x, int y, SpriteBatch spriteBatch)
		{
			if (Main.tile[x, y].type == Type)
			{
				Texture2D portalTexture = mod.GetTexture("Tiles/Portal");
				int frameHeight = portalTexture.Height / 8;
				int frameY = (int)(Main.GlobalTime * 12f) % 8 * frameHeight;

				Vector2 offset = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
				Vector2 drawPosition = new Vector2(x * 16 + 8, y * 16) - Main.screenPosition + offset;
				Rectangle sourceRectangle = new Rectangle(0, frameY, portalTexture.Width, frameHeight);
				Color brightenColor = new Color((byte)(100 + 150 * System.Math.Abs(System.Math.Sin(Main.GlobalTime))), 255, 255);
				spriteBatch.Draw(portalTexture, drawPosition, sourceRectangle, brightenColor, 0, new Vector2(portalTexture.Width / 2, frameHeight), 1f, SpriteEffects.None, 0f);
			}
		}

		public override void MouseOver(int i, int j)
		{
			Main.LocalPlayer.showItemIcon2 = ModContent.ItemType<PortalFrameItem>();
			Main.LocalPlayer.showItemIcon = true;
		}

		// TODO: Make the portal itself right clickable in order to show the UI. I can probably solve this with extending tileobjectdata height
		public override bool NewRightClick(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int left = i - (tile.frameX / 18);
			int top = j - (tile.frameY / 18);

			int index = ModContent.GetInstance<PortalFrameTE>().Find(left, top);
			if (index == -1)
				return true;

			PortalFrameTE portalFrameTE = (PortalFrameTE)TileEntity.ByID[index];
			Main.LocalPlayer.GetModPlayer<PortalPlayer>().portalTileTE = portalFrameTE;

			//Your UI toggle code here
			ModContent.GetInstance<NoxiumMod>().ToggleDimensionalUI();

			return true;
		}
	}

	public class PortalFrameTE : ModTileEntity
	{
		public override bool ValidTile(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			bool valid = tile.active() && tile.frameX % 18 == 0 && tile.frameY == 0 && tile.type == ModContent.TileType<PortalFrameTile>();
			return valid;
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 3); // this is -1, -1, however, because -1, -1 places the 3 diameter square over all the tiles, which are sent to other clients as an update.
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);
				return -1;
			}

			int num = Place(i, j);
			return num;
		}

	}

	public class PortalPlayer : ModPlayer
	{
		public ModTileEntity portalTileTE;
		int counter = 0;

		public override void PreUpdateMovement()
		{
			counter += 1;
			//If your UI is visble, for some reason SGAmod would not let me access theirs despite it existing :/
			//if (SGAmod.CustomUIMenu.visible)
			//{
			if (portalTileTE == null || ((portalTileTE.Position.ToWorldCoordinates() + new Vector2(40, 0) - player.Center).Length() > 120))
			{
				portalTileTE = null;
				ModContent.GetInstance<NoxiumMod>().dimensionalInterface.SetState(null);
			}
		}
	}
}