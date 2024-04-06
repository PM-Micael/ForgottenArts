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
    public class WristGuard : ModItem
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
            player.statDefense += 1;
            player.GetDamage<Heavy>() *= 0.05f;
            player.lifeRegen += 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ItemID.Shackle, 1);
            recipe1.AddIngredient(ItemID.BandofRegeneration, 1);
            recipe1.AddIngredient(ItemID.DemoniteBar, 3);
            recipe1.AddTile(TileID.Anvils);
            recipe1.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.Shackle, 1);
            recipe2.AddIngredient(ItemID.BandofRegeneration, 1);
            recipe2.AddIngredient(ItemID.CrimtaneBar, 3);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();
        }
    }
}
