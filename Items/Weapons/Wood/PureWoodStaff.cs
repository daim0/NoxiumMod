using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using NoxiumMod.Projectiles.Wooden;

namespace NoxiumMod.Items.Weapons.Wood
{
    class PureWoodStaff : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 10;
            item.magic = true;
            item.mana = 5;
            item.width = 42;
            item.height = 42;
            item.useTime = 37;
            item.useAnimation = 37;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = 10000;
            item.rare = 2;
            item.UseSound = SoundID.Item20;
            item.autoReuse = true;
            item.shoot = ProjectileType<PureWoodStaffP>();
            item.shootSpeed = 6f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 25);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
