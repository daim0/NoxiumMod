using SubworldLibrary;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using NoxiumMod.Tiles.Plasma;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using NoxiumMod.Utilities;
using Terraria.ObjectData;

namespace NoxiumMod.Dimensions
{
    public class PlasmaDesert : Subworld
    {
        //https://stackoverflow.com/questions/9697079/generating-terrain-in-c-sharp-using-perlin-noise
        public override int width => 1500;
        public override int height => 700;

        public override ModWorld modWorld => ModContent.GetInstance<NoxiumWorld>();

        public override bool saveSubworld => false;
        public override bool disablePlayerSaving => true;
        public override bool saveModData => false;



        public override List<GenPass> tasks { get; }

        public PlasmaDesert()
        {
            tasks = new List<GenPass>();
            tasks.Add(new SubworldGenPass(progress =>
        {
            progress.Message = "Generating basic terrain..."; //Sets the text above the worldgen progress bar
            Main.worldSurface = Main.maxTilesY - 42; //Hides the underground layer just out of bounds
             Main.rockLayer = Main.maxTilesY; //Hides the cavern layer way out of bounds

            int[] terrainContour = new int[width * height];

            //Make Random Numbers
            double rand1 = Main.rand.NextDouble() + 1;
            double rand2 = Main.rand.NextDouble() + 2;
            double rand3 = Main.rand.NextDouble() + 3;

            //Variables, Play with these for unique results!
            float peakheight = 10;
            float flatness = 65;
            int offset = 350; 


            //Generate basic terrain sine
            for (int x = 0; x < width; x++)
            {

               double height = peakheight / rand1 * Math.Sin((float)x / flatness * rand1 + rand1);
               height += peakheight / rand2 * Math.Sin((float)x / flatness * rand2 + rand2);
               height += peakheight / rand3 * Math.Sin((float)x / flatness * rand3 + rand3);

               height += offset;

               terrainContour[x] = (int)height;
            }
            for (int x = 0; x < width; x++)
            {
               for (int y = 0; y < height; y++)
               {

                   if (y > terrainContour[x])
                   {
                        Main.tile[x, y].active(true);
                        Main.tile[x, y].type = (ushort)ModContent.TileType<PlasmaSandNoFall>();
                        //Main.tile[x, y + 2].wall = (ushort)ModContent.WallType<PlasmaSandstoneWall>();
                        Main.tile[x, Math.Min(height - 1, y + 8)].wall = (ushort)ModContent.WallType<PlasmaSandstoneWall>();


                    }
                   else
                       Main.tile[x, y].active(false);
                }
            }

        }));
            tasks.Add(new SubworldGenPass(progress =>
            {
            progress.Message = "Placing sandstone...";
            int[] terrainContour = new int[width * height];

            //Make Random Numbers
            double rand1 = Main.rand.NextDouble() + 1;
            double rand2 = Main.rand.NextDouble() + 2;
            double rand3 = Main.rand.NextDouble() + 3;

            float peakheight = 15;
            float flatness = 40;
            int offset = 370; ;


            //Generate basic terrain sine
            for (int x = 0; x < width; x++)
            {

                double height = peakheight / rand1 * Math.Sin((float)x / flatness * rand1 + rand1);
                height += peakheight / rand2 * Math.Sin((float)x / flatness * rand2 + rand2);
                height += peakheight / rand3 * Math.Sin((float)x / flatness * rand3 + rand3);

                height += offset;

                terrainContour[x] = (int)height;
            }
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (Main.rand.Next(320) == 2)
                        {
                            if (Main.tile[x, y].type == (ushort)ModContent.TileType<PlasmaSandNoFall>())
                            {
                                TileRunner runner = new TileRunner(new Vector2(x, y), new Vector2(0, 0), new Point16(-5, 5), new Point16(-5, 5), 9f, Main.rand.Next(10,15), (ushort)ModContent.TileType<PlasmaSandstone>(), false, true);
                                runner.Start();
                            }
                        }
                        if (y > terrainContour[x])
                        {
                            if (Main.tile[x, y].type == (ushort)ModContent.TileType<PlasmaSandNoFall>())
                            {
                                Main.tile[x, y].active(true);
                                Main.tile[x, y].type = (ushort)ModContent.TileType<PlasmaSandstone>();
                            }
                        }
                    }
                }
            }));
            tasks.Add(new SubworldGenPass(progress =>
            {
                progress.Message = "Generating caves...";
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height - 20; y++)
                    {
                        if (Main.rand.Next(1, 3800) == 2)
                        {
                            //cave
                            TileRunner runner = new TileRunner(new Vector2(x, y), new Vector2(0, 5), new Point16(-20, 20), new Point16(-20, 20), 15f, Main.rand.Next(200, 400), 0, false, true);
                            runner.Start();
                        }
                    }
                }
            }));
            tasks.Add(new SubworldGenPass(progress =>
            {
                progress.Message = "Generating trees...";
                for (int i = 1; i < width - 1; i++)
                {
                    for(int j = 20; (double)j < Main.worldSurface; j++)
                    {
                        if(Main.rand.Next(6) == 0)
                            WorldGen.GrowTree(i,j);

                    }
                    if (Main.rand.Next(3) == 0)
                        i++;
                    if (Main.rand.Next(4) == 0)
                        i++;
                }
            }));
            tasks.Add(new SubworldGenPass(progress =>
            {
                progress.Message = "Generating ores...";

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < Main.worldSurface - 40; y++)
                    {
                        if(Main.rand.Next(320) == 2)
                        {
                            if (Main.tile[x, y].type == (ushort)ModContent.TileType<PlasmaSandstone>())
                            {
                                TileRunner runner = new TileRunner(new Vector2(x, y), new Vector2(0, 0), new Point16(-5, 5), new Point16(-5, 5), 5f, Main.rand.Next(1,4), (ushort)ModContent.TileType<PlasmiteOre>(), false, true);
                                runner.Start();
                            }
                        }
                    }
                    for (int y = height -200; y < height; y++)
                    {
                        if (Main.rand.Next(1000) == 2)
                        {
                            if (Main.tile[x, y].type == (ushort)ModContent.TileType<PlasmaSandstone>() || Main.tile[x, y].type == (ushort)ModContent.TileType<PlasmiteOre>())
                            {
                                TileRunner runner = new TileRunner(new Vector2(x, y), new Vector2(0, 0), new Point16(-5, 5), new Point16(-5, 5), 5f, Main.rand.Next(1, 2), (ushort)ModContent.TileType<SealedMoonlessStone>(), false, true);
                                runner.Start();
                            }
                        }
                    }
                }
            }));
            tasks.Add(new SubworldGenPass(progress =>
            {
                progress.Message = "Generating floating islands...";
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height / 3; y++)
                    {
                        if (Main.rand.Next(17000) == 1)
                        {
                            int size = 50;
                            for (int i = -size / 2; i < size / 2; ++i)
                            {
                                int repY = (size / 2) - (Math.Abs(i));
                                int offset = repY / 5;
                                repY += WorldGen.genRand.Next(4);
                                for (int j = -offset; j < repY; ++j)
                                {
                                    WorldGen.PlaceTile(x + i, y + j, (ushort)ModContent.TileType<PlasmaSandNoFall>());
                                }
                            }
                        }
                        if (Main.rand.Next(17000) == 1)
                        {
                            TileRunner runner = new TileRunner(new Vector2(x, y), new Vector2(0, 0), new Point16(-5, 5), new Point16(-5, 5), 7f, 1, (ushort)ModContent.TileType<SealedMoonlessStone>(), false, true);
                            runner.Start();
                        }
                    }
                }

            }));
            tasks.Add(new SubworldGenPass(progress =>
            {
                Vector2 treePos = new Vector2(Main.rand.Next(width), Main.rand.Next((height / 3) + (height / 3), height));
                TileRunner runner = new TileRunner(treePos, new Vector2(0, -5), new Point16(-5, 5), new Point16(-5, 5), 40f, 3, 0, false, true);
                runner.Start();
                for(int i = (int)treePos.X - 17; i < treePos.X + 17; i++)
                {
                    for(int j = (int)treePos.Y + 5; j < treePos.Y + 17; j++)
                    {
                        Main.tile[i, j].active(true);
                        Main.tile[i, j].type = (ushort)ModContent.TileType<PlasmaSandstone>();
                    }
                }
                WorldGen.PlaceTile((int)treePos.X, (int)treePos.Y - 15, TileID.Torches, false, false, -1, 4);
                WorldGen.PlaceTile((int)treePos.X + 12, (int)treePos.Y - 5, TileID.Torches, false, false, -1, 4);
                WorldGen.PlaceTile((int)treePos.X - 12, (int)treePos.Y - 5, TileID.Torches, false, false, -1, 4);
                TileRunner runner2 = new TileRunner(treePos + new Vector2(0, 10), new Vector2(0, -5), new Point16(-5, 5), new Point16(-5, 5), 10f, 3, (ushort)ModContent.TileType<PlasmaSand>(), false, true);
                runner2.Start();
                TileUtils.PlaceMultitile(new Point16((int)treePos.X - 5, (int)treePos.Y - 11), ModContent.TileType<HugeTree>());
                //WorldGen.PlaceChest((int)treePos.X - 10, (int)treePos.Y + 4, (ushort)ModContent.TileType<PlasmaDesertChest>());
            }));
        }

        public override void Load()
        {
            Main.dayTime = true;
            Main.time = 27000;
            NoxiumMod.noxiumInstance.ToggleDimensionalUI();
        }
    }
}