using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ID;

namespace NoxiumMod.Dimensions.Bubbles
{
    // The reason this is a class and not a struct is because the velocity was never updating.
    // In turn, the reason for this is because foreach iterations operate on copies of the object instead of the actual object.
    // This could probably be rectified by adding for loops instead of foreach ones, but I'm too lazy to try it out.
    // -Oli.
    public class Bubble
    {
        public Bubble(Texture2D texture, Vector2 center)
        {
            this.texture = texture;
            this.center = center;

            oldCenter = center;

            bobOffset = Main.rand.Next(0, 361);

            rotationSpeed = MathHelper.ToRadians(Main.rand.NextFloat(0.005f, 0.25f) * (Main.rand.NextBool() ? -1 : 1));

            velocity = Vector2.Zero;
        }

        public Texture2D texture;

        public Vector2 velocity;

        public Vector2 center;

        public Vector2 oldCenter;

        public int identity;

        public Rectangle Hitbox => new Rectangle((int)center.X - texture.Width / 2, (int)center.Y - texture.Height / 2, texture.Width, texture.Height);

        private readonly int bobOffset;

        private readonly float rotationSpeed;

        private float rotation;

        public void Pop()
        {
            Main.PlaySound(SoundID.Item54, center);

            for (int i = 0; i < 30; i++)
            {
                float angle = MathHelper.ToRadians(12 * i);
                Vector2 vector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                Dust dust = Dust.NewDustPerfect(center + (vector * 40), 245, vector * 8f);
                dust.noGravity = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            rotation += rotationSpeed;

            spriteBatch.Draw(texture, center - Main.screenPosition
            + (Vector2.UnitY * (float)Math.Sin(3 * (Main.GlobalTime + bobOffset)) * 2), null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
        }
    }
}
