using NoxiumMod.Projectiles.Throwing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Items.Weapons.Throwing
{
    class LeadWaraxe : ModItem
    {
        public override void SetDefaults()
        {
            item.shootSpeed = 12f;
            item.damage = 16;
            item.knockBack = 5f;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useAnimation = 32;
            item.useTime = 32;
            item.width = 50;
            item.height = 50;
            item.maxStack = 1;
            item.rare = ItemRarityID.Green;

            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.thrown = true;

            item.UseSound = SoundID.Item1;
            item.value = Item.sellPrice(gold: 2);
            item.shoot = ProjectileType<LeadWaraxeP>();
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LeadBar, 12);
            recipe.AddIngredient(ItemID.Wood, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
