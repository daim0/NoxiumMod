
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.GameContent.UI.Elements;
using Terraria.DataStructures;


namespace NoxiumMod.UI
{
    class ahmBar : UIState
    {
        public Texture2D BorderTexture;
        public Texture2D BarTexture;

        public override void OnInitialize()
        {
            BorderTexture = ModContent.GetTexture("NoxiumMod/UI/PlaceholderBar");
            BarTexture = ModContent.GetTexture("NoxiumMod/UI/PlaceholderBarFill");
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (NoxiumWorld.ahmBarShown)
            {
                //adjust the position in screen
                Vector2 ScreenPos = new Vector2(Main.screenWidth / 2 - BorderTexture.Width / 2, Main.screenHeight / 2 - 500);
                //adjust the bar position relative to the frame
                Vector2 BarAdjust = new Vector2(6, 6);
                var length = ((NoxiumWorld.ahmTimer * BarTexture.Width) / NoxiumWorld.ahmTimerCap);
                //Draw frame
                spriteBatch.Draw(BorderTexture, ScreenPos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                //Draw bar
                spriteBatch.Draw(BarTexture, ScreenPos + BarAdjust, new Rectangle(0, 0, length, BarTexture.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }
        }
    }
}
