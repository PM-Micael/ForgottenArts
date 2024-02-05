using ForgottenArts.Buffs.BaseBuffs;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ForgottenArts.Buffs;
using ForgottenArts.Buffs.AdvancedBuffs;
using ForgottenArts.Items.Shields;
using static Terraria.NPC;
using ForgottenArts;

namespace ForgottenArts.Items.Armor.MoltenSet
{
    [AutoloadEquip(EquipType.Body)]

    public class MoltenChestplate : ModItem
    {
        public int countdown = 0;

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 50;
            Item.useTime = 20;
            Item.value = 10000;
            Item.rare = 2;
            Item.UseSound = SoundID.Item1;
        }

        public override void UpdateEquip(Player player)
        {
            player.statDefense += 10;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<MoltenHelm>() && legs.type == ModContent.ItemType<MoltenLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();
            player.setBonus = "Sets nearby enemies on fire with each succesfull Parrys";
            player.statDefense += 5;

            if (playerClass.parryStreak == null)
            {
                playerClass.parryStreak = new ParryStreak();
            }
            else
            {
                BlazingAuroa(player, playerClass);
            }
        }

        public void BlazingAuroa(Player player, PlayerClass playerClass)
        {
            float radius = 500f;
            int damage = 0;

            if (player.HasBuff(ModContent.BuffType<ParryStreak>()) && countdown >= 60)
            {
                countdown = 0;

                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && npc.Distance(player.Center) < radius)
                    {
                        HitInfo hitInfo = new HitInfo();
                        hitInfo.Knockback = 0;

                        switch (playerClass.parryStreak.count)
                        {
                            case 0:
                                break;
                            case 1:
                                damage = 10;
                                break;
                            case 2:
                                damage = 20;
                                break;
                            case 3:
                                damage = 40;
                                break;

                        }

                        npc.life -= damage;

                        if (npc.life <= 0)
                        {
                            npc.life = 0;
                            npc.HitEffect();
                            npc.NPCLoot();
                            npc.checkDead();
                        }
                        else if (damage > 0)
                        {
                            CombatText.NewText(npc.Hitbox, Color.Yellow, damage);
                            npc.HitEffect();
                        }
                    }
                }
            }
            countdown++;
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
