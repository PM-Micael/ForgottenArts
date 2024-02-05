using ForgottenArts.Buffs.BaseBuffs;
using Microsoft.Xna.Framework;
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
            Item.defense = 5;

            base.SetDefaults();
        }

        public override float Multipliers()
        {
            return 1f;
        }

        public override void ParryMeleeSkill()
        {
            throw new System.NotImplementedException();
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
    }
}