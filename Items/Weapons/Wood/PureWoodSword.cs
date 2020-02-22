using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace NoxiumMod.Items.Weapons.Wood
{
    class PureWoodSword : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 4;
            item.melee = true;
            item.width = 38;
            item.height = 38;
            item.useTime = 23;
            item.useAnimation = 23;
            item.useStyle = 1;
            item.knockBack = 6;
            item.value = Item.buyPrice(gold: 1);
            item.rare = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
