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

namespace ForgottenArts.Items.Armor.HardMode.CursedSet
{
    [AutoloadEquip(EquipType.Body)]

    public class CursedChestplate : ModItem
    {
        public NPC CurrentEnemy = new NPC();

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
            player.statDefense += 19;
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();
            playerClass.EndurenceMax += 300;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<CursedHelm>() && legs.type == ModContent.ItemType<CursedLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();
            playerClass.EndurenceMax += 400;
            playerClass.EndurenceRegenRate += 2;
            player.setBonus = "400 Endurance\n" +
                "x3 Endurance Regeneration\n" +
                "Blocking the same enemy type in multiple succession gives you a stack.\n" +
                "Each stack reduces the endurance cost of blocking.\n" +
                "Blocking a different enemy type resets your stacks";

            SetBonus(player, playerClass);
        }

        /// <summary>
        /// Blocking an enemy type grants the player stacks.
        /// Each stack decreses the amount of Endurence lost upon blocking.
        /// Blocking a different enemy type clears all stacks.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="playerClass"></param>
        public void SetBonus(Player player, PlayerClass playerClass)
        {
            //Runs to see if targets are within blocking distance
            if (playerClass.inBlockStance)
            {
                float blockRadius = playerClass.GetHeldItem().BlockRadius;

                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && npc.Distance(player.Center) < blockRadius && npc.type != NPCID.DD2LanePortal)
                    {
                        if (playerClass.IsFacingNPC(npc))
                        {
                            // Add new enemy type
                            if (CurrentEnemy.type != npc.type)
                            {
                                CurrentEnemy.type = npc.type;
                                playerClass.AdaptiveDefenceStacks = 0f;
                            }
                            else // Add Stacks
                            {
                                playerClass.AdaptiveDefenceStacks += 0.25f;

                                if (playerClass.AdaptiveDefenceStacks > 3)
                                    playerClass.AdaptiveDefenceStacks = 3;

                            }
                        }
                    }
                }
            }
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
