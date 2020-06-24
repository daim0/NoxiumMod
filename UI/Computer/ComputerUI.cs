using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoxiumMod.Items.Computer;
using NoxiumMod.Items.Other;
using NoxiumMod.UI.Computer.Games;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;

namespace NoxiumMod.UI.Computer
{
	public class ComputerUI : UIElement
	{
		internal static Texture2D screenTexture;
		internal static Texture2D fontTexture;
		internal static Texture2D snakeTexture;
		internal static Texture2D snakeFoodTexture;
		internal static Texture2D floppySlotTexture;

		internal static Dictionary<char, int> specialCharIndices;

		internal string input;
		internal string[] outputLines;
		internal uint littleGuyTimer;
		internal bool showLittleGuy;
		internal int screenFrame;
		internal int screenWidth;
		internal int screenHeight;
		internal bool gaming = false;

		private static bool focused;

		internal ComputerItemSlot pickaxeSlot;
		internal ComputerItemSlot turtleSlot;
		internal ComputerItemSlot floppySlot;

		internal ushort chosenWidth;
		internal ushort chosenHeight;
		internal byte chosenDirection; // to goodpro: this is not an enum because i am lazy. fix it if you want tho

		private ComputerGame activeGame;

		private bool DoingBarrelRoll
		{
			get => NoxiumMod.noxiumInstance.doingBarrelRoll;
			set => NoxiumMod.noxiumInstance.doingBarrelRoll = value;
		}

		public static void PleaseForTheLoveOfGodDontOpenChatWhenIPressEnter()
		{
			On.Terraria.Main.DoUpdate_Enter_ToggleChat += (orig) =>
			{
				if (focused)
				{
					return;
				}
				orig();
			};
		}

		public override void OnInitialize()
		{
			screenTexture = NoxiumMod.noxiumInstance.GetTexture("UI/Computer/ComputerScreen");
			fontTexture = NoxiumMod.noxiumInstance.GetTexture("UI/Computer/ComputerFont");
			floppySlotTexture = NoxiumMod.noxiumInstance.GetTexture("UI/Computer/ComputerFloppySlot");

			snakeTexture = NoxiumMod.noxiumInstance.GetTexture("UI/Computer/Games/SnakeSegment");
			snakeFoodTexture = NoxiumMod.noxiumInstance.GetTexture("UI/Computer/Games/SnakeFood");

			screenWidth = screenTexture.Width;
			screenHeight = screenTexture.Height / 10;

			Width.Set(screenWidth, 0f);
			Height.Set(screenHeight, 0f);

			specialCharIndices = new Dictionary<char, int>()
			{
				{ '.', 26 },
				{ '>', 27 },
				{ '\'', 28 },
				{ ' ', 29 },
				{ '_', 30 }
			};

			// because y'know, strings are null by default
			outputLines = new string[6] { "", "", "", "", "", "" };

			pickaxeSlot = new ComputerItemSlot(item => item.pick > 0);
			pickaxeSlot.Left.Set(Width.Pixels + 10f, 0f);

			turtleSlot = new ComputerItemSlot(item => item.type == ModContent.ItemType<TurtleItem>());
			turtleSlot.Left.Set(Width.Pixels + 10f, 0f);
			turtleSlot.Top.Set(50f, 0f);

			floppySlot = new ComputerItemSlot(item => item.modItem is FloppyDisk, floppySlotTexture);
			floppySlot.Left.Set(Width.Pixels + 10f, 0f);
			floppySlot.Top.Set(100f, 0f);

			Append(pickaxeSlot);
			Append(turtleSlot);
			Append(floppySlot);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			// btw, this only seems to work if i put it in Draw, not Update
			// blame vanilla
			if (focused && activeGame == null)
			{
				PlayerInput.WritingText = true;
				Main.instance.HandleIME();

				string updatedCommand = Main.GetInputText(input);

				if (updatedCommand.Length < 25)
					input = updatedCommand;

				// considering i'm doing text input here might as well do enter-handling here
				if (Main.oldKeyState.IsKeyDown(Keys.Enter) && Main.keyState.IsKeyUp(Keys.Enter))
				{
					HandleCommand();
					Main.chatRelease = true;
				}
			}

			spriteBatch.Draw(screenTexture, GetDimensions().Position(), new Rectangle(0, screenFrame * screenHeight, screenWidth, screenHeight), Color.White);

			if (activeGame == null)
			{
				DrawFontText(spriteBatch, new Vector2(22, 22), ">" + input + (showLittleGuy && focused ? "_" : ""));

				for (int i = 0; i < outputLines.Length; i++)
					DrawFontText(spriteBatch, new Vector2(22, i * 22 + 40), outputLines[i]);
			}
			else
			{
				activeGame.Draw(spriteBatch, GetDimensions().ToRectangle());
			}

			pickaxeSlot.Draw(spriteBatch);
			turtleSlot.Draw(spriteBatch);
			floppySlot.Draw(spriteBatch);
		}

		public void DrawFontText(SpriteBatch spriteBatch, Vector2 offsetPosition, string text)
		{
			char[] characters = text.ToCharArray();

			for (int i = 0; i < characters.Length; i++)
			{
				spriteBatch.Draw(
					fontTexture,
					GetDimensions().Position() + offsetPosition + new Vector2(i * 12, 0),
					new Rectangle(GetCharFontIndex(characters[i]) * 12, 0, 10, 10),
					Color.White
					);
			}
		}

		public override void Update(GameTime gameTime)
		{
			if (!ContainsPoint(Main.MouseScreen) && Main.mouseLeft)
				Unfocus();

			if (activeGame == null)
			{
				littleGuyTimer++;

				if (littleGuyTimer % 30 == 0)
				{
					showLittleGuy = !showLittleGuy;

					if (gaming)
					{
						// maybe clean this up later
						if (screenFrame + 1 > 9)
							screenFrame = 0;
						else
							screenFrame++;

						if (turtleSlot.slotFrame + 1 > 9)
							turtleSlot.slotFrame = 0;
						else
							turtleSlot.slotFrame++;

						if (pickaxeSlot.slotFrame + 1 > 9)
							pickaxeSlot.slotFrame = 0;
						else
							pickaxeSlot.slotFrame++;
					}
				}
			}
			else
			{
				if (floppySlot.Empty && activeGame != null)
				{
					activeGame = null;

					PrintError("disk was removed");

					return;
				}

				activeGame.Update(this, GetDimensions().ToRectangle());
			}
		}

		public void StopVideoGame()
		{
			Unfocus();
			activeGame = null;
		}

		public override void Click(UIMouseEvent evt)
		{
			if (activeGame == null)
			{
				Focus();
			}
		}

		public void HandleCommand()
		{
			if (string.IsNullOrWhiteSpace(input))
				return;

			input = input.ToLower();

			string[] words = input.Split(' ');
			string command = words[0];
			string[] args = new string[1] { "" };

			if (words.Length > 1)
			{
				args = new string[words.Length - 1];

				for (int i = 1; i < words.Length; i++)
					args[i - 1] = words[i];
			}

			if (input == "do a barrel roll" && !DoingBarrelRoll)
			{
				DoingBarrelRoll = true;
				PrintScreen("doing...");
				return;
			}

			switch (command)
			{
				case "hello":
					PrintScreen("hello world");
					break;

				case "clear":
					ClearScreen();
					break;

				case "help":
					PrintScreen("width > set turtle width\nheight > set turtle height\nright > set direction\nleft > set direction\nsave > save setup to disk\nclear > clear screen");
					break;

				case "width":
					if (ulong.TryParse(args[0], out ulong widthResult))
					{
						if (widthResult > ushort.MaxValue)
							PrintError("width too big");
						else
						{
							chosenWidth = (ushort)widthResult;
							PrintScreen("set turtle width to >\n" + widthResult);
						}
					}
					else
						PrintError("no number specified");

					break;

				case "height":
					if (ulong.TryParse(args[0], out ulong heightResult))
					{
						if (heightResult > ushort.MaxValue)
							PrintError("height too big");
						else
						{
							chosenHeight = (ushort)heightResult;
							PrintScreen("set turtle height to >\n" + heightResult);
						}
					}
					else
						PrintError("no number specified");

					break;

				case "left":
					chosenDirection = 1;
					PrintScreen("set turtle direction to >\nleft");
					break;

				case "right":
					chosenDirection = 2;
					PrintScreen("set turtle direction to >\nright");
					break;

				case "save":
					if (turtleSlot.Empty)
					{
						PrintError("no turtle found");
						break;
					}

					if (pickaxeSlot.Empty)
					{
						PrintError("no pickaxe found");
						break;
					}

					if (chosenWidth == 0 && chosenHeight == 0)
					{
						PrintError("width and height both\ncannot be zero");
						break;
					}

					if (chosenDirection == 0)
					{
						PrintError("no direction specified");
						break;
					}

					WriteToTurtle();
					pickaxeSlot.item.TurnToAir();
					ResetComputerState();

					PrintScreen("successfully saved turtle\nto use place turtle down");
					break;

				case "gaming":
					gaming = !gaming;
					break;

				case "doom":
					PrintScreen("no. i dont think i will");
					Main.LocalPlayer.KillMe(PlayerDeathReason.ByCustomReason(Main.LocalPlayer.name + " tried to do the forbidden"), 389348, 1);
					break;

				case "play":
					if (floppySlot.Empty)
					{
						PrintError("no game inserted");
					}
					else
					{
						activeGame = (ComputerGame)Activator.CreateInstance((floppySlot.item.modItem as FloppyDisk).GameType);
						activeGame.OnBegin(GetDimensions().ToRectangle());

						Unfocus();
					}
					break;

				default:
					PrintScreen("unknown command");
					break;
			}

			input = "";
		}

		public void PrintScreen(string text)
		{
			ClearScreen();

			string[] lines = text.Split('\n');

			for (int i = 0; i < lines.Length; i++)
				outputLines[i] = lines[i];
		}

		public void PrintError(string text) => PrintScreen("error\n" + text);

		public void WriteToTurtle()
		{
			TurtleItem turtle = turtleSlot.item.modItem as TurtleItem;
			turtle.TurtleInfo.Width = chosenWidth;
			turtle.TurtleInfo.Height = chosenHeight;
			turtle.TurtleInfo.Direction = chosenDirection;
			turtle.TurtleInfo.PickaxePower = pickaxeSlot.item.pick;
			turtle.TurtleInfo.PickaxeSpeed = pickaxeSlot.item.useTime;
			turtle.TurtleInfo.PickaxeType = pickaxeSlot.item.type;
		}

		public void ResetComputerState()
		{
			chosenDirection = 0;
			chosenHeight = 0;
			chosenWidth = 0;
		}

		public void ClearScreen()
		{
			for (int i = 0; i < outputLines.Length; i++)
				outputLines[i] = "";
		}

		public int GetCharFontIndex(char c)
		{
			if (specialCharIndices.ContainsKey(c))
				return specialCharIndices[c];

			// conveniently, digits map directly to their ASCII number
			// ae 0 -> 0, 1 -> 1, etc
			if (int.TryParse(c.ToString(), out int index))
				return index + 31;

			return (c % 32) - 1;
		}

		public void Focus()
		{
			Main.clrInput();
			focused = true;
			Main.blockInput = true;
			littleGuyTimer = 0;
			showLittleGuy = true;
		}

		public void Unfocus()
		{
			focused = false;
			Main.blockInput = false;
		}
	}
}