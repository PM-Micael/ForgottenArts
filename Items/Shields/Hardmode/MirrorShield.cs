using ForgottenArts.Buffs.BaseBuffs;
using ForgottenArts.Items.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenArts.Items.Shields.Hardmode
{
    public class MirrorShield : Shield
    {
        public override void SetDefaults()
        {
            Item.shoot = 0;
            Item.shootSpeed = 30f;
            Item.defense = 10;
            BlockRadius = 86;

            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BeetleHusk, 10);
            recipe.AddIngredient(ItemID.SunStone, 1);
            //recipe.AddIngredient(ItemID.ShieldofCthulhu, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override int ArmorCheck()
        {
            return 55;
        }

        public override List<ShieldBuff> StatusEffects()
        {
            List<ShieldBuff> buffs = new List<ShieldBuff>();
            buffs.Add(new ShieldBuff(BuffID.Ichor, 600));
            return buffs;
        }

        public override int MaxContactDamageBlock()
        {
            throw new NotImplementedException();
        }

        public override int MaxContactDamageParry()
        {
            throw new NotImplementedException();
        }

        public override int MaxProjectileDamageBlock()
        {
            throw new NotImplementedException();
        }

        public override int MaxProjectileDamageParry()
        {
            throw new NotImplementedException();
        }

        public override void BlockMeleeSkill(Player player, NPC npc)
        {

        }

        public override void BlockNone(Player player)
        {
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();

            if (playerClass.parryStreak.count >= 3)
            {
                Item.shoot = ModContent.ProjectileType<LaserProjectile>();
            }
            else
            {
                Item.shoot = 0;
            }
        }
 
        public override void BlockRangedSkill(Player player, Projectile proj)
        {
            throw new NotImplementedException();
        }

        public override int Multipliers(Player player)
        {
            throw new NotImplementedException();
        }

        public override void ParryMeleeSkill(Player player, NPC npc)
        {
            throw new NotImplementedException();
        }

        public override void ParryRangedSkill(Player player, Projectile proj)
        {
            throw new NotImplementedException();
        }
    }
}
