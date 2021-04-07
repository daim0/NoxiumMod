using Microsoft.Xna.Framework;
using NoxiumMod.UI.Dimensions;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace NoxiumMod
{
	// TODO: Move all UI stuff elsewhere
	public class NoxiumMod : Mod
	{
		private DimensionSelectionUI dimensionSelectionUI;
		internal UserInterface dimensionalInterface;

		public override void Load()
		{
			NoxiumDetours.ApplyDetours();

			if (!Main.dedServ)
			{
				dimensionSelectionUI = new DimensionSelectionUI();
				dimensionSelectionUI.LoadUI();
				dimensionSelectionUI.Activate();

				dimensionalInterface = new UserInterface();
			}
		}

		public override void Unload()
		{
			NoxiumDetours.UnloadDetours();
		}

		public override void UpdateUI(GameTime gameTime)
		{
			dimensionalInterface?.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
			if (resourceBarIndex != -1)
			{
				layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer("NoxiumMod: Dimension Selector", delegate
				{
					dimensionalInterface.Draw(Main.spriteBatch, new GameTime());
					return true;
				}, InterfaceScaleType.UI));
			}
		}

		public void ToggleDimensionalUI()
		{
			if (dimensionalInterface.CurrentState == null)
				dimensionSelectionUI.Enable();
			else
				dimensionSelectionUI.Disable();
		}
	}
}