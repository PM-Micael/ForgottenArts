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
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();
            playerClass.EndurenceMax += 300;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<MoltenHelm>() && legs.type == ModContent.ItemType<MoltenLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();
            playerClass.EndurenceRegenRate += 1;
            player.setBonus = " X 2 Enduraence Regeneration" +
                "Burns enemies around you. " +
                "Damage increased for each stack parry streak";

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
                    if (npc.active && !npc.friendly && npc.Distance(player.Center) < radius && npc.type != NPCID.DD2LanePortal)
                    {
                        HitInfo hitInfo = new HitInfo();
                        hitInfo.Knockback = 0;

                        switch (playerClass.parryStreak.count)
                        {
                            case 0:
                                break;
                            case 1:
                                damage = 10;
                                CombatText.NewText(npc.Hitbox, Color.Yellow, damage);
                                break;
                            case 2:
                                damage = 20;
                                CombatText.NewText(npc.Hitbox, Color.Orange, damage);
                                break;
                            case 3:
                                damage = 40;
                                CombatText.NewText(npc.Hitbox, Color.Red, damage);
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
            recipe.AddIngredient(ItemID.HellstoneBar, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
