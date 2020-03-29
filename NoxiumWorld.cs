using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace NoxiumMod
{
	public class NoxiumWorld : ModWorld
	{
		public static bool oculumOreSpawn;
		public static bool downedAHM;

		public override void Initialize()
		{
			oculumOreSpawn = false;
			downedAHM = false;
		}

		public override TagCompound Save()
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();

			if (oculumOreSpawn)
				list.Add("oculumOreSpawn");

			if (downedAHM)
				list.Add("downedAHM");

			TagCompound tagCompound = new TagCompound
			{
				{ "spawned", list },
				{ "downed", list2 }
			};
			return tagCompound;
		}

		public override void Load(TagCompound tag)
		{
			IList<string> list = tag.GetList<string>("spawned");
			IList<string> list2 = tag.GetList<string>("downed");

			oculumOreSpawn = list.Contains("oculumOreSpawn");

			downedAHM = list2.Contains("downedAHM");
		}

		public override void LoadLegacy(BinaryReader reader)
		{
			int loadVersion = reader.ReadInt32();

			if (loadVersion == 0)
			{
				BitsByte flag = reader.ReadByte();

				oculumOreSpawn = flag[0];
				downedAHM = flag[1];
			}
		}

		public override void NetSend(BinaryWriter writer)
		{
			BitsByte flag = new BitsByte();

			flag[0] = oculumOreSpawn;
			flag[1] = downedAHM;

			writer.Write(flag);
		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flag = reader.ReadByte();

			oculumOreSpawn = flag[0];
			downedAHM = flag[1];
		}

		public override void PreUpdate()
		{
			/*
			while (0 < Main.maxTilesX * Main.maxTilesY * (3E-05f * Main.worldRate))
			{
				int tileX = WorldGen.genRand.Next(10, Main.maxTilesX - 10); //a random x tile
				int tileY = WorldGen.genRand.Next(10, (int)Main.worldSurface - 1); //a psuedo random y tile based on the world surface

				int tileYAbovetileY = tileY - 1 < 10 ? 10 : tileY - 1; //the tile above the psuedo random tile y position

				Tile groundTile = Framing.GetTileSafely(tileX, tileY); //the ground tile that the new tile will be placed on

				//an array of all the tiles that will need to be free
				Tile[] spaceRequired = new Tile[]
				{
					Framing.GetTileSafely(tileX, tileYAbovetileY), //bottom left
					Framing.GetTileSafely(tileX, tileYAbovetileY - 1), //top left
					Framing.GetTileSafely(tileX + 1, tileYAbovetileY), //bottom right
					Framing.GetTileSafely(tileX + 1, tileYAbovetileY - 1) //top right
				};

				//if the ground tile isnt null and its grass
				if (groundTile != null && groundTile.type == TileID.Grass)
				{
					//loop through every required tile
					foreach (Tile tile in spaceRequired)
					{
						//if the tile isnt active; there is space for the new tile
						if (!tile.active())
						{
							if (WorldGen.genRand.NextBool(50)) //Random amount to slow down growing
							{
								//place the 2x2 strawberry multitile
								WorldGen.Place2x2(tileX, tileYAbovetileY - 1, (ushort)ModContent.TileType<StrawberryPlantTile>(), 0);

								//if its still active
								if (tile.active())
								{
									//set the paint color to the ground tiles paint color
									tile.color(groundTile.color());

									//Synce the multitile placement
									if (Main.netMode == NetmodeID.Server)
										NetMessage.SendTileRange(-1, tileX, tileYAbovetileY - 1, 2, 2);
								}
							}
						}
					}
				}
			}*/
		}
	}
}