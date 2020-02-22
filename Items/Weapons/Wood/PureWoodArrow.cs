using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using NoxiumMod.Projectiles.Wooden;

namespace NoxiumMod.Items.Weapons.Wood
{
    class PureWoodArrow : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 4;
            item.ranged = true;
            item.width = 14;
            item.height = 24;
            item.maxStack = 999;
            item.consumable = true;             
            item.knockBack = 1.5f;
            item.value = 10;
            item.rare = 1;
            item.shoot = ProjectileType<PureWoodArrowP>();   
            item.shootSpeed = 3f;                  
            item.ammo = AmmoID.Arrow;              
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 13);
            recipe.AddRecipe();
        }
    }
}
