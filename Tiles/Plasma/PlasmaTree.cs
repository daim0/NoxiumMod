using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using NoxiumMod.Dusts;

namespace NoxiumMod.Tiles.Plasma
{
    class PlasmaTree : ModTree
    {
        private Mod mod => ModLoader.GetMod("NoxiumMod");

        public override int CreateDust()
        {
            return DustType<PureWoodStaffD>();
        }

        public override int DropWood()
        {
            return 1;
        }

        public override Texture2D GetTexture()
        {
            return mod.GetTexture("Tiles/Plasma/PlasmaTree");
        }

        public override Texture2D GetTopTextures(int i, int j, ref int frame, ref int frameWidth, ref int frameHeight, ref int xOffsetLeft, ref int yOffset)
        {
            return mod.GetTexture("Tiles/Plasma/PlasmaTree_Tops");
        }

        public override Texture2D GetBranchTextures(int i, int j, int trunkOffset, ref int frame)
        {
            return mod.GetTexture("Tiles/Plasma/PlasmaTree_Branches");
        }
    }
}
