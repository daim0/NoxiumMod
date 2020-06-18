using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoxiumMod.UI;
using NoxiumMod.UI.Dimensions;
using NoxiumMod.Dimensions;
using System;
using System.Collections;
using System.Collections.Generic;
using SubworldLibrary;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using NoxiumMod.UI.Subworld;

namespace NoxiumMod
{
	public class NoxiumMod : Mod
	{
		public static NoxiumMod noxiumInstance;

		public static ModHotKey SeedHotkey;

		private AhmBar AhmUI;
		internal UserInterface AHMUiInterface;

		private DimensionalUI dimensionalUI;
		internal UserInterface dimensionalInterface;

		public static float shakeAmount = 0;
		public static int ShakeTimer;

		public override void Load()
		{
			noxiumInstance = GetInstance<NoxiumMod>();

			SeedHotkey = RegisterHotKey("Seed Fruit", "C");

			if (!Main.dedServ)
			{
				AhmUI = new AhmBar();
				AHMUiInterface = new UserInterface();
				AHMUiInterface.SetState(AhmUI);

				dimensionalUI = new DimensionalUI();
				dimensionalUI.LoadUI();
				dimensionalUI.Activate();

				JoiningUI.LoadLoadingSymbol();

                /* Examples:

				dimensionalUI.RegisterDimension("The Cum Zone", ModContent.GetTexture("Terraria/Item_2"), () => Main.dayTime, () => Main.NewText("Welcome to the cum zone")); 
				// Appears as 'The Cum Zone', shows a dirt block texture, only appears in the day, when clicked will say 'Welcome to the cum zone'.

				dimensionalUI.RegisterDimension("Hell", ModContent.GetTexture("Terraria/Item_1"), () => !Main.dayTime, () => Main.NewText("Welcome to hell"));
				// Appears as 'Hell', shows an iron pickaxe texture, only appears at night, when clicked will say 'Welcome to hell'.

				Call("AddDimension", "The Cum Zone", ModContent.GetTexture("Terraria/Item_2"), (Func<bool>)(() => Main.dayTime), (Action)(() => Main.NewText("Welcome to the cum zone")));
				// Example of a mod.Call to add a dimension.

				*/
                // Gaming
                dimensionalUI.RegisterDimension("Plasma Desert", ModContent.GetTexture("NoxiumMod/PlasmaDesert"), () => true, () => Subworld.Enter<PlasmaDesert>());
                dimensionalUI.RegisterDimension("The Cum Zone", ModContent.GetTexture("Terraria/Item_2"), () => true, () => Main.NewText("Welcome to the cum zone"));

                dimensionalInterface = new UserInterface();
			}

			Mod yabhb = ModLoader.GetMod("FKBossHealthBar");

			if (yabhb != null)
			{
				yabhb.Call("hbStart");
				yabhb.Call("hbSetTexture", GetTexture("UI/AhmHealthStart"), GetTexture("UI/AhmHealthMid"), GetTexture("UI/AhmHealthEnd"), GetTexture("UI/AhmHealthFill"));
				yabhb.Call("hbFinishSingle", NPCType("AncientHealingMachine"));
			}
		}

		public override void Unload()
		{
			Type[] types = noxiumInstance.Code.GetTypes(); // Automatically sets all static fields to null, so you don't have to worry about it, ever.
			foreach (Type type in types)
			{
				foreach (FieldInfo item in from field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
										   where !field.FieldType.IsValueType && !field.IsLiteral
										   select field)
				{
					if (item.FieldType.IsGenericType)
					{
						if (item.FieldType.GetGenericTypeDefinition() == typeof(List<>))
						{
							((IList)item.GetValue(null))?.Clear();
						}
						else if (item.FieldType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
						{
							((IDictionary)item.GetValue(null))?.Clear();
						}
					}
					else
					{
						item.SetValue(null, null);
					}
				}
			}
		}

		public override object Call(params object[] args)
		{
			Array.Resize(ref args, 5);
			if (args[0] is string callType)
			{
				if (callType == "AddDimension" && args[1] is string name && args[2] is Texture2D texture && args[3] is Func<bool> func && args[4] is Action action && !Main.dedServ)
				{
					dimensionalUI.RegisterDimension(name, texture, func, action);
				}
			}
			return base.Call(args);
		}

		public override void UpdateUI(GameTime gameTime)
		{
			AHMUiInterface?.Update(gameTime);
			dimensionalInterface?.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

			if (mouseTextIndex != -1)
			{
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("NoxiumMod: Vanilla Stack Reproduction", delegate
				{
					Main.LocalPlayer.GetModPlayer<NoxiumPlayer>().SetSeedStackDelays();
					return true;
				}, InterfaceScaleType.UI));
			}

			int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));

			if (resourceBarIndex != -1)
			{
				layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer("NoxiumMod: AHM Bar", delegate
				{
					AHMUiInterface.Draw(Main.spriteBatch, new GameTime());
					return true;
				}, InterfaceScaleType.UI));

				layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer("NoxiumMod: Dimension Selector", delegate
				{
					dimensionalInterface.Draw(Main.spriteBatch, new GameTime());
					return true;
				}, InterfaceScaleType.UI));
			}
		}

		public static void ShakeScreen(float AmountOfShake, int TimeSeconds)
		{
			ShakeTimer = TimeSeconds * Main.frameRate;
			shakeAmount = AmountOfShake;
		}

		public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
		{
			Player player = Main.LocalPlayer;

			//Cant shake main menu, :widepeeposad:
			//We'll see about that - goodpro
			//We certainly will - Oli

			if (!Main.gameMenu)
			{
				if (shakeAmount != 0 && ShakeTimer != 0)
				{
					ShakeTimer--;
					Vector2 Shakey = new Vector2(player.Center.X + Main.rand.NextFloat(shakeAmount), player.Center.Y + Main.rand.NextFloat(shakeAmount)) - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
					Main.screenPosition = Shakey;
				}
			}
		}

		public override void AddRecipes()
		{
			//ew
            // :( -Daim
			ModRecipe recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.StrangePlant3, 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.DyeVat);
			recipe.SetResult(ItemID.AcidDye, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.StrangePlant3, 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.DyeVat);
			recipe.SetResult(ItemID.BlueAcidDye, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.StrangePlant4, 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.DyeVat);
			recipe.SetResult(ItemID.RedAcidDye, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.StrangePlant1, 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.DyeVat);
			recipe.SetResult(ItemID.MushroomDye, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.StrangePlant2, 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.DyeVat);
			recipe.SetResult(ItemID.MirageDye, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.StrangePlant1, 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.DyeVat);
			recipe.SetResult(ItemID.NegativeDye, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.StrangePlant1, 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.DyeVat);
			recipe.SetResult(ItemID.PurpleOozeDye, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.StrangePlant2, 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.DyeVat);
			recipe.SetResult(ItemID.ReflectiveDye, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.StrangePlant2, 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.DyeVat);
			recipe.SetResult(ItemID.ReflectiveCopperDye, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.StrangePlant2, 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.DyeVat);
			recipe.SetResult(ItemID.ReflectiveGoldDye, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.StrangePlant1, 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.DyeVat);
			recipe.SetResult(ItemID.ReflectiveObsidianDye, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.StrangePlant2, 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.DyeVat);
			recipe.SetResult(ItemID.ReflectiveMetalDye, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.StrangePlant2, 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.DyeVat);
			recipe.SetResult(ItemID.ReflectiveSilverDye, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.StrangePlant1, 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.DyeVat);
			recipe.SetResult(ItemID.ShadowDye, 1);
			recipe.AddRecipe();
		}

		public void ToggleDimensionalUI()
		{
			if (dimensionalInterface.CurrentState == null)
			{
				dimensionalUI.Enable();
			}
			else
			{
				dimensionalUI.Disable();
			}
		}
	}
}