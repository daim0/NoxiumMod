using Terraria.ModLoader;
using Terraria.ID;

namespace NoxiumMod
{
    public class NoxiumMod : Mod
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.StrangePlant3, 1);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(ItemID.AcidDye, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.StrangePlant3, 1);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(ItemID.BlueAcidDye, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.StrangePlant4, 1);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(ItemID.RedAcidDye, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.StrangePlant1, 1);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(ItemID.MushroomDye, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.StrangePlant2, 1);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(ItemID.MirageDye, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.StrangePlant1, 1);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(ItemID.NegativeDye, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.StrangePlant1, 1);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(ItemID.PurpleOozeDye, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.StrangePlant2, 1);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(ItemID.ReflectiveDye, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.StrangePlant2, 1);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(ItemID.ReflectiveCopperDye, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.StrangePlant2, 1);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(ItemID.ReflectiveGoldDye, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.StrangePlant1, 1);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(ItemID.ReflectiveObsidianDye, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.StrangePlant2, 1);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(ItemID.ReflectiveMetalDye, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.StrangePlant2, 1);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(ItemID.ReflectiveSilverDye, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.StrangePlant1, 1);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(ItemID.ShadowDye, 1);
            recipe.AddRecipe();

        }
    }
}
