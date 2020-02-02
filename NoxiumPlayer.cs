using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod
{
    class NoxiumPlayer : ModPlayer
    {
        public bool fireMinion;

        public override void ResetEffects()
        {
            this.fireMinion = false;
        }
    }
}
