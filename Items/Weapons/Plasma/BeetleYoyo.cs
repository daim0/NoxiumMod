using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using NoxiumMod.Projectiles.PlasmaStuff;

namespace NoxiumMod.Items.Weapons.Plasma
{
    class BeetleYoyo : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.Yoyo[item.type] = true;
            ItemID.Sets.GamepadExtraRange[item.type] = 16;
            ItemID.Sets.GamepadSmartQuickReach[item.type] = true;
        }

        int Timer;
        public static bool ShootProjectiles = false;
        public override bool AltFunctionUse(Player player)
        {
            if (Timer >= 120)
            {
                Timer = 120;
                return true;
            }
            else return false;
        }
        public override void SetDefaults()
        {
            item.useStyle = 5;
            item.width = 40;
            item.height = 24;
            item.useAnimation = 25;
            item.useTime = 25;
            item.shootSpeed = 16f;
            item.knockBack = 2.5f;
            item.damage = 24;
            item.rare = 0;

            item.melee = true;
            item.channel = true;
            item.noMelee = true;
            item.noUseGraphic = true;

            item.UseSound = SoundID.Item1;
            item.value = Item.sellPrice(silver: 1);
            item.shoot = ProjectileType<BeetleEyePro>();
        }
        private static readonly int[] unwantedPrefixes = new int[] { PrefixID.Terrible, PrefixID.Dull, PrefixID.Shameful, PrefixID.Annoying, PrefixID.Broken, PrefixID.Damaged, PrefixID.Shoddy };
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.noMelee = true;
                item.noUseGraphic = true;
                ShootProjectiles = true;
            }
            else ShootProjectiles = false;
            return base.CanUseItem(player);
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Timer++;
        }
    }
}
