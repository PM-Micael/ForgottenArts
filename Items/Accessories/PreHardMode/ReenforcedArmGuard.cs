using ForgottenArts.Buffs.AdvancedBuffs;
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
    public class ReenforcedArmGuard : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = 3000;
            Item.rare = 2;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();

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

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<WristGuard>(), 1);
            recipe.AddIngredient(ItemID.HellstoneBar, 4);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
