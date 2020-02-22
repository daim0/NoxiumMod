using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NoxiumMod.Projectiles.Wooden;

namespace NoxiumMod.Items.Weapons.Wood
{
    class PureWoodBoomerang : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 32;
            item.damage = 7;
            item.useStyle = 1;
            item.useAnimation = 25;
            item.useTime = 30;
            item.shootSpeed = 10f;
            item.knockBack = 5f;
            item.scale = 1f;
            item.rare = 1;
            item.value = Item.sellPrice(silver: 1);
            item.shootSpeed = 6f;

            item.melee = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.autoReuse = false;

            item.UseSound = SoundID.Item1;
            item.shoot = mod.ProjectileType("WoodenBoomer");
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 6);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
