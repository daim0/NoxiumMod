using Microsoft.Xna.Framework;
using Terraria;

namespace NoxiumMod.Utilities
{
	public static class ClickUtils
	{
		public static bool ClickedOnRectangle(Rectangle rectangle, ClickType clickType)
		{
			if (rectangle.Contains(new Point(Main.mouseX, Main.mouseY)))
			{
				if (clickType == ClickType.Left)
				{
					if (Main.mouseLeft && Main.mouseLeftRelease)
						return true;
				}
				else if (clickType == ClickType.Right)
				{
					if (Main.mouseRight && Main.mouseRightRelease)
						return true;
				}
				else if (clickType == ClickType.Middle)
				{
					if (Main.mouseMiddle && Main.mouseMiddleRelease)
						return true;
				}
			}

			return false;
		}
	}

	public enum ClickType
	{
		Left,
		Right,
		Middle
	}
}
