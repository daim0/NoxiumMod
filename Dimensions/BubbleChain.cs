using System.Collections.Generic;
using Terraria.DataStructures;

namespace NoxiumMod.Dimensions
{
    public class BubbleChain
    {
        public Point16 start;
        public Point16 end;

        public static List<BubbleChain> bubbleChains;

        public BubbleChain(Point16 start, Point16 end)
        {
            this.start = start;
            this.end = end;
        }
    }
}
