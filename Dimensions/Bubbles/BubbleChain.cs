using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Globalization;
using Terraria.DataStructures;
using NoxiumMod.Utilities;

namespace NoxiumMod.Dimensions.Bubbles
{
    public struct BubbleChain
    {
        public readonly List<Bubble> bubbles;

        private readonly bool verlet;

        public BubbleChain(Bubble[] bubbles, bool verlet = false)
        {
            this.bubbles = new List<Bubble>(bubbles);
            this.verlet = verlet;
        }

        public void Update(ref int position)
        {
            Rectangle screenRect = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);

            for (int i = 0; i < bubbles.Count; i++)
            {
                Bubble bubble = bubbles[i];

                if (screenRect.Intersects(bubble.Hitbox))
                {
                    for (int j = 0; j < Main.maxProjectiles; j++)
                    {
                        Projectile proj = Main.projectile[j];

                        if (proj.friendly && proj.active && proj.Hitbox.Intersects(bubble.Hitbox))
                        {
                            proj.penetrate--;

                            //proj.modProjectile?.OnTileCollide(proj.velocity); Uncomment to make it trigger tile collisions (might only work with modded)

                            BurstBubble(bubble, ref i, ref position);

                            break;
                        }
                    }

                    for (int j = 0; j < 256; j++)
                    {
                        if (HitboxesGlobalItem.meleeHitbox[j].HasValue)
                        {
                            Rectangle hitbox = HitboxesGlobalItem.meleeHitbox[j].Value;

                            if (bubble.Hitbox.Intersects(hitbox))
                            {
                                BurstBubble(bubble, ref i, ref position);
                            }
                        }
                    }

                    for (int j = 0; j < Main.ActivePlayersCount; j++)
                    {
                        Player player = Main.player[j];

                        if (!player.dead && player.Hitbox.Intersects(bubble.Hitbox))
                        {
                            Main.PlaySound(SoundID.Item56, bubble.center);

                            Vector2 boost = player.DirectionFrom(bubble.center);

                            player.velocity = boost * Math.Max(player.velocity.Length(), 8);

                            bubble.velocity = -boost * Math.Max(player.velocity.Length(), 8);
                        }
                    }
                }
            }

            if (verlet)
            {
                Simulate();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle screenRect = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);

            foreach (Bubble bubble in bubbles)
            {
                if (screenRect.Intersects(bubble.Hitbox))
                {
                    bubble.Draw(spriteBatch);
                }
            }
        } 

        private void BurstBubble(Bubble bubble, ref int i, ref int chainPosition)
        {
            bubble.Pop();

            bubbles.RemoveAt(i);

            i--; // CoLlEcTiOn wAs MoDiFiEd, eNuMeRaTiOn OpErAtIoN mAY NoT eXeCuTe.

            PlasmaDesert.bubbleChains.Remove(this); 

            chainPosition--; // CoLlEcTiOn wAs MoDiFiEd, eNuMeRaTiOn OpErAtIoN mAY NoT eXeCuTe 2 - electric boogaloo.

            if (i < 0)
            {
                return; // I'd rather not get even more IOOB exceptions.
            }

            Bubble[] boubels = new Bubble[i];

            for (int j = 0; j < i; j++) // Makes a new chain of bubbles which consists of the first segment of the array, bisected by the bubble that just popped.
            {
                boubels[j] = bubbles[j];
            }

            BubbleChain new1 = new BubbleChain(boubels, true);

            Bubble[] boubels2 = new Bubble[bubbles.Count - i];

            for (int j = i; j < bubbles.Count; j++) // Makes a second new chain, consisting of the rest of the bubbles.
            {
                boubels2[j - i] = bubbles[j];
            }

            Array.Reverse(boubels2); // Reverses the array so the origin is its last point.

            BubbleChain new2 = new BubbleChain(boubels2, true);

            PlasmaDesert.bubbleChains.Add(new1); // Adds the new chains to the world.
            PlasmaDesert.bubbleChains.Add(new2); 
        }

        // Thanks Andy
        private void Simulate()
        {
            // SIMULATION
            Vector2 forceGravity = new Vector2(0, 1); //GRAVITY

            for (int i = 0; i < bubbles.Count; i++)
            {
                Bubble firstSegment = bubbles[i];

                if (TileCheck(firstSegment))
                {
                    firstSegment.center += firstSegment.velocity;

                    firstSegment.velocity *= 0.95f; // deceleration

                    Vector2 velocity = (firstSegment.center - firstSegment.oldCenter) * 0.35f; // drag

                    firstSegment.oldCenter = firstSegment.center;

                    firstSegment.center += velocity;

                    firstSegment.center += forceGravity;
                }
            }

            int constraints = 1; // Increase to make chains stiffer and create more lag :coolandgood:

            //CONSTRAINTS
            for (int i = 0; i < constraints; i++)
            {
                ApplyConstraint();
            }
        }

        private void ApplyConstraint()
        {
            for (int i = 0; i < bubbles.Count - 1; i++)
            {
                Bubble firstSeg = bubbles[i];
                Bubble secondSeg = bubbles[i + 1];

                int constraintLength = (firstSeg.texture.Width / 2) + (secondSeg.texture.Width / 2);

                float dist = (firstSeg.center - secondSeg.center).Length();

                float error = Math.Abs(dist - constraintLength);

                Vector2 changeDir = Vector2.Zero;

                if (dist > constraintLength)
                {
                    changeDir = Vector2.Normalize(firstSeg.center - secondSeg.center);
                }
                else if (dist < constraintLength)
                {
                    changeDir = Vector2.Normalize(secondSeg.center - firstSeg.center);
                }

                Vector2 changeAmount = changeDir * error;
                if (i != 0)
                {
                    firstSeg.center -= changeAmount * 0.5f;

                    bubbles[i] = firstSeg;

                    secondSeg.center += changeAmount * 0.5f;
                }
                else
                {
                    secondSeg.center += changeAmount;
                }
            }
        }

        private bool TileCheck(Bubble origin)
        {
            Point16 tilePoint16 = origin.center.ToPoint16();

            tilePoint16 = new Point16(tilePoint16.X / 16, tilePoint16.Y / 16);

            if (tilePoint16.X.IsInBetween(0, Main.maxTilesX) && tilePoint16.Y.IsInBetween(0, Main.maxTilesY))
            {
                Tile tile = Main.tile[tilePoint16.X, tilePoint16.Y];

                if (tile == null || !Main.tileSolid[tile.type] || !tile.nactive())
                {
                    return true;
                }
            }
            return false;
        }
    }

    // Thank you modder's toolkit.
    public class HitboxesGlobalItem : GlobalItem
    {
        public static Rectangle?[] meleeHitbox;

        public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            meleeHitbox[player.whoAmI] = hitbox;
        }
    }
}
