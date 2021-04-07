using NoxiumMod.Utilities;
using SubworldLibrary;
using Terraria.ModLoader;

namespace NoxiumMod.Subworlds.EnchantedForest
{
	public class EnchantedForestSurfaceBgStyle : ModSurfaceBgStyle, IOffsetable
	{
		public int XOffset => 0;

		public int YOffset => -1000;

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
