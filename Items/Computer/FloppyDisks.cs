using Microsoft.Xna.Framework;
using NoxiumMod.UI.Computer.Games;
using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Computer
{
    public abstract class FloppyDisk : ModItem
    {
        public virtual string GameName => "";

        public virtual Type GameType => null;

        public override string Texture => "NoxiumMod/Items/Computer/" + GameName + "GameDisk";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Floppy Disk: " + GameName);
            Tooltip.SetDefault("Insert into a computer to play!");
        }

        public override void SetDefaults()
        {
            item.rare = ItemRarityID.Green;
            item.Size = new Vector2(20);
        }
    }

    public class Snake : FloppyDisk
    {
        public override string GameName => "Snake";

        public override Type GameType => typeof(SnakeGame);
    }

    public class Pong : FloppyDisk
    {
        public override string GameName => "Pong";

        public override Type GameType => typeof(PongGame);
    }
}
