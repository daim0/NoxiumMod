using NoxiumMod.Tiles.EnchantedForest;
using NoxiumMod.Utilities;
using SubworldLibrary;
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
			new SubworldGenPass(FrameAllTiles),
		};

		private void PrepareGeneration(GenerationProgress progress)
		{
			progress.Message = "Preparing to generate...";
			Generation.HideUndergroundLayers();
		}

		private void GenerateTerrain(GenerationProgress progress)
		{
			progress.Message = "Generating terrain...";

			int maxDisplacement = 60;
			int terrainBaseLine = (int)(height / 3.5f);
			int minDirtDepth = 21;
			int maxDirtDepth = 32;

			FastNoiseLite noise = new FastNoiseLite(WorldGen._genRandSeed);
			noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
			noise.SetFractalType(FastNoiseLite.FractalType.FBm);
			noise.SetFractalOctaves(4);
			noise.SetFrequency(0.0032f);
			noise.SetFractalLacunarity(2.5f);

			for (int i = 0; i < width; i++)
			{
				progress.Value = i / (float)width;

				float noiseValue = noise.GetNoise(i, 0);
				int displacement = (int)(noiseValue * maxDisplacement) + terrainBaseLine;

				Generation.FillRectangle(i, displacement, 1, height - displacement, TileID.Stone);
				WorldGen.PlaceTile(i, displacement, ModContent.TileType<EnchantedGrassTile>(), true, true);

				int dirtDepth = (int)(noise.GetNoise(i * 3, i * 10) * (maxDirtDepth - minDirtDepth)) + minDirtDepth;
				Generation.FillRectangle(i, displacement + 1, 1, dirtDepth, TileID.Dirt);

				if (i > 1 && i < width - 1 && WorldGen.genRand.NextBool(2))
					WorldGen.GrowTree(i, displacement);
			}
		}

		private void FrameAllTiles(GenerationProgress progress)
		{
			progress.Message = "Framing tiles...";

			for (int j = 0; j < Main.maxTilesY; j++)
			{
				for (int i = 0; i < Main.maxTilesX; i++)
				{
					if (Main.tile[i, j].active())
						WorldGen.SquareTileFrame(i, j);
				}
			}
		}
	}
}