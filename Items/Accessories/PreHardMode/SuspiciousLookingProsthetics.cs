using ForgottenArts.Buffs.AdvancedBuffs;
using ForgottenArts.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenArts.Items.Accessories.PreHardMode
{
    public class SuspiciousLookingProsthetics : ModItem, IDodgeChance
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = 3000;
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();

            playerClass.damageReduction += 0.9f;

            if (playerClass.parryStreak == null)
            {
                playerClass.parryStreak = new ParryStreak();
            }
            else
            {
                ParryStreakBonus(player, playerClass);
            }
        }

        private void ParryStreakBonus(Player player, PlayerClass playerClass)
        {
            switch (playerClass.parryStreak.count)
            {
                case 1:
                    player.statDefense += 2;
                    player.lifeRegen += 1;
                    break;
                case 2:
                    player.statDefense += 4;
                    player.lifeRegen += 2;
                    break;
                case 3:
                    player.statDefense += 6;
                    player.lifeRegen += 3;
                    break;
            }
        }

        public int RollForDodgeDamage()
        {
            Random r = new Random();
            return r.Next(1, 7);
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ModContent.ItemType<ReenforcedArmGuard>(), 1);
            recipe1.AddIngredient(ItemID.BrainOfConfusion, 10);
            recipe1.AddIngredient(ItemID.Bone, 10);
            recipe1.AddTile(TileID.Anvils);
            recipe1.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<ReenforcedArmGuard>(), 1);
            recipe2.AddIngredient(ItemID.WormScarf, 10);
            recipe2.AddIngredient(ItemID.Bone, 10);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();
        }
    }
}
