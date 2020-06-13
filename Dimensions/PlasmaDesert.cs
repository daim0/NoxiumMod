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
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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
                List<Vector2> Positions = new List<Vector2>();
                for (int x = 20; x < width - 20; x++)
                {
                    for (int y = 20; y < height / 3; y++)
                    {
                        if (Main.rand.Next(15000) == 1)
                        {
                            
                            int ammount = Main.rand.Next(1, 12);
                            for(int i = 5; i < ammount * 5; i += 5)
                            {
                                GenCloud.makeOvalFlatTop(new Vector2(x + i, y), Main.rand.Next(20, 25), Main.rand.Next(30, 35), ModContent.TileType<PlasmaSandNoFall>());
                            }
                            TileRunner ae = new TileRunner(new Vector2(x + ((ammount / 2) * 5 + 5), y + 30), new Vector2(0, -5), new Point16(0, 0), new Point16(0, 0), Main.rand.NextFloat(15, 20), Main.rand.Next(3, 6), (ushort)ModContent.TileType<PlasmaSandNoFall>(), true, true); ;
                            ae.Start();

                            Positions.Add(new Vector2(x,y));
                        }
                    }
                }
                foreach(Vector2 pos in Positions)
                {
                    int distance = 200;
                    foreach (Vector2 pos2 in Positions)
                    {
                        if(Vector2.Distance(pos,pos2) <= distance)
                        {
                        }
                    }
                }
            }));
            tasks.Add(new SubworldGenPass(progress =>
            {
                progress.Message = "Generating lakes...";
                for (int x = 0; x < width; x++)
                {
                    for (int y = (height / 2) + 59; y < height; y++)
                    {
                        if (Main.rand.Next(4000) == 1)
                        {
                            WaterTileRunner waterrunner = new WaterTileRunner(new Vector2(x, y), new Vector2(0, -5), new Point16(-5, 5), new Point16(-5, 5), Main.rand.NextFloat(15,30), Main.rand.Next(3,6), 0, false, true);
                            waterrunner.Start();

                        }
                    }
                }
             }));
            tasks.Add(new SubworldGenPass(progress =>
            {
                progress.Message = "Generating big tree...";
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
            tasks.Add(new SubworldGenPass(progress =>
            {
                progress.Message = "Generating trees...";
                for (int i = 1; i < width - 1; i++)
                {
                    for (int j = 20; (double)j < Main.worldSurface; j++)
                    {
                        if (Main.rand.Next(5) == 0)
                            WorldGen.GrowTree(i, j);

                    }
                    if (Main.rand.Next(3) == 0)
                        i++;
                    if (Main.rand.Next(4) == 0)
                        i++;
                }
            }));
            tasks.Add(new SubworldGenPass(progress =>
            {
                progress.Message = "Generating decoration...";
                //Underground
                for(int x = 0; x < width; x++)
                {
                    for (int y = (height / 2) + 59; y < height; y++)
                    {
                        if (Main.rand.Next(20) == 0)
                        {
                            if (Main.tile[x, y].active() && Main.tileSolid[Main.tile[x, y].type] && !Main.tile[x, y - 1].active())
                            {
                                WorldGen.PlaceTile(x, y - 1,ModContent.TileType<PlasmaPebble>(), true, false, -1, Main.rand.Next(0, 3));
                            }
                        }
                        if (Main.rand.Next(22) == 0)
                        {
                            if(Main.tile[x, y].active() && Main.tileSolid[Main.tile[x, y].type] && !Main.tile[x, y - 1].active())
                            {
                                if(Main.tile[x + 1, y].active() && Main.tileSolid[Main.tile[x + 1, y].type] && !Main.tile[x + 1, y - 1].active())
                                WorldGen.PlaceTile(x, y - 1, ModContent.TileType<PlasmaPebbleWide>(), true, false, -1, Main.rand.Next(0, 3));
                            }
                        }
                    }
                }
                //Surface
                for (int x = 1; x < width - 1; x++)
                {
                    for (int y = 1; y < (height / 2) + 25; y++)
                    {
                        if (Main.rand.Next(30) == 0)
                        {
                            if (Main.tile[x, y].active() && Main.tileSolid[Main.tile[x, y].type] && !Main.tile[x, y - 1].active())
                            {
                                if (Main.tile[x + 1, y].active() && Main.tileSolid[Main.tile[x + 1, y].type] && !Main.tile[x + 1, y - 1].active())
                                    WorldGen.PlaceTile(x, y - 1, ModContent.TileType<PlasmaGrassDeco>(), true, false, -1, Main.rand.Next(0, 3));
                            }
                        }
                        if (Main.rand.Next(5) == 0)
                        {
                            if (Main.tile[x, y].active() && Main.tileSolid[Main.tile[x, y].type] && !Main.tile[x, y - 1].active())
                            {
                                    WorldGen.PlaceTile(x, y - 1, ModContent.TileType<PlasmaPlant>(), true, false, -1, Main.rand.Next(0, 9));
                            }
                        }
                        if (Main.rand.Next(10) == 0)
                        {
                            if (Main.tile[x, y].active() && Main.tileSolid[Main.tile[x, y].type] && !Main.tile[x, y - 1].active())
                            {
                                WorldGen.PlaceTile(x, y - 1, ModContent.TileType<PlasmaBubbleDeco>(), true, false, -1, Main.rand.Next(0, 4));
                            }
                        }
                    }
                }
            }));
            tasks.Add(new SubworldGenPass(progress =>
            {
                progress.Message = "Smoothing terrain...";
                for (int i = 2; i < Main.maxTilesX - 2; i++)
                {
                    for (int j = 2; j < Main.maxTilesY - 2; j++)
                    {
                        Tile.SmoothSlope(i, j);
                    }
                }
            }));
        }

        public override void Load()
        {
            Main.dayTime = true;
            Main.time = 27000;
            //NoxiumMod.noxiumInstance.ToggleDimensionalUI();
        }
    }
}