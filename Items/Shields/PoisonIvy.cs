using ForgottenArts.Buffs.BaseBuffs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenArts.Items.Shields
{
    public class PoisonIvy : Shield
    {

        public override void SetDefaults()
        {
            Item.defense = 2;

            base.SetDefaults();
        }

        public override int Multipliers(Player player)
        {
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();
            float multiplier = 1f;
            int playerDefense = playerClass.playerDefense;
            int result = (int)Math.Round(playerDefense * multiplier);
            return result;
        }


        public override List<ShieldBuff> StatusEffects()
        {
            List<ShieldBuff> buffs = new List<ShieldBuff>();
            buffs.Add(new ShieldBuff(BuffID.Poisoned, 600));
            return buffs;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 25);
            recipe.AddIngredient(ItemID.Stinger, 12);
            recipe.AddIngredient(ItemID.JungleSpores, 8);
            recipe.AddIngredient(ItemID.Vine, 3);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override void ParryMeleeSkill(Player player, NPC npc)
        {
            
        }

        public override void ParryRangedSkill(Player player, Projectile proj)
        {
            proj.Kill();
        }

        public override void BlockMeleeSkill(Player player, NPC npc)
        {

        }

        public override void BlockRangedSkill(Player player, Projectile proj)
        {
            proj.Kill();
        }

        public override void BlockNone(Player player)
        {
            
        }

        public override int ArmorCheck()
        {
            return 18;
        }

        public override int MaxContactDamageBlock()
        {
            return 75;
        }

        public override int MaxProjectileDamageBlock()
        {
            throw new NotImplementedException();
        }

        public override int MaxContactDamageParry()
        {
            throw new NotImplementedException();
        }

        public override int MaxProjectileDamageParry()
        {
            throw new NotImplementedException();
        }
    }
}