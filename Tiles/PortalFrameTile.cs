//#define Debugstuff

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Tiles
{
    class PortalFrameTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Width = 5;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            Main.tileWaterDeath[Type] = false;
            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.newTile.Origin = new Point16(0, 2 - 1);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(GetInstance<PortalFrameTE>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addTile(Type);
            disableSmartCursor = true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 16, 48, mod.ItemType("PortalFrame"));
        }

        public override void DrawEffects(int x, int y, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
        {
            if (Main.tile[x, y].type == base.Type && Main.tile[x, y].frameX == 36 && Main.tile[x, y].frameY == 0)
            {
                if (nextSpecialDrawIndex < Main.specX.Length)
                {
                    Main.specX[nextSpecialDrawIndex] = x;
                    Main.specY[nextSpecialDrawIndex] = y;
                    nextSpecialDrawIndex += 1;
                }
            }
        }
        public override void SpecialDraw(int x, int y, SpriteBatch spriteBatch)
        {
            if (Main.tile[x, y].type == base.Type)
            {
                Texture2D portaltex = mod.GetTexture("Tiles/Portal");
                int index = (int)(Main.GlobalTime * 12f);
                index %= 8;
                index *= (portaltex.Height / 8);


                Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
                Vector2 drawOffset = new Vector2((float)(x * 16) - Main.screenPosition.X, (float)(y * 16) - Main.screenPosition.Y) + zerooroffset;

                spriteBatch.Draw(portaltex, drawOffset + new Vector2(8, 0), new Rectangle(0, index, portaltex.Width, (portaltex.Height / 8)), Color.White, 0, new Vector2(portaltex.Width / 2, portaltex.Height / 8), 1f, SpriteEffects.None, 0f);
            }
        }

        public override void MouseOver(int i, int j)
        {
            Main.LocalPlayer.showItemIcon2 = mod.ItemType("PortalFrame");
            Main.LocalPlayer.showItemIconText = "Travel to Dimensions";
            Main.LocalPlayer.showItemIcon = true;
        }

        public override bool NewRightClick(int i, int j)
        {

            Tile tile = Main.tile[i, j];
            int left = i - (tile.frameX / 18);
            int top = j - (tile.frameY / 18);

            int index = GetInstance<PortalFrameTE>().Find(left, top);
            if (index == -1)
            {
                return true;
            }
            PortalFrameTE PortalFrameTE = (PortalFrameTE)TileEntity.ByID[index];
            Main.LocalPlayer.GetModPlayer<PortalPlayer>().portalTileTE = PortalFrameTE;

#if Debugstuff
            Main.NewText("Toggled");
#endif


            //Your UI toggle code here
            NoxiumMod.noxiumInstance.ToggleDimensionalUI();

            return true;
        }

        /*TODO:

        Aight well I have no idea how to do this, but when the tile is placed a portal must spawn above it, portal sprite is on the discord, it is animated
        This portal would be right clickable and once right clicked a UI menu will pop out.
        Portal must be aligned with the middle of the tile and despawn once the tile is broken
        I supposed it could be an npc? Idk whatever works for the portal.
         */
    }

    public class PortalFrameTE : ModTileEntity
    {

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            bool valid= tile.active() && tile.frameX % 18 == 0 && tile.frameY == 0 && tile.type == TileType<PortalFrameTile>();
            return valid;// && tile.frameX % 36 == 0 && tile.frameY == 0;
        }

        public PortalFrameTE()
        {
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
#if Debugstuff
            Main.NewText("Placed");
#endif

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
                if (portalTileTE == null || (((portalTileTE.Position.ToVector2()+new Vector2(2,0)) *16)+new Vector2(8,0)-player.Center).Length()>120)
                {
                portalTileTE = null;
#if Debugstuff
                if (counter%90==0)
                Main.NewText("Untoggled");
#endif

                //Un-toggle UI here
            }

            //}
        }

    }


}
