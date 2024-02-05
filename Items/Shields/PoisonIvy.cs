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
            float multiplier = 1f;
            int playerDefense = player.statDefense;
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
            throw new NotImplementedException();
        }

        public override void ParryRangedSkill(Player player, Projectile proj)
        {
            throw new NotImplementedException();
        }
    }
}