using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria.ModLoader;
using static SubworldLibrary.SLWorld;

namespace NoxiumMod.UI.Subworld
{
    public class JoiningUI : UIState
    {
        private static readonly Color whiteColor = new Color(211, 221, 227);

        private static readonly Color blackColor = new Color(34, 31, 46);

        private static Texture2D loadingSymbol;

        private string dots;

        private int dotTimer;

        private float spinnyThingrotationDegrees;

        public static void LoadLoadingSymbol()
        {
            loadingSymbol = ModContent.GetTexture("NoxiumMod/UI/Subworld/LoadingThing");
        }

        public override void Update(GameTime gameTime)
        {
            dots = SetDots();

            spinnyThingrotationDegrees += 6;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Main.magicPixel, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), whiteColor);

            spriteBatch.DrawString(Main.fontDeathText, "Joining" + dots, new Vector2(20, 20), blackColor);

            float ratio = loadingSymbol.Width / (float)loadingSymbol.Height;

            spriteBatch.Draw(loadingSymbol, new Vector2(Main.screenWidth - loadingSymbol.Width - 30, Main.screenHeight - loadingSymbol.Height - (30 * ratio)),
                null, blackColor, MathHelper.ToRadians(spinnyThingrotationDegrees), new Vector2(loadingSymbol.Width / 2, loadingSymbol.Height / 2), 1, SpriteEffects.None, 0);

            int extent = (int)((progress == null ? 0 : progress.Value) * 100);

            Vector2 drawPos = new Vector2(20, Main.screenHeight - 50);

            for (int i = 0; i < extent; i++)
            {
                if (i == 0 || i == extent - 1)
                {
                    spriteBatch.Draw(Main.magicPixel, new Rectangle((int)drawPos.X, (int)drawPos.Y + 4, 12, 20), blackColor);
                }
                else
                {
                    spriteBatch.Draw(Main.magicPixel, new Rectangle((int)drawPos.X, (int)drawPos.Y, 12, 28), blackColor);
                }
                drawPos.X += 12;
            }
        }

        private string SetDots()
        {
            dotTimer++;

            string dots = ".";

            if (dotTimer >= 20)
            {
                dots += ".";
            }
            if (dotTimer >= 40)
            {
                dots += ".";
            }
            if (dotTimer >= 60)
            {
                dotTimer = 0;
            }

            return dots;
        }
    }
}
