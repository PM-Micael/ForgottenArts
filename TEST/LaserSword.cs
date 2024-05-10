using ForgottenArts.Buffs.BaseBuffs;
using ForgottenArts.Items.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenArts.TEST
{
    internal class LaserSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 50; // Adjust damage as necessary
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.value = 10000;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.SaucerDeathray;
            Item.shootSpeed = 20f;
            
        }
    }
}
