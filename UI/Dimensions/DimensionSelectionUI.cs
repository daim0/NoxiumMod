using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace NoxiumMod.UI.Dimensions
{
	public class DimensionSelectionUI : UIState
	{
		private Texture2D mainPanelTexture;
		private Texture2D defaultDimensionTexture;
		private Texture2D dimensionImageFrame;
		private Texture2D currentDimensionImage;
		private Texture2D leftArrowTex;
		private Texture2D rightArrowTex;
		private Texture2D leftArrowTexHover;
		private Texture2D rightArrowTexHover;
		private Texture2D currentLeftArrowTex;
		private Texture2D currentRightArrowTex;
		private Texture2D exitButtonTexture;

		private UIImage mainPanel;
		private UIImageButton dimensionTransportButton;
		private UIText selectedDimensionName;
		private UIImageButton leftArrow;
		private UIImageButton rightArrow;
		private UIImageButton exitButton;

		private bool shouldDeactivate;

		private float opacity;

		private int selectedDimensionIndex;

		private const float opacityIncrement = 0.075f;

		private const string defaultText = "No dimensions detected!";

		private UserInterface Interface => ModContent.GetInstance<NoxiumMod>().dimensionalInterface;

		private List<Tuple<string, Texture2D, Func<bool>, Action>> dimensions;
		private List<Tuple<string, Texture2D, Action>> availableDimensions;

		public override void OnInitialize()
		{
			mainPanel = new UIImage(mainPanelTexture)
			{
				HAlign = 0.5f,
				VAlign = 0.35f
			};
			mainPanel.Width.Set(mainPanelTexture.Width, 0);
			mainPanel.Height.Set(mainPanelTexture.Height, 0);
			Append(mainPanel);

			dimensionTransportButton = new UIImageButton(defaultDimensionTexture)
			{
				HAlign = 0.5f,
				VAlign = 0.5f
			};
			dimensionTransportButton.Width.Set(188, 0);
			dimensionTransportButton.Height.Set(68, 0);
			dimensionTransportButton.OnClick += delegate
			{
				availableDimensions[selectedDimensionIndex].Item3.Invoke();
			};
			mainPanel.Append(dimensionTransportButton);

			selectedDimensionName = new UIText(defaultText)
			{
				HAlign = 0.5f
			};
			selectedDimensionName.Top.Set(-32, 0);
			mainPanel.Append(selectedDimensionName);

			leftArrow = new UIImageButton(leftArrowTex)
			{
				HAlign = 0.15f,
				VAlign = 0.5f
			};
			leftArrow.Width.Set(leftArrowTex.Width, 0);
			leftArrow.Height.Set(leftArrowTex.Height, 0);
			leftArrow.OnClick += delegate
			{
				selectedDimensionIndex -= 1;
				Main.PlaySound(SoundID.MenuTick);
			};
			mainPanel.Append(leftArrow);

			rightArrow = new UIImageButton(rightArrowTex)
			{
				HAlign = 0.85f,
				VAlign = 0.5f
			};
			rightArrow.Width.Set(rightArrowTex.Width, 0);
			rightArrow.Height.Set(rightArrowTex.Height, 0);
			rightArrow.OnClick += delegate
			{
				selectedDimensionIndex += 1;
				Main.PlaySound(SoundID.MenuTick);
			};
			mainPanel.Append(rightArrow);

			exitButton = new UIImageButton(exitButtonTexture)
			{
				HAlign = 0.5f,
			};
			exitButton.Top.Set(mainPanel.Height.Pixels - 20, 0);
			exitButton.Width.Set(exitButtonTexture.Width, 0);
			exitButton.Height.Set(exitButtonTexture.Height, 0);
			exitButton.OnClick += delegate
			{
				Disable();
			};
			mainPanel.Append(exitButton);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (mainPanel.IsMouseHovering)
				Main.LocalPlayer.mouseInterface = true;

			Fade();

			CheckDimensionsAvailable();

			WrapSelectedIndex();

			currentDimensionImage = availableDimensions[selectedDimensionIndex].Item2;
			selectedDimensionName.SetText(availableDimensions[selectedDimensionIndex].Item1);

			currentLeftArrowTex = leftArrow.IsMouseHovering ? leftArrowTexHover : leftArrowTex;
			currentRightArrowTex = rightArrow.IsMouseHovering ? rightArrowTexHover : rightArrowTex;
		}

		protected override void DrawChildren(SpriteBatch spriteBatch)
		{
			// Drawing is done manually so that the panel's opacity can be a thing - there isn't a way to set opacity of a UIImage. 
			// As such, DO NOT make this call base.DrawChildren or it will break!
			// I'm looking at you, GoodPro.
			// Lord forgive me for what I'm about to do.
			CalculatedStyle mainPanelDimensions = mainPanel.GetDimensions();
			CalculatedStyle currentImageDimensions = dimensionTransportButton.GetDimensions();
			CalculatedStyle textDimensions = selectedDimensionName.GetDimensions();
			CalculatedStyle leftArrowDimensions = leftArrow.GetDimensions();
			CalculatedStyle rightArrowDimensions = rightArrow.GetDimensions();
			CalculatedStyle exitButtonDimensions = exitButton.GetDimensions();
			spriteBatch.Draw(mainPanelTexture, new Vector2(mainPanelDimensions.X, mainPanelDimensions.Y), Color.White * opacity);
			spriteBatch.Draw(currentDimensionImage, new Vector2(currentImageDimensions.X, currentImageDimensions.Y), Color.White * opacity);
			spriteBatch.Draw(dimensionImageFrame, new Vector2(mainPanelDimensions.X, mainPanelDimensions.Y), Color.White * opacity);
			spriteBatch.DrawString(Main.fontMouseText, selectedDimensionName.Text, new Vector2(textDimensions.X, textDimensions.Y), Color.White * opacity);
			spriteBatch.Draw(currentLeftArrowTex, new Vector2(leftArrowDimensions.X, leftArrowDimensions.Y), Color.White * opacity);
			spriteBatch.Draw(currentRightArrowTex, new Vector2(rightArrowDimensions.X, rightArrowDimensions.Y), Color.White * opacity);
			spriteBatch.Draw(exitButtonTexture, new Vector2(exitButtonDimensions.X, exitButtonDimensions.Y), Color.White * opacity);
		}

		public void RegisterDimension(string name, Texture2D dimensionTexture, Func<bool> isAvailable, Action doWhenPressed)
		{
			dimensions.Add(new Tuple<string, Texture2D, Func<bool>, Action>(name, dimensionTexture, isAvailable, doWhenPressed));
		}

		public void LoadUI()
		{
			dimensions = new List<Tuple<string, Texture2D, Func<bool>, Action>>();
			availableDimensions = new List<Tuple<string, Texture2D, Action>>();

			mainPanelTexture = ModContent.GetTexture("NoxiumMod/UI/Dimensions/Textures/MainPanel");
			defaultDimensionTexture = ModContent.GetTexture("NoxiumMod/UI/Dimensions/Textures/DefaultDimensionImage"); // This is the one that shows up when no dimensions have been detected (all the conditions for the other dimensions have failed).
			dimensionImageFrame = ModContent.GetTexture("NoxiumMod/UI/Dimensions/Textures/DimensionImageFrame");
			leftArrowTex = ModContent.GetTexture("NoxiumMod/UI/Dimensions/Textures/LeftArrow");
			leftArrowTexHover = ModContent.GetTexture("NoxiumMod/UI/Dimensions/Textures/LeftArrowHover");
			rightArrowTex = ModContent.GetTexture("NoxiumMod/UI/Dimensions/Textures/RightArrow");
			rightArrowTexHover = ModContent.GetTexture("NoxiumMod/UI/Dimensions/Textures/RightArrowHover");
			exitButtonTexture = ModContent.GetTexture("NoxiumMod/UI/Dimensions/Textures/ExitButton");

			currentDimensionImage = defaultDimensionTexture;
			currentLeftArrowTex = leftArrowTex;
			currentRightArrowTex = rightArrowTex;

			RegisterDimension(defaultText, defaultDimensionTexture, () =>
			{
				for (int i = 0; i < dimensions.Count; i++)
					if (dimensions[i].Item1 != defaultText && dimensions[i].Item3.Invoke())
						return false;

				return true;
			}, delegate { /* Do nothing when clicked */ }); // The 'no dimensions found' dimension is registered here.
		}

		public void Enable()
		{
			shouldDeactivate = false;
			Interface.SetState(this);
			Main.PlaySound(SoundID.MenuOpen);
		}

		public void Disable()
		{
			shouldDeactivate = true;
			Main.PlaySound(SoundID.MenuClose);
		}

		private void Fade()
		{
			if (!shouldDeactivate && opacity < 1)
				opacity += opacityIncrement;
			else if (shouldDeactivate && Interface.CurrentState == this)
			{
				opacity -= opacityIncrement;
				if (opacity < 0)
				{
					opacity = 0;
					Interface.SetState(null);
				}
			}
		}

		private void CheckDimensionsAvailable()
		{
			for (int i = 0; i < dimensions.Count; i++)
			{
				Tuple<string, Texture2D, Action> potentialDimension = new Tuple<string, Texture2D, Action>(dimensions[i].Item1, dimensions[i].Item2, dimensions[i].Item4);
				if (dimensions[i].Item3.Invoke() && !availableDimensions.Contains(potentialDimension))
					availableDimensions.Add(new Tuple<string, Texture2D, Action>(dimensions[i].Item1, dimensions[i].Item2, dimensions[i].Item4));
				else if (!dimensions[i].Item3.Invoke())
					availableDimensions.Remove(potentialDimension);
			}
		}

		private void WrapSelectedIndex()
		{
			if (selectedDimensionIndex > availableDimensions.Count - 1)
				selectedDimensionIndex = 0;	

			if (selectedDimensionIndex < 0)
				selectedDimensionIndex = availableDimensions.Count - 1;
		}
	}
}