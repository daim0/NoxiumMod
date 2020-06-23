namespace NoxiumMod.Utilities
{
    public static class Extensions
    {
        public static bool IsInBetween(this short value, int min, int max)
        {
            return value >= min && value <= max;
        }
    }
}
