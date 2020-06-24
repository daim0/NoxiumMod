using Microsoft.Xna.Framework;
using NoxiumMod.Systems;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Items.Other
{
    public class TurtleItem : ModItem
    {
        public TurtleInfo TurtleInfo;

        public bool TurtleProgrammed => TurtleInfo.PickaxePower != 0;

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
            item.useTime = 10;
            item.useAnimation = 10;
            item.consumable = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.buyPrice(silver: 10);
        }

		public override bool UseItem(Player player)
		{
            if (TurtleProgrammed)
            {
                Projectile.NewProjectile(player.position, Vector2.Zero, ProjectileType<Projectiles.TurtleProjectile>(), 0, 0f);
                return true;
            }

            return false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
            // PickaxePower is only zero if the turtle hasn't been programmed
            if (!TurtleProgrammed)
                return;

            TooltipLine tooltip = new TooltipLine(mod, "Turtle Info",
                "\n" +
                $"Turtle range width: {TurtleInfo.Width}\n" +
                $"Turtle range height: {TurtleInfo.Height}\n" +
                $"Turtle direction: {(TurtleInfo.Direction == 1 ? "right" : "left")}\n" +
                $"Turtle mining power: {TurtleInfo.PickaxePower}%\n" +
                $"Turtle mining speed: {TurtleInfo.PickaxeSpeed}");

            tooltip.overrideColor = Color.SteelBlue;

            tooltips.Add(tooltip);
		}

		public override TagCompound Save()
		{
            return new TagCompound()
            {
                ["TurtleInfo"] = TurtleInfo 
            };
		}

		public override void Load(TagCompound tag)
		{
            TurtleInfo = tag.Get<TurtleInfo>("TurtleInfo");
		}

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(TurtleInfo.Width);
            writer.Write(TurtleInfo.Height);
            writer.Write(TurtleInfo.Direction);
            writer.Write(TurtleInfo.PickaxePower);
            writer.Write(TurtleInfo.PickaxeSpeed);
        }

        public override void NetRecieve(BinaryReader reader)
        {
            TurtleInfo tInfo = new TurtleInfo // The reason I have to create a new object is because it's a struct, and so we're operating on a copy of the object rather than a reference.
            {
                Width = reader.ReadUInt16(),
                Height = reader.ReadUInt16(),
                Direction = reader.ReadByte(),
                PickaxePower = reader.ReadInt32(),
                PickaxeSpeed = reader.ReadInt32()
            };

            TurtleInfo = tInfo;
        }
    }
}