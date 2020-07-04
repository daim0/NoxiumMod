using NoxiumMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Items.Weapons
{
    class LightningGun : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 40;
            item.noMelee = true;
            item.magic = true;
            item.channel = true; //Channel so that you can hold the weapon [Important]
            item.mana = 5;
            item.rare = ItemRarityID.Pink;
            item.width = 28;
            item.height = 30;
            item.useTime = 20;
            item.UseSound = SoundID.Item13;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shootSpeed = 14f;
            item.useAnimation = 20;
            item.shoot = ProjectileType<LightningGunProj>();
            item.value = Item.sellPrice(silver: 3);
        }
    }
}
