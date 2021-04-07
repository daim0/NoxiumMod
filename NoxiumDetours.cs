using log4net;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using NoxiumMod.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace NoxiumMod
{
	public static class NoxiumDetours
	{
		private static List<IDisposable> disposedHooks = new List<IDisposable>();

		private static ILog Logger => ModContent.GetInstance<NoxiumMod>().Logger;

		public static void ApplyDetours()
		{
			MonoModHooks.RequestNativeAccess();

			Type surfaceBgType = typeof(SurfaceBgStyleLoader);
			EditDrawCloseBackground();
			EditDrawFartherBackground(surfaceBgType.GetMethod("DrawMiddleTexture"));
			EditDrawFartherBackground(surfaceBgType.GetMethod("DrawFarTexture"));
		}

		// unload the detours we created since tML won't do it for us
		public static void UnloadDetours()
		{
			foreach (IDisposable hook in disposedHooks)
				hook.Dispose();
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

					if (bgStyle is IOffsetable offsetable)
						return xPos + offsetable.XOffset;

					return xPos;
				});

				if (!cursor.TryGotoNext(instr => instr.MatchLdfld<Main>("bgTop")))
				{
					Logger.Error("Failed to patch DrawCloseBackground Y offset");
					return;
				}

				cursor.Index++;
				cursor.Emit(OpCodes.Ldarg_0);  // pushes the parameter indicating the background style

				cursor.EmitDelegate<Func<int, int, int>>((bgTop, style) =>
				{
					ModSurfaceBgStyle bgStyle = SurfaceBgStyleLoader.GetSurfaceBgStyle(style);

					if (bgStyle is IOffsetable offsetable)
						return bgTop + offsetable.YOffset;

					return bgTop;
				});

			});

			disposedHooks.Add(hook);
		}

		// Note: this is only meant to be used with SurfaceBgStyleLoader.DrawMiddleTexture and SurfaceBgStyleLoader.DrawFarTexture, nothing else
		private static void EditDrawFartherBackground(MethodInfo info)
		{
			ILHook hook = new ILHook(info, il =>
			{
				ILCursor cursor = new ILCursor(il);

				if (!cursor.TryGotoNext(
					instr => instr.MatchLdfld<Main>("bgStart"),
					instr => instr.MatchLdsfld<Main>("bgW")))
					Logger.Error("Failed to patch " + info.Name);

				cursor.Index++;
				cursor.Emit(OpCodes.Ldloc_1); // push the current ModSurfaceBgStyle

				// consume bgStart, the current ModSurfaceBgStyle and return xPos + XOffset if the style implements IOffsetable
				cursor.EmitDelegate<Func<int, ModSurfaceBgStyle, int>>((xPos, style) =>
				{
					if (style is IOffsetable offsetable)
						return xPos + offsetable.XOffset;

					return xPos;
				});

				if (!cursor.TryGotoNext(instr => instr.MatchLdfld<Main>("bgTop")))
				{
					Logger.Error("Failed to patch DrawCloseBackground Y offset");
					return;
				}

				cursor.Index++;
				cursor.Emit(OpCodes.Ldloc_1);  // push the current ModSurfaceBgStyle

				cursor.EmitDelegate<Func<int, ModSurfaceBgStyle, int>>((bgTop, style) =>
				{
					if (style is IOffsetable offsetable)
						return bgTop + offsetable.YOffset;

					return bgTop;
				});
			});

			disposedHooks.Add(hook);
		}
	}
}