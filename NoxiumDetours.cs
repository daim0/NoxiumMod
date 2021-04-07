using log4net;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using NoxiumMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace NoxiumMod
{
	public static class NoxiumDetours
	{
		private static List<IDisposable> disposedHooks;

		private static ILog Logger => ModContent.GetInstance<NoxiumMod>().Logger;

		public static void ApplyDetours()
		{
			disposedHooks = new List<IDisposable>();

			MonoModHooks.RequestNativeAccess();

			EditDrawCloseBackground();
			EditDrawMiddleBackground();
			EditDrawFarBackground();
		}

		// unload the detours we created since tML won't do it for us
		public static void UnloadDetours()
		{
			foreach (IDisposable hook in disposedHooks)
				hook.Dispose();

			disposedHooks = null;
		}

		private static void EditDrawCloseBackground()
		{
			ILHook hook = new ILHook(typeof(SurfaceBgStyleLoader).GetMethod("DrawCloseBackground"), il =>
			{
				ILCursor cursor = new ILCursor(il);

				if (!cursor.TryGotoNext(
					instr => instr.MatchLdfld<Main>("bgStart"),
					instr => instr.MatchLdsfld<Main>("bgW")))
				{
					Logger.Error("Failed to patch DrawCloseBackground X offset");
					return;
				}

				cursor.Index += 5; // skip to after the X position is calculated
				cursor.Emit(OpCodes.Ldarg_0); // pushes the parameter indicating the background style

				// consume bgStart, the style parameter and return xPos + XOffset if the style implements IOffsetable
				cursor.EmitDelegate<Func<int, int, int>>((xPos, style) =>
				{
					ModSurfaceBgStyle bgStyle = SurfaceBgStyleLoader.GetSurfaceBgStyle(style);

					if (bgStyle is IOffsetableBg offsetable)
						return xPos + offsetable.CloseXOffset;

					return xPos;
				});

				if (!cursor.TryGotoNext(instr => instr.MatchLdfld<Main>("bgTop")))
				{
					Logger.Error("Failed to patch DrawCloseBackground Y offset");
					return;
				}

				cursor.Index++;
				cursor.Emit(OpCodes.Ldarg_0); // pushes the parameter indicating the background style

				cursor.EmitDelegate<Func<int, int, int>>((bgTop, style) =>
				{
					ModSurfaceBgStyle bgStyle = SurfaceBgStyleLoader.GetSurfaceBgStyle(style);

					if (bgStyle is IOffsetableBg offsetable)
						return bgTop + offsetable.CloseYOffset;

					return bgTop;
				});

			});

			disposedHooks.Add(hook);
		}

		private static void EditDrawMiddleBackground()
		{
			ILHook hook = new ILHook(typeof(SurfaceBgStyleLoader).GetMethod("DrawMiddleTexture"), il =>
			{
				ILCursor cursor = new ILCursor(il);

				if (!cursor.TryGotoNext(
					instr => instr.MatchLdfld<Main>("bgStart"),
					instr => instr.MatchLdsfld<Main>("bgW")))
					Logger.Error("Failed to patch DrawMiddleTexture");

				cursor.Index++;
				cursor.Emit(OpCodes.Ldloc_1); // push the current ModSurfaceBgStyle

				// consume bgStart, the current ModSurfaceBgStyle and return xPos + XOffset if the style implements IOffsetable
				cursor.EmitDelegate<Func<int, ModSurfaceBgStyle, int>>((xPos, style) =>
				{
					if (style is IOffsetableBg offsetable)
						return xPos + offsetable.MiddleXOffset;

					return xPos;
				});

				if (!cursor.TryGotoNext(instr => instr.MatchLdfld<Main>("bgTop")))
				{
					Logger.Error("Failed to patch DrawMiddleTexture Y offset");
					return;
				}

				cursor.Index++;
				cursor.Emit(OpCodes.Ldloc_1); // push the current ModSurfaceBgStyle

				cursor.EmitDelegate<Func<int, ModSurfaceBgStyle, int>>((bgTop, style) =>
				{
					if (style is IOffsetableBg offsetable)
						return bgTop + offsetable.MiddleYOffset;

					return bgTop;
				});
			});

			disposedHooks.Add(hook);
		}

		private static void EditDrawFarBackground()
		{
			ILHook hook = new ILHook(typeof(SurfaceBgStyleLoader).GetMethod("DrawFarTexture"), il =>
			{
				ILCursor cursor = new ILCursor(il);

				if (!cursor.TryGotoNext(
					instr => instr.MatchLdfld<Main>("bgStart"),
					instr => instr.MatchLdsfld<Main>("bgW")))
					Logger.Error("Failed to patch DrawFarTexture");

				cursor.Index++;
				cursor.Emit(OpCodes.Ldloc_1); // push the current ModSurfaceBgStyle

				// consume bgStart, the current ModSurfaceBgStyle and return xPos + XOffset if the style implements IOffsetable
				cursor.EmitDelegate<Func<int, ModSurfaceBgStyle, int>>((xPos, style) =>
				{
					if (style is IOffsetableBg offsetable)
						return xPos + offsetable.FarXOffset;

					return xPos;
				});

				if (!cursor.TryGotoNext(instr => instr.MatchLdfld<Main>("bgTop")))
				{
					Logger.Error("Failed to patch DrawFarTexture Y offset");
					return;
				}

				cursor.Index++;
				cursor.Emit(OpCodes.Ldloc_1); // push the current ModSurfaceBgStyle

				cursor.EmitDelegate<Func<int, ModSurfaceBgStyle, int>>((bgTop, style) =>
				{
					if (style is IOffsetableBg offsetable)
						return bgTop + offsetable.FarYOffset;

					return bgTop;
				});
			});

			disposedHooks.Add(hook);
		}
	}
}