using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace NoxiumMod.Items.Other
{
    public class TurtleItem : ModItem
    {
        public ushort Width;
        public ushort Height;
        public byte Direction;
        public int PickaxeType;
        public int PickaxePower;
        public int PickaxeSpeed;

        public override bool CloneNewInstances => true;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Turtle");
            Tooltip.SetDefault("Can be programmed to mine entire quarries at a computer");
        }

        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 26;
            item.maxStack = 1;
            item.value = Item.buyPrice(silver: 10);
        }

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
            // PickaxePower is only zero if the turtle hasn't been programmed
            if (PickaxePower == 0)
                return;

            TooltipLine tooltip = new TooltipLine(mod, "Turtle Info",
                "\n" +
                $"Turtle range width: {Width}\n" +
                $"Turtle range height: {Height}\n" +
                $"Turtle direction: {(Direction == 1 ? "right" : "left")}\n" +
                $"Turtle mining power: {PickaxePower}%\n" +
                $"Turtle mining speed: {PickaxeSpeed}");

            tooltip.overrideColor = Color.SteelBlue;

            tooltips.Add(tooltip);
		}

		public override TagCompound Save()
		{
            return new TagCompound()
            {
                { "Width", Width },
                { "Height", Height },
                { "Direction", Direction },
                { "PickaxeType", PickaxeType },
                { "PickaxePower", PickaxePower },
                { "PickaxeSpeed", PickaxeSpeed }
            };
		}

		public override void Load(TagCompound tag)
		{
            Width = tag.Get<ushort>("Width");
            Height = tag.Get<ushort>("Height");
            Direction = tag.GetByte("Direction");
            PickaxeType = tag.GetInt("PickaxeType");
            PickaxePower = tag.GetInt("PickaxePower");
            PickaxeSpeed = tag.GetInt("PickaxeSpeed");
		}
	}
}