using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoxiumMod.Utilities;
using Microsoft.Xna.Framework.Input;
using System;
using Newtonsoft.Json.Serialization;

namespace NoxiumMod.UI.Computer.Games
{
    public class PongGame : ComputerGame
    {
        private KeyboardState Key => Keyboard.GetState();

        private bool PressedExit => oldKeyState.IsKeyDown(Keys.Escape) && !Key.IsKeyDown(Keys.Escape);

        private bool MovedUp => Key.IsKeyDown(Keys.Up) || Key.IsKeyDown(Keys.W);

        private bool MovedDown => Key.IsKeyDown(Keys.Down) || Key.IsKeyDown(Keys.S);

        private Texture2D paddleTexture;
        private Texture2D ballTexture;
        private Texture2D whiteSquare;

        private Paddle yourPaddle;
        private Paddle aiPaddle;

        private Ball ball;

        private int countToBeginning;
        private int yourScore;
        private int aiScore;
        private int delay;

        private bool gameOver;

        private KeyboardState oldKeyState;

        private const float paddleSpeed = 2.5f;

        public override void OnBegin(Rectangle screenBounds)
        {
            BoardReset(screenBounds);

            paddleTexture = TextureFromRect(yourPaddle.Hitbox);
            ballTexture = TextureFromRect(ball.Hitbox);

            whiteSquare = TextureFromRect(new Rectangle(0, 0, 6, 6));
        }

        private void BoardReset(Rectangle screenBounds)
        {
            yourPaddle = new Paddle(offset - new Vector2(-8, 20) + new Vector2(4, 75));
            aiPaddle = new Paddle(offset - new Vector2(8, 20) + new Vector2(screenBounds.Width - 46, 75));

            ball = new Ball(offset - new Vector2(5, 5) + new Vector2(160, 75))
            {
                velocity = Main.rand.Next(new Vector2[] 
                {
                    new Vector2(1, -1),
                    new Vector2(1, 1),
                    new Vector2(-1, 1),
                    new Vector2(-1, -1)
                }) * 4
            };

            delay = 80;
        }

        public override void Update(ComputerUI parent, Rectangle screenBounds)
        {
            Rectangle actualBounds = new Rectangle((int)offset.X, (int)offset.Y, 320, 150);

            Main.blockInput = true;

            if (PressedExit || (gameOver && countToBeginning > 120))
            {
                parent.StopVideoGame();
            }

            if (countToBeginning < 120 && !gameOver)
            {
                countToBeginning++;
            }
            else if (countToBeginning >= 120 && !gameOver)
            {
                if (delay > 0)
                {
                    delay--;
                }
                else
                {
                    if (MovedUp && yourPaddle.position.Y > actualBounds.Y)
                    {
                        yourPaddle.position.Y -= paddleSpeed;
                    }
                    else if (MovedDown && yourPaddle.position.Y + 40 < actualBounds.Y + actualBounds.Height)
                    {
                        yourPaddle.position.Y += paddleSpeed;
                    }

                    if (ball.position.X > actualBounds.X + (actualBounds.Width / 2))

                        AIPaddle(actualBounds);

                    Rectangle yourGoal = new Rectangle(actualBounds.X, actualBounds.Y, 8, 150);

                    Rectangle aiGoal = new Rectangle(actualBounds.X + actualBounds.Width - 8, actualBounds.Y, 8, 150);

                    if (ball.Hitbox.Intersects(yourGoal))
                    {
                        aiScore++;

                        BoardReset(screenBounds);
                    }
                    else if (ball.Hitbox.Intersects(aiGoal))
                    {
                        yourScore++;

                        BoardReset(screenBounds);
                    }

                    if (ball.Hitbox.Intersects(yourPaddle.Hitbox))
                    {
                        MakePositive(ref ball.velocity.X);
                    }
                    else if (ball.Hitbox.Intersects(aiPaddle.Hitbox))
                    {
                        MakeNegative(ref ball.velocity.X);
                    }

                    if (ball.position.Y < actualBounds.Y || ball.position.Y + 10 > actualBounds.Y + actualBounds.Height)
                    {
                        ball.velocity.Y *= -1;
                    }

                    ball.position += ball.velocity;
                }
            }

            if (yourScore == 10 || aiScore == 10)
            {
                gameOver = true;

                countToBeginning = 0;
            }

            if (gameOver)
            {
                countToBeginning++;
            }

            oldKeyState = Key;
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle screenBounds)
        {
            if (countToBeginning < 120 && !gameOver)
            {
                string text = "esc to exit";

                Vector2 measure = MeasureText(text);

                ComputerUI.DrawFontText(spriteBatch, new Vector2(screenBounds.Width / 2, (screenBounds.Height / 2) - (measure.Y / 2) - 1) - (measure / 2), text);

                text = "arrow keys or wasd to move";

                measure = MeasureText(text);

                ComputerUI.DrawFontText(spriteBatch, new Vector2(screenBounds.Width / 2, (screenBounds.Height / 2) + (measure.Y / 2) + 1) - (measure / 2), text);
            }
            else if (!gameOver)
            {
                for (int i = 24; i < screenBounds.Height - 16; i += 8)
                {
                    spriteBatch.Draw(whiteSquare, screenBounds.GetPos() + new Vector2(screenBounds.Width / 2, i) - new Vector2(3, 3), Color.White * 0.6f);
                }

                spriteBatch.Draw(paddleTexture, screenBounds.GetPos() + yourPaddle.position, Color.White);

                spriteBatch.Draw(paddleTexture, screenBounds.GetPos() + aiPaddle.position, Color.White);

                spriteBatch.Draw(ballTexture, screenBounds.GetPos() + ball.position, Color.White);

                ComputerUI.DrawFontText(spriteBatch, new Vector2((screenBounds.Width / 2) - 16, 32), yourScore.ToString());

                ComputerUI.DrawFontText(spriteBatch, new Vector2((screenBounds.Width / 2) + 6, 32), aiScore.ToString());
            }

            if (gameOver)
            {
                string text = yourScore == 10 ? "you win" : "you lose";

                Vector2 measure = MeasureText(text);

                ComputerUI.DrawFontText(spriteBatch, new Vector2(screenBounds.Width / 2, screenBounds.Height / 2) - (measure / 2), text);
            }
        }

        private Texture2D TextureFromRect(Rectangle rect)
        {
            Texture2D tex = new Texture2D(Main.graphics.GraphicsDevice, rect.Width, rect.Height);

            Color[] data = new Color[tex.Width * tex.Height];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Color.White;
            }
            tex.SetData(data);

            return tex;
        }

        private void AIPaddle(Rectangle actualBounds)
        {
            if (aiPaddle.Center.Y > ball.Center.Y && aiPaddle.position.Y > actualBounds.Y)
            {
                aiPaddle.position.Y -= paddleSpeed / 2;
            }
            else if (aiPaddle.Center.Y < ball.position.Y && aiPaddle.position.Y + 40 < actualBounds.Y + actualBounds.Height)
            {
                aiPaddle.position.Y += paddleSpeed / 2;
            }
        }

        private void MakePositive(ref float val)
        {
            if (val < 0)
            {
                val *= -1;
            }
        }

        private void MakeNegative(ref float val)
        {
            if (val > 0)
            {
                val *= -1;
            }
        }
    }

    public class Ball
    {
        public Vector2 position;

        public Vector2 velocity;

        public Rectangle Hitbox => new Rectangle((int)position.X, (int)position.Y, 10, 10);

        public Vector2 Center => position + new Vector2(5, 5);

        public Ball(Vector2 position)
        {
            this.position = position;
        }
    }

    public class Paddle
    {
        public Vector2 position;

        public Rectangle Hitbox => new Rectangle((int)position.X, (int)position.Y, 10, 40);

        public Vector2 Center => position + new Vector2(5, 20);

        public Paddle(Vector2 position)
        {
            this.position = position;
        }
    }
}
