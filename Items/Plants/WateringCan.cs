using NoxiumMod.Systems.Plants;
using NoxiumMod.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Plants
{
	//TODO smart cursor
	//TODO cool animal corssing animation
	public class WateringCan : ModItem
	{
		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useAnimation = 17;
			item.useTime = 17;
			item.width = 30;
			item.height = 30;
			item.rare = ItemRarityID.Pink;
			item.consumable = false;
			//item.noMelee = true;
			item.value = Item.sellPrice(silver: 5);
		}

		//TODO cant figure out watering
		public override bool UseItem(Player player)
		{
			Point16 tilePos = (Main.MouseWorld / 16).ToPoint16();// new Point16(16);
			PlantEntity entity = TileUtils.GetTileEntity<PlantEntity>(tilePos);

			if (entity != null)
				entity.hasBeenWatered = true;
			return true;
		}

		public override void RightClick(Player player)
		{
			Point16 tilePos = Main.MouseWorld.ToPoint16();
			PlantEntity entity = TileUtils.GetTileEntity<PlantEntity>(tilePos);
			
			if (entity != null)
				entity.hasBeenWatered = true;
		}
	}
}