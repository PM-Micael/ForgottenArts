using ForgottenArts.Buffs.AdvancedBuffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenArts.Items.Accessories
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
