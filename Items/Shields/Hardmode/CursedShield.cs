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
    public class CursedShield : Shield
    {
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

        public override void SetDefaults()
        {
            Item.defense = 9;
            BlockRadius = 86;

            base.SetDefaults();
        }

        public override List<ShieldBuff> StatusEffects()
        {
            List<ShieldBuff> buffs = new List<ShieldBuff>();
            buffs.Add(new ShieldBuff(BuffID.CursedInferno, 600));
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
                    npcNew.AddBuff(BuffID.CursedInferno, 300);
                }
            }
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
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();
            float speedTiles = Vector2.Distance(Vector2.Zero, new Vector2(player.velocity.X, 0)) * 60 / 16;
            float speed = speedTiles * (20.45f / 15f);
            playerClass.Mph = speed;
            if (player.velocity.X != 0 && playerClass.Mph >= 20f)
            {
                playerClass.runningDuration += 1;

                if (playerClass.runningDuration >= 120)
                {
                    playerClass.runningDuration = 120;

                    player.moveSpeed *= 3f;
                    BlockRadius = 86f;
                }
            }
            else
            {
                playerClass.runningDuration = 0;
                BlockRadius = 86f;
            }
        }
    }
}