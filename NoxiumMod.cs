using Microsoft.Xna.Framework;
using NoxiumMod.UI;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace NoxiumMod
{
	public class NoxiumMod : Mod
	{
		public static ModHotKey SeedHotkey;

		//shake
		public static float shakeAmount = 0;

		public static int ShakeTimer;
		internal UserInterface AHMUiInterface;
		private AhmBar AhmUI;

		public static void ShakeScreen(float AmountOfShake, int TimeSeconds)
		{
			ShakeTimer = TimeSeconds * Main.frameRate;
			shakeAmount = AmountOfShake;
		}

		public override void Load()
		{
			SeedHotkey = RegisterHotKey("Seed Fruit", "C");

			if (!Main.dedServ)
			{
				AhmUI = new AhmBar();
				AHMUiInterface = new UserInterface();
				AHMUiInterface.SetState(AhmUI);
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
			SeedHotkey = null;
		}

		public override void UpdateUI(GameTime gameTime)
		{
			AHMUiInterface?.Update(gameTime);
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
			}
		}

		public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
		{
			Player player = Main.LocalPlayer;

			//Cant shake main menu, :widepeeposad:
			//We'll see about that - goodpro

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
	}
}