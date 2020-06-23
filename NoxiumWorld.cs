using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using SubworldLibrary;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace NoxiumMod
{
    public class NoxiumWorld : ModWorld
    {
        public static bool oculumOreSpawn;
        public static bool downedAHM;
        public static bool ahmSpawned;
        public static bool ahmBarShown;
        public static bool desertDwellerSpawned;

        public static int ahmTimer;
        public static int ahmTimerCap = 12 * 60 * 60;
        public static int plasmaSandTiles;

        /* Bools have a default value of false when declared globally, so you dont need this.
        public override void Initialize()
        {
            oculumOreSpawn = false;
            downedAHM = false;
            ahmBarShown = false;
            desertDwellerSpawned = false;
        }
        */

        public static event Action ShouldUpdateBubbles;

        public static event Action<SpriteBatch> ShouldDrawBubbles;

        public override TagCompound Save()
        {
            List<string> list = new List<string>();

            if (oculumOreSpawn)
                list.Add("oculumOreSpawn");
            if (ahmSpawned)
                list.Add("ahmSpawned");
            if (downedAHM)
                list.Add("downedAHM");
            if (ahmBarShown)
                list.Add("ahmBarShown");
            if (desertDwellerSpawned)
                list.Add("DwellerSpawned");

            TagCompound tagCompound = new TagCompound
            {
                { "spawned", list },
                { "ahmSpawned", list },
                { "downed", list },
                { "shown", list },
                { "dspawned", list }
            };
            return tagCompound;
        }

        public override void Load(TagCompound tag)
        {
            // Make the bubbles update just after the world is updated.
            On.Terraria.WorldGen.UpdateWorld += (On.Terraria.WorldGen.orig_UpdateWorld orig) =>
            {
                orig();

                if (Subworld.IsActive<Dimensions.PlasmaDesert>())
                {
                    ShouldUpdateBubbles.Invoke();
                }
            };

            // Make the bubbles draw just after cached NPCs draw.
            On.Terraria.Main.CacheNPCDraws += (On.Terraria.Main.orig_CacheNPCDraws orig, Main self) =>
            {
                if (Subworld.IsActive<Dimensions.PlasmaDesert>())
                {
                    ShouldDrawBubbles.Invoke(Main.spriteBatch);
                }

                orig(self);
            };

            var spawned = tag.GetList<string>("spawned");
            var ahmSpawnedV = tag.GetList<string>("ahmSpawned");
            var downed = tag.GetList<string>("downed");
            var shown = tag.GetList<string>("shown");
            var dspawned = tag.GetList<string>("dspawned");

            oculumOreSpawn = spawned.Contains("oculumOreSpawn");

            downedAHM = downed.Contains("downedAHM");

            ahmSpawned = ahmSpawnedV.Contains("spawned");

            ahmBarShown = shown.Contains("shown");

            desertDwellerSpawned = dspawned.Contains("dspawned");
        }

        public override void LoadLegacy(BinaryReader reader)
        {
            int loadVersion = reader.ReadInt32();

            if (loadVersion == 0)
            {
                BitsByte flag = reader.ReadByte();

                oculumOreSpawn = flag[0];
                downedAHM = flag[1];
                ahmSpawned = flag[2];
                ahmBarShown = flag[3];
                desertDwellerSpawned = flag[4];
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flag = new BitsByte();

            flag[0] = oculumOreSpawn;
            flag[1] = downedAHM;
            flag[2] = ahmSpawned;
            flag[3] = ahmBarShown;
            flag[4] = desertDwellerSpawned;

            writer.Write(flag);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flag = reader.ReadByte();

            oculumOreSpawn = flag[0];
            downedAHM = flag[1];
            ahmSpawned = flag[2];
            ahmBarShown = flag[3];
            desertDwellerSpawned = flag[4];
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
            if (Subworld.IsActive<Dimensions.PlasmaDesert>())
            {
                if (++Liquid.skipCount > 1)
                {
                    Liquid.UpdateLiquid();
                    Liquid.skipCount = 0;
                }
            }
        }

        public override void PostUpdate()
        {
            Player player = Main.LocalPlayer;
            if (Main.hardMode && !ahmSpawned)
            {
                ahmTimer++;
                ahmBarShown = true;
                if (ahmTimer > ahmTimerCap)
                {
                    ahmTimer = ahmTimerCap;
                }
                if (ahmTimer == ahmTimerCap && !ahmSpawned)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("AncientHealingMachine"));
                    Main.PlaySound(SoundID.Roar, player.position, 0);
                }
            }
            else
            {
                ahmBarShown = false;
            }
        }

        public override void ResetNearbyTileEffects()
        {
            plasmaSandTiles = 0;
        }

        public override void TileCountsAvailable(int[] tileCounts)
        {
            plasmaSandTiles = tileCounts[ModContent.TileType<Tiles.Plasma.PlasmaSand>()] + tileCounts[ModContent.TileType<Tiles.Plasma.PlasmaSandNoFall>()];
        }
    }
}