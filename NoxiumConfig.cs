using System;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace NoxiumMod
{
    public class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [Label("Loading Screen Night Mode")]
        [Tooltip("Changes the base colors of the loading screen.")]
        public bool NightMode { get; set; }
    }
}
