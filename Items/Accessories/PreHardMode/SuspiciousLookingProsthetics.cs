using ForgottenArts.Buffs.AdvancedBuffs;
using ForgottenArts.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
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
            player.statDefense += (playerClass.parryStreak.count * 2);
            player.lifeRegen += (playerClass.parryStreak.count * 1);
        }

        public bool RollForDodgeDamage(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<Buffs.BaseBuffs.CannotTakeDamage>()))
            {
                return false;
            }

            Random r = new Random();
            int num = r.Next(1, 7);
            
            if(num == 1)
            {
                SoundStyle parrySound = new SoundStyle("ForgottenArts/Sounds/Parry-Success")
                {
                    Volume = 0.5f,
                    PitchVariance = 0.2f
                };
                SoundEngine.PlaySound(parrySound);

                return true;
            }

            return false;
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
