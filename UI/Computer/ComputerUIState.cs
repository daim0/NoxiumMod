using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;

namespace NoxiumMod.UI.Computer
{
	public class ComputerUIState : UIState
	{
		private ComputerUI computer;

		private Vector2 offset;
		private bool dragging;

		public override void OnInitialize()
		{
			computer = new ComputerUI();
			computer.HAlign = 0.5f;
			computer.VAlign = 0.3f;
			Append(computer);
		}

		// all the following code is jut ripped out of examplemod so the turtle screen is draggable
		public override void MouseDown(UIMouseEvent evt)
		{
			base.MouseDown(evt);

			if (computer.ContainsPoint(Main.MouseScreen))
				DragStart(evt);
		}

		public override void MouseUp(UIMouseEvent evt)
		{
			base.MouseUp(evt);

			if (computer.ContainsPoint(Main.MouseScreen))
				DragEnd(evt);
		}

		private void DragStart(UIMouseEvent evt)
		{
			offset = new Vector2(evt.MousePosition.X - computer.Left.Pixels, evt.MousePosition.Y - computer.Top.Pixels);
			dragging = true;
		}

		private void DragEnd(UIMouseEvent evt)
		{
			Vector2 end = evt.MousePosition;
			dragging = false;

			computer.Left.Set(end.X - offset.X, 0f);
			computer.Top.Set(end.Y - offset.Y, 0f);

			computer.Recalculate();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (computer.ContainsPoint(Main.MouseScreen))
				Main.LocalPlayer.mouseInterface = true;
			

			if (dragging)
			{
				computer.Left.Set(Main.mouseX - offset.X, 0f);
				computer.Top.Set(Main.mouseY - offset.Y, 0f);
				computer.Recalculate();
			}

			var parentSpace = GetDimensions().ToRectangle();

			if (!GetDimensions().ToRectangle().Intersects(parentSpace))
			{
				computer.Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
				computer.Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
				computer.Recalculate();
			}
		}
	}
}
