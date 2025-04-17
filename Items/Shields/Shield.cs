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
        public float BlockRadius = 40f;
        public abstract int ArmorCheck();
        public abstract int MaxContactDamageBlock();
        public abstract int MaxContactDamageParry();
        public abstract int MaxProjectileDamageBlock();
        public abstract int MaxProjectileDamageParry();

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
            BlockRadius = 40f;
        }

        public override void HoldItem(Player player)
        {
            player.statDefense += Item.defense;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();

            if(playerClass.playerDefense >= ArmorCheck()) //Armor check
            {

            }
            if (player.HasBuff(ModContent.BuffType<ShieldCooldown>()) || player.HasBuff(ModContent.BuffType<ParryStance>()))
            {
                return false;
            }
            else if (Main.mouseLeft && !playerClass.inParryStance) //Block
            {
                Item.useAnimation = 1;
                Item.useTime = 1;
                player.AddBuff(ModContent.BuffType<Buffs.BaseBuffs.BlockStance>(), 1);
            }
            else if (Main.mouseRight && !playerClass.inBlockStance)//Parry
            {

                if (playerClass.playerDefense >= 40) // More defence = higher parry window
                {
                    Item.useAnimation = 35;
                    Item.useTime = 35;
                    player.AddBuff(ModContent.BuffType<Buffs.BaseBuffs.ParryStance>(), 35);
                }
                else
                {
                    Item.useAnimation = 20;
                    Item.useTime = 20;
                    player.AddBuff(ModContent.BuffType<Buffs.BaseBuffs.ParryStance>(), 20);
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (Main.mouseX + Main.screenPosition.X > player.Center.X)
            {
                player.direction = 1;
                //player.itemRotation = (float)System.Math.Atan2(Main.mouseY + Main.screenPosition.Y - player.Center.Y, Main.mouseX + Main.screenPosition.X - player.Center.X);
            }
            else
            {
                player.direction = -1;
                //player.itemRotation = (float)System.Math.Atan2(Main.mouseY + Main.screenPosition.Y - player.Center.Y, Main.mouseX + Main.screenPosition.X - player.Center.X) + MathHelper.Pi;
            }

            /*
            if(player.direction == -1)
            {
                //player.itemRotation += MathHelper.ToRadians(180f);
            }
            */
        }

        /*
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Vector2 direction = Main.MouseWorld - player.position;
            direction.Normalize();
        }
        */


        //My methods*******************************************************************************************************

        public abstract List<ShieldBuff> StatusEffects();

        public abstract int Multipliers(Player player);

        public abstract void ParryMeleeSkill(Player player, NPC npc);

        public abstract void ParryRangedSkill(Player player, Projectile proj);

        public abstract void BlockMeleeSkill(Player player, NPC npc);

        public abstract void BlockRangedSkill(Player player, Projectile proj);

        public abstract void BlockNone(Player player);
    }
}