using Microsoft.Xna.Framework;
using NoxiumMod.Projectiles;
using NoxiumMod.Projectiles.Turtles;
using NoxiumMod.Systems.Turtles;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace NoxiumMod.Items.Turtles
{
    public abstract class TurtleItem : ModItem
    {
        // stop TurtleItem from autoloading but still allow its children
        public override bool Autoload(ref string name) => GetType() != typeof(TurtleItem);

		public TurtleInfo TurtleInfo;

        public bool TurtleProgrammed => TurtleInfo.PickaxePower != 0;

        public override bool CloneNewInstances => true;

		public abstract int MaxArea
		{
            get;
		}

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Can be programmed to mine entire quarries at a computer\nThis turtle size can only mine up to " + MaxArea + " blocks per run");
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

        public override bool CanUseItem(Player player) => TurtleProgrammed;

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            int projectileIndex = Projectile.NewProjectile(player.BottomLeft, Vector2.Zero, item.shoot, 0, 0f);
            (Main.projectile[projectileIndex].modProjectile as TurtleProjectile).Turtle = TurtleInfo;

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
                $"Turtle direction: {(TurtleInfo.Direction == TurtleDirection.Right ? "right" : "left")}\n" +
                $"Turtle mining power: {TurtleInfo.PickaxePower}%\n" +
                $"Turtle mining speed: {TurtleInfo.PickaxeSpeed}");

            tooltip.overrideColor = Color.SteelBlue;

            tooltips.Add(tooltip);
		}

		public override TagCompound Save()
		{
            return new TagCompound()
            {
                { "TurtleInfo", TurtleInfo }
            };
		}

		public override void Load(TagCompound tag)
		{
            TurtleInfo = tag.Get<TurtleInfo>("TurtleInfo");
		}
	}
}