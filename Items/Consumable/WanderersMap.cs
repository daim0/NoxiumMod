using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace NoxiumMod.Items.Consumable
{
    class WanderersMap : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 32;
            item.value = 1000;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.consumable = true;
            item.rare = ItemRarityID.Orange;
        }

        public override bool UseItem(Player player)
        {
            for(int xCoord= 0; xCoord < Main.maxTilesX; xCoord++)
            {
                for (int yCoord = 0; yCoord < Main.maxTilesX; yCoord++)
                {
                    if (WorldGen.InWorld(xCoord, yCoord, 0))
                    {
                        Main.Map.Update(xCoord, yCoord, 255);
                    }
                }
            }
            Main.refreshMap = true;
            return true;
        }
    }
}
