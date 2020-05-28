using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace NoxiumMod.UI
{
    public class AhmBar : UIState
    {
        public Texture2D BorderTexture;
        public Texture2D BarTexture;
        public Texture2D BottomTexture;

        public override void OnInitialize()
        {
            BorderTexture = ModContent.GetTexture("NoxiumMod/UI/UI_Exterior");
            BarTexture = ModContent.GetTexture("NoxiumMod/UI/UI_Bar");
            BottomTexture = ModContent.GetTexture("NoxiumMod/UI/UI_Interior");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (NoxiumWorld.ahmBarShown)
            {
                //adjust the position in screen
                Vector2 ScreenPos = new Vector2(Main.screenWidth / 2 - BorderTexture.Width / 2 + 600, Main.screenHeight / 2 - 410);
                //adjust the bar position relative to the frame
                Vector2 BarAdjust = new Vector2(18, 54);
                int length = ((NoxiumWorld.ahmTimer * BarTexture.Height) / NoxiumWorld.ahmTimerCap);
                //Draw frame
                spriteBatch.Draw(BottomTexture, ScreenPos + new Vector2(0, 2), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                //Draw bar
                spriteBatch.Draw(BarTexture, ScreenPos + BarAdjust + new Vector2(0, length), new Rectangle(0, 0, BarTexture.Width, BarTexture.Height - length), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                spriteBatch.Draw(BorderTexture, ScreenPos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }
        }
    }
}