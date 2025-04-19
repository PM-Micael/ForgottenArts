using ForgottenArts.Buffs.BaseBuffs;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ForgottenArts.Buffs;

namespace ForgottenArts.Items.Armor.MoltenSet
{
    [AutoloadEquip(EquipType.Head)]

    public class MoltenHelm : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.DamageType = DamageClass.Melee;
            Item.width = 20;
            Item.height = 50;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.value = 10000;
            Item.rare = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
        }

        public override void UpdateEquip(Player player)
        {
            player.statDefense += 9;
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();
            playerClass.EndurenceMax += 100;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
