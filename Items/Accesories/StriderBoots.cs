using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Items.Accesories
{
    [AutoloadEquip(EquipType.Shoes)]
    class StriderBoots : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 24;
            item.accessory = true;
        }

        private bool doubleJumped = false;
        private bool releaseJump = false;
        private const int maxSpeed = 26;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxRunSpeed += 5f;
            player.jumpSpeedBoost += 1f;
            player.accRunSpeed = 9f;

            if (!player.controlJump && player.velocity.Y != 0)
            {
                releaseJump = true;
            }
            if (player.controlJump && player.velocity.Y != 0 && releaseJump && !doubleJumped)
            {
                doubleJumped = true;
                if (player.controlLeft) { Jump(-1); } else if(player.controlRight) { Jump(1); }
            }
            if (player.velocity.Y == 0)
            {
                releaseJump = false;
                doubleJumped = false;
            }
            if(player.velocity.X >= maxSpeed / 2 || player.velocity.X <= -(maxSpeed/2))
            {
                if (Main.rand.NextBool(5))
                {
                    Vector2 offset = player.velocity.RotatedBy(1.57f) * ((float)Main.rand.NextDouble() - 0.5f) * player.width;
                    Dust dust = Main.dust[Dust.NewDust(player.Center + offset - Vector2.One * 4f, 8, 8, 31, 0.0f, 0.0f, 100, new Color(), 1.5f)];
                    dust.velocity *= 0.5f;
                    dust.velocity.Y = -Math.Abs(dust.velocity.Y);
                    dust = Main.dust[Dust.NewDust(player.Center, 8, 8, 31, 0.0f, 0.0f, 100, new Color(), 1.5f)];
                    dust.velocity = dust.velocity * 0.5f;
                    dust.velocity.Y = -Math.Abs(dust.velocity.Y);
                    for (int i = 0; i < 2; ++i)
                    {
                        float num1 = player.velocity.ToRotation() + (Main.rand.Next(2) == 1 ? -1.0f : 1.0f) * 1.57f;
                        float num2 = (float)(Main.rand.NextDouble() * 0.8f + 1.0f);
                        Vector2 dustVel = new Vector2((float)Math.Cos(num1) * num2, (float)Math.Sin(num1) * num2);
                        Dust dust2 = Main.dust[Dust.NewDust(player.BottomLeft, 0, 0, 226, dustVel.X + player.velocity.X, dustVel.Y)];
                        dust2.noGravity = true;
                        dust2.scale = 1.2f;
                        dust2.fadeIn = 0f;
                        dust2.noGravity = true;
                        dust2.scale = 0.88f;
                        dust2.color = Color.Cyan;
                    }
                }
            }
            void Jump(int dir)
            {
                float speed = player.velocity.X * dir;
                if(speed > 0 && speed < maxSpeed)
                {
                    player.velocity.X += (((maxSpeed * dir) - player.velocity.X) * 0.66f);
                }
                else if (speed < 0)
                {
                    player.velocity.X = (6 * dir);
                }
                for (int i = 0; i < 2; ++i)
                {
                    float num1 = player.velocity.ToRotation() + (Main.rand.Next(2) == 1 ? -1.0f : 1.0f) * 1.57f;
                    float num2 = (float)(Main.rand.NextDouble() * 0.8f + 1.0f);
                    Vector2 dustVel = new Vector2((float)Math.Cos(num1) * num2, (float)Math.Sin(num1) * num2);
                    Dust dust = Main.dust[Dust.NewDust(player.Center, 0, 0, 226, dustVel.X, dustVel.Y)];
                    dust.noGravity = true;
                    dust.scale = 1.2f;
                    dust.fadeIn = 0f;
                    dust.noGravity = true;
                    dust.scale = 0.88f;
                    dust.color = Color.Cyan;
                }

                if (Main.rand.NextBool(5))
                {
                    Vector2 offset = player.velocity.RotatedBy(1.57f) * ((float)Main.rand.NextDouble() - 0.5f) * player.width;
                    Dust dust = Main.dust[Dust.NewDust(player.Center + offset - Vector2.One * 4f, 8, 8, 31, 0.0f, 0.0f, 100, new Color(), 1.5f)];
                    dust.velocity *= 0.5f;
                    dust.velocity.Y = -Math.Abs(dust.velocity.Y);
                    dust = Main.dust[Dust.NewDust(player.Center, 8, 8, 31, 0.0f, 0.0f, 100, new Color(), 1.5f)];
                    dust.velocity = dust.velocity * 0.5f;
                    dust.velocity.Y = -Math.Abs(dust.velocity.Y);
                }
                for (int i = 0; i < 36; i++)
                {
                    float angle = MathHelper.ToRadians(10 * i);
                    Vector2 vector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    Dust dust = Dust.NewDustPerfect(player.Center + (vector * 10), 230, vector * 5f);
                    Dust dust2 = Dust.NewDustPerfect(player.Center + (vector * 10), 197, vector * 5f);
                    dust.noGravity = true;
                    dust2.noGravity = true;
                }
                Main.PlaySound(SoundID.DD2_LightningBugZap, player.Center);
            }
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Items/Accesories/StriderBoots_Glow");
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}
