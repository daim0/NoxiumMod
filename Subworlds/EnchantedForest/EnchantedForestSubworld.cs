using NoxiumMod.Tiles.EnchantedForest;
using NoxiumMod.Utilities;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace NoxiumMod.Subworlds.EnchantedForest
{
	public class EnchantedForestSubworld : Subworld
	{
		public override int width => Generation.SmallWorldWidth / 3;

		public override int height => Generation.SmallWorldHeight / 3;

		// TODO: change this to true when subworld is finished
		public override bool saveSubworld => false;

		public override List<GenPass> tasks => new List<GenPass>()
		{
			new SubworldGenPass(PrepareGeneration),
			new SubworldGenPass(GenerateTerrain),
			new SubworldGenPass(FrameTiles),
		};

		private void PrepareGeneration(GenerationProgress progress)
		{
			progress.Message = "Preparing to generate...";
			Generation.HideUndergroundLayers();
		}

		private void GenerateTerrain(GenerationProgress progress)
		{
			progress.Message = "Generating terrain...";

			int maxPrimaryDisplacement = 60;
			int maxMountainDisplacement = 220;
			int terrainBaseLine = (int)(height / 3.5f);
			int minDirtDepth = 21;
			int maxDirtDepth = 32;

			FastNoiseLite primaryNoise = new FastNoiseLite(WorldGen._genRandSeed);
			primaryNoise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
			primaryNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
			primaryNoise.SetFractalOctaves(3);
			primaryNoise.SetFrequency(0.0032f);
			primaryNoise.SetFractalLacunarity(2.25f);

			FastNoiseLite mountainNoise = new FastNoiseLite(WorldGen._genRandSeed);
			mountainNoise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
			mountainNoise.SetFrequency(0.0005f);

			for (int i = 0; i < width; i++)
			{
				progress.Value = i / (float)width;

				float primaryNoiseValue = primaryNoise.GetNoise(i, 0);
				int primaryDisplacement = (int)(primaryNoiseValue * maxPrimaryDisplacement) + terrainBaseLine;

				float mountainNoiseValue = mountainNoise.GetNoise(i, 0);
				int mountainDisplacement = (int)(Math.Abs(mountainNoiseValue) * maxMountainDisplacement);

				int finalDisplacement = primaryDisplacement + mountainDisplacement;

				Generation.FillRectangle(i, finalDisplacement, 1, height - finalDisplacement, TileID.Stone);
				WorldGen.PlaceTile(i, finalDisplacement, ModContent.TileType<EnchantedGrassTile>(), true, true);

				int dirtDepth = (int)(primaryNoise.GetNoise(i * 3, i * 10) * (maxDirtDepth - minDirtDepth)) + minDirtDepth;
				Generation.FillRectangle(i, finalDisplacement + 1, 1, dirtDepth, TileID.Dirt);

				if (i > 1 && i < width - 1)
				{
					// TODO: make the grass connect nicer

					if (WorldGen.genRand.NextBool(2))
						WorldGen.GrowTree(i, finalDisplacement);
				}
			}
		}

		private void FrameTiles(GenerationProgress progress)
		{
			progress.Message = "Framing tiles...";

			for (int j = (int)(height * 0.7f); j < height; j++)
			{
				for (int i = 0; i < Main.maxTilesX; i++)
				{
					progress.Value = j / (float)Main.maxTilesY;

					if (Main.tile[i, j].active())
						WorldGen.SquareTileFrame(i, j);
				}
			}
		}
	}
}