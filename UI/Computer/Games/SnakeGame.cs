using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using NoxiumMod.Utilities;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace NoxiumMod.UI.Computer.Games
{
    public class SnakeGame : ComputerGame
    {
        private KeyboardState Key => Keyboard.GetState();

        private bool PressedExit => oldKeyState.IsKeyDown(Keys.Escape) && !Key.IsKeyDown(Keys.Escape);

        private KeyboardState oldKeyState;

        private int countToBeginning;
        private int gameTimer;
        private int score;

        private Snake snake;

        private Vector2 lastMove;

        private bool gameOver;

        private List<Vector2> snakeSegmentsCache;
        private List<Vector2> possibleFoodSpawns;

        private SnakeFood snakeFood;

        private string endText;

        public override void OnBegin(Rectangle screenBounds)
        {
            Vector2 start = screenBounds.GetPos() + offset + new Vector2(160, 75);

            List<Vector2> startSegments = new List<Vector2>
            {
                start,
                start + new Vector2(0, 10),
                start + new Vector2(0, 20)
            };

            snake = new Snake(startSegments);

            snakeSegmentsCache = startSegments;

            possibleFoodSpawns = new List<Vector2>();

            for (int i = 1; i < 31; i++)
            {
                for (int j = 1; j < 14; j++)
                {
                    possibleFoodSpawns.Add(new Vector2(i * 10, j * 10) - new Vector2(0, 5));
                }
            }
        }

        public override void Update(ComputerUI parent, Rectangle screenBounds)
        {
            Main.blockInput = true;

            if (snake.segments.Count >= 390)
            {
                gameOver = true;

                endText = "You win!";
            }

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
                gameTimer++;

                Vector2 move = GetMoveDir();

                if (lastMove == Vector2.Zero && move == Vector2.UnitY)
                {
                    move = lastMove;
                }

                if (move != lastMove)
                {
                    lastMove = move;
                }

                if (gameTimer % 12 == 0)
                {
                    if (move != Vector2.Zero)
                    {
                        snakeSegmentsCache.Insert(0, snake.segments[0]);

                        if (snakeSegmentsCache.Count > snake.segments.Count)
                        {
                            snakeSegmentsCache.RemoveAt(snakeSegmentsCache.Count - 1);
                        }

                        snake.segments[0] += move * 10;

                        for (int i = 1; i < snakeSegmentsCache.Count - 1; i++)
                        {
                            snake.segments[i] = snakeSegmentsCache[i];
                        }

                        snakeSegmentsCache.RemoveAt(snakeSegmentsCache.Count - 1);
                    }
                }

                if (snakeFood == null && move != Vector2.Zero)
                {
                    Vector2 possiblePos = Main.rand.Next(possibleFoodSpawns);

                    while (new Rectangle((int)possiblePos.X, (int)possiblePos.Y, 10, 10).Intersects(new Rectangle((int)snake.segments[0].X, (int)snake.segments[0].Y, 10, 10)))
                    {
                        possiblePos = Main.rand.Next(possibleFoodSpawns);
                    }

                    snakeFood = new SnakeFood(possiblePos + screenBounds.GetPos() + offset);
                }

                if (snakeFood != null && new Rectangle((int)snakeFood.position.X, (int)snakeFood.position.Y, 10, 10).Intersects(new Rectangle((int)snake.segments[0].X, (int)snake.segments[0].Y, 10, 10)))
                {
                    score++;

                    snakeFood = null;

                    snake.segments.Add(snakeSegmentsCache[snakeSegmentsCache.Count - 1]);
                }

                if (!GetBounds(screenBounds).Contains(snake.segments[0].ToPoint()) || AnySegmentsAtSeg0())
                {
                    gameOver = true;

                    countToBeginning = 0;
                }
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
                foreach (Vector2 pos in snake.segments)
                {
                    spriteBatch.Draw(ComputerUI.snakeTexture, pos, Color.White);
                }

                if (snakeFood != null)
                {
                    spriteBatch.Draw(ComputerUI.snakeFoodTexture, snakeFood.position, Color.White);
                }
            }

            if (gameOver)
            {
                if (endText == null)
                {
                    string text = "game over";

                    Vector2 measure = MeasureText(text);

                    ComputerUI.DrawFontText(spriteBatch, new Vector2(screenBounds.Width / 2, (screenBounds.Height / 2) - (measure.Y * 1.5f) - 1) - (measure / 2), text);

                    ScorePlayer sPlayer = Main.LocalPlayer.GetModPlayer<ScorePlayer>();

                    if (score > sPlayer.highScore)
                    {
                        sPlayer.highScore = score;
                    }

                    text = $"your high score is {sPlayer.highScore}";

                    measure = MeasureText(text);

                    ComputerUI.DrawFontText(spriteBatch, new Vector2(screenBounds.Width / 2, (screenBounds.Height / 2) + (measure.Y * 1.5f) + 1) - (measure / 2), text);

                    text = $"you scored {score}";

                    measure = MeasureText(text);

                    ComputerUI.DrawFontText(spriteBatch, new Vector2(screenBounds.Width / 2, screenBounds.Height / 2) - (measure / 2), text);
                }
                else
                {
                    ComputerUI.DrawFontText(spriteBatch, new Vector2(screenBounds.Width / 2, screenBounds.Height / 2) - (MeasureText(endText) / 2), endText);
                }
            }
        }

        private Rectangle GetBounds(Rectangle screenBounds)
        {
            return new Rectangle(screenBounds.X + (int)offset.X, screenBounds.Y + (int)offset.Y, 320, 150);
        }

        private Vector2 GetMoveDir()
        {
            Vector2 move = lastMove;

            if (Key.IsKeyDown(Keys.W) || Key.IsKeyDown(Keys.Up))
            {
                move = -Vector2.UnitY;
            }
            else if (Key.IsKeyDown(Keys.S) || Key.IsKeyDown(Keys.Down))
            {
                move = Vector2.UnitY;
            }
            else if (Key.IsKeyDown(Keys.A) || Key.IsKeyDown(Keys.Left))
            {
                move = -Vector2.UnitX;
            }
            else if (Key.IsKeyDown(Keys.D) || Key.IsKeyDown(Keys.Right))
            {
                move = Vector2.UnitX;
            }

            if (move == -lastMove)
            {
                return lastMove;
            }

            return move;
        }

        private bool AnySegmentsAtSeg0()
        {
            for (int i = 1; i < snake.segments.Count; i++)
            {
                if (snake.segments[0] == snake.segments[i])
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class Snake
    {
        public List<Vector2> segments;

        public Snake(List<Vector2> segments)
        {
            this.segments = segments;
        }
    }

    public class SnakeFood
    {
        public Vector2 position;

        public SnakeFood(Vector2 position)
        {
            this.position = position;
        }
    }

    public class ScorePlayer : ModPlayer
    {
        public int highScore;

        public override TagCompound Save()
        {
            return new TagCompound()
            {
                [nameof(highScore)] = highScore
            };
        }

        public override void Load(TagCompound tag)
        {
            highScore = tag.GetInt(nameof(highScore));
        }
    }
}
