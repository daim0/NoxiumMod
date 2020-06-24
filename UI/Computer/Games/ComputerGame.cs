using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoxiumMod.UI.Computer.Games
{
    public abstract class ComputerGame
    {
        protected ComputerUI ComputerUI => NoxiumMod.noxiumInstance.computerUI.computer;

        protected static readonly Vector2 offset = new Vector2(16, 20);

        public virtual void OnBegin(Rectangle screenBounds)
        {
        }

        public virtual void Update(ComputerUI parent, Rectangle screenBounds)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch, Rectangle screenBounds)
        {
        }

        protected Vector2 MeasureText(string text)
        {
            return new Vector2(text.Length * 12, 10);
        }
    }
}
