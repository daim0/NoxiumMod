using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.NPCs.Boss.AncientHealingMachine
{
    public class AncientHealingBossBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }
		
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 36;
            item.height = 32;
            item.rare = 9;
            item.expert = true;
           
        }

		
        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            player.TryGettingDevArmor();
            int choice = Main.rand.Next(3);
            if (choice == 0)
            {
                
                player.QuickSpawnItem(1225, Main.rand.Next(25, 35));
                player.QuickSpawnItem(75, Main.rand.Next(10, 15));
				player.QuickSpawnItem(549, Main.rand.Next(10, 25));

				
            }
            if (choice == 1)
            {
                
                player.QuickSpawnItem(1225, Main.rand.Next(25, 35));
                player.QuickSpawnItem(75, Main.rand.Next(10, 15));
				player.QuickSpawnItem(548, Main.rand.Next(10, 25));

            }
			if (choice == 2)
            {
                
                player.QuickSpawnItem(1225, Main.rand.Next(25, 35));
                player.QuickSpawnItem(75, Main.rand.Next(10, 15));
				player.QuickSpawnItem(547, Main.rand.Next(10, 25));

			}
        }
		public override int BossBagNPC => NPCType<NPCs.Boss.AncientHealingMachine.AncientHealingCrystalLeft>(); 
    }
}