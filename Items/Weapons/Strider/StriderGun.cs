using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Items.Weapons.Strider
{
    class StriderGun : ModItem
    {
        public override void SetDefaults()
        {
            item.autoReuse = true;
            item.useStyle = 5;
            item.useAnimation = 14;
            item.useTime = 14;
            item.width = 44;
            item.height = 30;
            item.mana = 3;
            item.UseSound = SoundID.Item12;
            item.knockBack = 0.75f;
            item.damage = 19;
            item.shootSpeed = 15f;
            item.noMelee = true;
            item.rare = 1;
            item.magic = true;
            item.value = 20000;
            item.shoot = ModContent.ProjectileType<Projectiles.StriderGunP>();
            if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Weapons/Strider/StriderGun_Glow");
                item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -2;

            }
        }
        private int ShotCount;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            ShotCount++;
            Main.PlaySound(SoundID.DD2_LightningAuraZap);
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
            if(ShotCount == 10)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<Projectiles.BigBoyLaser>(), 40, 0f, Main.myPlayer);
                for (int i = 0; i < 36; i++)
                {
                    float angle = MathHelper.ToRadians(10 * i);
                    Vector2 vector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    Dust dust = Dust.NewDustPerfect(player.Center + (vector * 10), 230, vector * 3f);
                    dust.noGravity = true;
                }
                Main.PlaySound(SoundID.DD2_LightningBugZap, player.Center);
                ShotCount = 0;
                return false;
            }
            return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Items/Weapons/Strider/StriderGun_Glow");
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
