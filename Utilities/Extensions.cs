using Microsoft.Xna.Framework;

namespace NoxiumMod.Utilities
{
    public static class Extensions
    {
        public static bool IsInBetween(this short value, int min, int max)
        {
            return value >= min && value <= max;
        }

        public static Vector2 GetPos(this Rectangle rect)
        {
            return new Vector2(rect.X, rect.Y);
        }
    }
}
