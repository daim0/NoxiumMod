
using Terraria.ModLoader;
using NoxiumMod.Tiles.Plasma;

namespace NoxiumMod.Items.Placeable.Plasma
{
    class PlasmaSandItem : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = ModContent.TileType<PlasmaSand>();
            //item.ammo = AmmoID.Sand; Using this Sand in the Sandgun would require PickAmmo code and changes to ExampleSandProjectile or a new ModProjectile.
        }
    }
}
