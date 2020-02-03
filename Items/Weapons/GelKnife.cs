using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using NoxiumMod.Projectiles;

namespace NoxiumMod.Items.Weapons
{
    class GelKnife : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pink Gel Throwing Knife");
        }
        public override void SetDefaults()
        {
            item.shootSpeed = 9f;
            item.damage = 14;
            item.knockBack = 2f;
            item.useStyle = 1;
            item.useAnimation = 25;
            item.useTime = 25;
            item.width = 22;
            item.height = 24;
            item.maxStack = 999;
            item.rare = 3;

            item.consumable = true;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.thrown = true;

            item.UseSound = SoundID.Item1;
            item.value = Item.sellPrice(silver: 2);
            item.shoot = ProjectileType<GelKnifeProjectile>();
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileID.WorkBenches);
            recipe.AddIngredient(ItemID.PinkGel, 5);
            recipe.AddIngredient(ItemID.ThrowingKnife, 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
