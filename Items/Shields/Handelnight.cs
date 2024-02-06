using ForgottenArts.Buffs.BaseBuffs;
using ForgottenArts.Items.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenArts.Items.Shields
{
    public class Handelnight : Shield
    {

        public override void SetDefaults()
        {
            Item.defense = 7;

            base.SetDefaults();
        }

        public override List<ShieldBuff> StatusEffects()
        {
            List<ShieldBuff> buffs = new List<ShieldBuff>();
            buffs.Add(new ShieldBuff(BuffID.ShadowFlame, 600));
            return buffs;
        }

        public override int Multipliers(Player player)
        {
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();
            int multiplier = 1;
            int playerDefense = playerClass.playerDefense;
            int result = playerDefense * multiplier;
            return result;
        }

        public override void ParryMeleeSkill(Player player, NPC npc)
        {
            PlayerClass modPlayer = player.GetModPlayer<PlayerClass>();

            foreach(NPC npcNew in Main.npc)
            {
                if(npcNew.Distance(player.Center) < 250)
                {
                    npcNew.AddBuff(BuffID.ShadowFlame, 300);
                }
            }
        }

        public override void ParryRangedSkill(Player player, Projectile proj)
        {
            if (proj.type != ModContent.ProjectileType<ShadowWave>())
            {
                Projectile.NewProjectile(proj.GetSource_FromThis(), proj.position, proj.velocity, ModContent.ProjectileType<ShadowWave>(), proj.damage, proj.knockBack, Main.myPlayer);
                proj.Kill();
            }
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

        public override void PowerUpSkill<T>(Player player,T entity)
        {
            if(entity is Projectile)
            {
                ParryRangedSkill(player, entity as Projectile);
            }
        }
    }
}