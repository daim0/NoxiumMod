using NoxiumMod.Utilities;
using SubworldLibrary;
using Terraria.ModLoader;

namespace NoxiumMod.Subworlds.EnchantedForest
{
	public class EnchantedForestSurfaceBgStyle : ModSurfaceBgStyle, IOffsetableBg
	{
		public int CloseXOffset => 0;

		public int CloseYOffset => -950;

		public int MiddleXOffset => 0;

		public int MiddleYOffset => -500;

		public int FarXOffset => 0;

		public int FarYOffset => -500;

		public override bool ChooseBgStyle() => Subworld.IsActive<EnchantedForestSubworld>();

		public override void ModifyFarFades(float[] fades, float transitionSpeed)
		{
			for (int i = 0; i < fades.Length; i++)
			{
				if (i == Slot)
				{
					fades[i] += transitionSpeed;
					if (fades[i] > 1f)
						fades[i] = 1f;
				}
				else
				{
					fades[i] -= transitionSpeed;
					if (fades[i] < 0f)
						fades[i] = 0f;
				}
			}
		}

		public override int ChooseFarTexture()
			=> mod.GetBackgroundSlot("Backgrounds/EnchantedForest/EnchantedForestSurfaceFar");

		public override int ChooseMiddleTexture()
			=> mod.GetBackgroundSlot("Backgrounds/EnchantedForest/EnchantedForestSurfaceMiddle");

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
			=> mod.GetBackgroundSlot("Backgrounds/EnchantedForest/EnchantedForestSurfaceClose");
	}
}
