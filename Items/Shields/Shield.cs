using ForgottenArts.Buffs.BaseBuffs;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenArts.Items.Shields
{
    public abstract class Shield : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.DamageType = ModContent.GetInstance<Heavy>();
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.useStyle = 2;
            Item.knockBack = 6;
            Item.value = 10000;
            Item.rare = 2;
            Item.UseSound = null;
            Item.autoReuse = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();

            if (player.HasBuff(ModContent.BuffType<ShieldCooldown>()))
            {
                return false;
            }
            else if(Main.mouseLeft && !playerClass.inParryStance) //Block
            {
                Item.useAnimation = 1;
                Item.useTime = 1;
                player.AddBuff(ModContent.BuffType<Buffs.BaseBuffs.BlockStance>(), 1);
            }
            else if(Main.mouseRight && !playerClass.inBlockStance)//Parry
            {
                Item.useAnimation = 10;
                Item.useTime = 10;
                player.AddBuff(ModContent.BuffType<Buffs.BaseBuffs.ParryStance>(), 20);
            }

            return true;
        }

        /*
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Vector2 direction = Main.MouseWorld - player.position;
            direction.Normalize();
        }
        */

        //My methods*******************************************************************************************************

        public abstract List<ShieldBuffs> StatusEffects();

        public abstract float Multipliers();

        public abstract void ParryMeleeSkill();
    }
}