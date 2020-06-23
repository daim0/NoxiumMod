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
using Terraria.UI;
using NoxiumMod.UI.Subworld;
using NoxiumMod.Dimensions.Bubbles;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Utilities;

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

        public override UIState loadingUIState => new JoiningUI();

        public static List<BubbleChain> bubbleChains;

        private static List<Texture2D> possibleBubbleTextures;

        private void UpdateBubbles()
        {
            for (int i = 0; i < bubbleChains.Count; i++)
            {
                BubbleChain chain = bubbleChains[i];
                chain.Update(ref i);
            }
        }

        private void DrawBubbles(SpriteBatch spriteBatch)
        {
            foreach (BubbleChain chain in bubbleChains)
            {
                chain.Draw(spriteBatch);
            }
        }

        public static void LoadBubbleTextures()
        {
            possibleBubbleTextures = new List<Texture2D>();

            for (int i = 1; i <= 8; i++)
            {
                possibleBubbleTextures.Add(ModContent.GetTexture($"NoxiumMod/Dimensions/BubbleTextures/Bubble{i}"));
            }
        }

        public PlasmaDesert()
        {
            bubbleChains = new List<BubbleChain>();

            NoxiumWorld.ShouldUpdateBubbles += UpdateBubbles;

            NoxiumWorld.ShouldDrawBubbles += DrawBubbles;

            tasks = new List<GenPass>
            {
                new SubworldGenPass(progress =>
                {
                    progress.Value = 0.1f;
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

                }),
                new SubworldGenPass(progress =>
                {
                    progress.Value = 0.2f;
                    int[] terrainContour = new int[width * height];

                    //Make Random Numbers
                    double rand1 = Main.rand.NextDouble() + 1;
                    double rand2 = Main.rand.NextDouble() + 2;
                    double rand3 = Main.rand.NextDouble() + 3;

                    float peakheight = 15;
                    float flatness = 40;
                    int offset = 370;

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
                                    TileRunner runner = new TileRunner(new Vector2(x, y), new Vector2(0, 0), new Point16(-5, 5), new Point16(-5, 5), 9f, Main.rand.Next(10, 15), (ushort)ModContent.TileType<PlasmaSandstone>(), false, true);
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
                }),
                new SubworldGenPass(progress =>
                {
                    progress.Value = 0.3f;
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
                }),
                new SubworldGenPass(progress =>
                {
                    progress.Value = 0.4f;

                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < Main.worldSurface - 40; y++)
                        {
                            if (Main.rand.Next(320) == 2)
                            {
                                if (Main.tile[x, y].type == (ushort)ModContent.TileType<PlasmaSandstone>())
                                {
                                    TileRunner runner = new TileRunner(new Vector2(x, y), new Vector2(0, 0), new Point16(-5, 5), new Point16(-5, 5), 5f, Main.rand.Next(1, 4), (ushort)ModContent.TileType<PlasmiteOre>(), false, true);
                                    runner.Start();
                                }
                            }
                        }
                        for (int y = height - 200; y < height; y++)
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
                }),
                new SubworldGenPass(progress =>
                {
                    progress.Value = 0.5f;
                    List<Vector2> Positions = new List<Vector2>();
                    for (int x = 20; x < width - 20; x++)
                    {
                        for (int y = 20; y < height / 3; y++)
                        {
                            if (Main.rand.Next(15000) == 1)
                            {
                                int ammount = Main.rand.Next(1, 12);
                                for (int i = 5; i < ammount * 5; i += 5)
                                {
                                    GenCloud.makeOvalFlatTop(new Vector2(x + i, y), Main.rand.Next(20, 25), Main.rand.Next(30, 35), ModContent.TileType<PlasmaSandNoFall>());
                                }
                                TileRunner ae = new TileRunner(new Vector2(x + (ammount / 2 * 5 + 5), y + 30), new Vector2(0, -5), new Point16(0, 0), new Point16(0, 0), Main.rand.NextFloat(15, 20), Main.rand.Next(3, 6), (ushort)ModContent.TileType<PlasmaSandNoFall>(), true, true); ;
                                ae.Start();

                                Positions.Add(new Vector2(x + (ammount / 2 * 5 + 5), y + 30));
                            }
                        }
                    }



                    int identity = 0;

                    // Converts to world coordinates from tile coordinates.
                    for (int i = 0; i < Positions.Count; i++)
                    {
                        Positions[i] *= 16;
                    }

                    foreach (Vector2 pos in Positions)
                    {
                        int distance = 2240; // Maximum distance between islands, in world coordinates.

                        foreach (Vector2 pos2 in Positions)
                        {
                            if (pos != pos2 && Vector2.Distance(pos, pos2) <= distance) // Ensures it does not draw a spline to itself.
                            {
                                for (int i = -1; i <= 1; i++)
                                {
                                    int direction = CalculateDirection(pos, pos2); // Returns either 1 or -1 to dictate the direction of the curve's peak.

                                    int steepnessMult = 640 + (80 * i); // The scalar for the steepness of the curve.

                                    Vector2 perpendicularToMidPoint = Vector2.Normalize(pos - pos2).RotatedBy(MathHelper.PiOver2 * -direction); // Gets a unit vector perpendicular to the vector between the 2 positions.
                                    Vector2 actualMidPoint = Midpoint(pos, pos2) + (perpendicularToMidPoint * steepnessMult); // Creates a control point that sits on the peak of the curve.

                                    BezierCurve curve = new BezierCurve(new Vector2[] { pos, actualMidPoint, pos2 }); // Creates a curve using the three points as controls.

                                    // Calculates the required amount of points by dividing the distance between the start and end by the divisor. Lower divisor = more bubbles.
                                    int points = (int)((pos - pos2).Length() / 60) + Main.rand.Next(-5, 6); // Adds some random variance to the range of points               

                                    if (points <= 0)
                                    {
                                        points = 1;
                                    }

                                    List<Vector2> drawPoints = curve.GetPoints(points); // Collapses the curve into a set of points.

                                    Bubble[] bubbles = new Bubble[points]; // Creates bubbles with random sizes at each of these points.

                                    for (int j = 0; j < points; j++)
                                    {
                                        bubbles[j] = new Bubble(GetBubble(), drawPoints[j]) { identity = identity };

                                        identity++;
                                    }

                                    BubbleChain chain = new BubbleChain(bubbles);

                                    bubbleChains.Add(chain);
                                }
                            }
                        }
                    }




                }),
                new SubworldGenPass(progress =>
                {
                    progress.Value = 0.6f;
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = (height / 2) + 59; y < height; y++)
                        {
                            if (Main.rand.Next(4000) == 1)
                            {
                                WaterTileRunner waterrunner = new WaterTileRunner(new Vector2(x, y), new Vector2(0, -5), new Point16(-5, 5), new Point16(-5, 5), Main.rand.NextFloat(15, 30), Main.rand.Next(3, 6), 0, false, true);
                                waterrunner.Start();

                            }
                        }
                    }
                }),
                new SubworldGenPass(progress =>
                {
                    progress.Value = 0.7f;
                    Vector2 treePos = new Vector2(Main.rand.Next(width), Main.rand.Next((height / 3) + (height / 3), height));
                    TileRunner runner = new TileRunner(treePos, new Vector2(0, -5), new Point16(-5, 5), new Point16(-5, 5), 40f, 3, 0, false, true);
                    runner.Start();
                    for (int i = (int)treePos.X - 17; i < treePos.X + 17; i++)
                    {
                        for (int j = (int)treePos.Y + 5; j < treePos.Y + 17; j++)
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
                }),
                new SubworldGenPass(progress =>
                {
                    progress.Value = 0.8f;
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
                }),
                new SubworldGenPass(progress =>
                {
                    progress.Value = 0.9f;
                    //Underground
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = (height / 2) + 59; y < height; y++)
                        {
                            if (Main.rand.Next(20) == 0)
                            {
                                if (Main.tile[x, y].active() && Main.tileSolid[Main.tile[x, y].type] && !Main.tile[x, y - 1].active())
                                {
                                    WorldGen.PlaceTile(x, y - 1, ModContent.TileType<PlasmaPebble>(), true, false, -1, Main.rand.Next(0, 3));
                                }
                            }
                            if (Main.rand.Next(22) == 0)
                            {
                                if (Main.tile[x, y].active() && Main.tileSolid[Main.tile[x, y].type] && !Main.tile[x, y - 1].active())
                                {
                                    if (Main.tile[x + 1, y].active() && Main.tileSolid[Main.tile[x + 1, y].type] && !Main.tile[x + 1, y - 1].active())
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
                }),
                new SubworldGenPass(progress =>
                {
                    progress.Value = 1;
                    for (int i = 2; i < Main.maxTilesX - 2; i++)
                    {
                        for (int j = 2; j < Main.maxTilesY - 2; j++)
                        {
                            Tile.SmoothSlope(i, j);
                        }
                    }
                })
            };
        }

        public override void Load()
        {
            Main.dayTime = true;
            Main.time = 27000;
            //NoxiumMod.noxiumInstance.ToggleDimensionalUI();
            if(NoxiumMod.noxiumInstance.dimensionalInterface.CurrentState != null)
            {
                NoxiumMod.noxiumInstance.ToggleDimensionalUI();
            }
        }

        private Vector2 Midpoint(Vector2 point1, Vector2 point2)
        {
            float x = (point1.X + point2.X) / 2;
            float y = (point1.Y + point2.Y) / 2;
            return new Vector2(x, y);
        }

        private int CalculateDirection(Vector2 compareTo, Vector2 position)
        {
            if (compareTo.X < position.X) return 1;
            else return -1;
        }

        private Texture2D GetBubble()
        {
            WeightedRandom<Texture2D> wr = new WeightedRandom<Texture2D>(Main.rand);

            wr.Add(possibleBubbleTextures[0], 0.1f);
            wr.Add(possibleBubbleTextures[1], 0.1f);
            wr.Add(possibleBubbleTextures[2], 0.05f);
            wr.Add(possibleBubbleTextures[3], 0.05f);
            wr.Add(possibleBubbleTextures[4], 0.2f);
            wr.Add(possibleBubbleTextures[5], 0.3f);
            wr.Add(possibleBubbleTextures[6], 0.025f);
            wr.Add(possibleBubbleTextures[7], 0.175f);

            return wr.Get();
        }
    }
}