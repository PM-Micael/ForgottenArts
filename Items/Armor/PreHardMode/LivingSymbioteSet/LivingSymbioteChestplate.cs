using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ForgottenArts.Buffs;
using ForgottenArts.Buffs.AdvancedBuffs;
using ForgottenArts.Items.Shields;
using static Terraria.NPC;
using ForgottenArts;
using ForgottenArts.Items.Armor.MoltenSet;
using ForgottenArts.Other;
using ForgottenArts.Buffs.BaseBuffs;

namespace ForgottenArts.Items.Armor.PreHardMode.LivingSymbioteSet
{
    [AutoloadEquip(EquipType.Body)]

    public class LivingSymbioteChestplate : ModItem, ISetBonus
    {
        public NPC CurrentEnemy = new NPC();
        public float AdaptiveDefenceStacks = 0f;

        public override void UpdateEquip(Player player)
        {
            player.statDefense += 7;
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();
            playerClass.EndurenceMax += 100;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            //return head.type == ModContent.ItemType<MoltenHelm>() && legs.type == ModContent.ItemType<MoltenLeggings>();
            return true;
        }

        public override void UpdateArmorSet(Player player)
        {
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();

            playerClass.EndurenceRegenRate += 1;
            playerClass.BlockMultiplier += AdaptiveDefenceStacks; // This saves in the object for the duration of the game session
            player.setBonus = "+1 Endurence regen\n" +
                "Blocking the same type of enemy gives you a stack\n" + 
                "Each stack reduces Endurance cost of blocking";

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
                    if(npc.active && !npc.friendly && npc.Distance(player.Center) < blockRadius && npc.type != NPCID.DD2LanePortal)
                    {
                        if (playerClass.IsFacingNPC(npc))
                        {
                            //Add Stacksa
                            if (CurrentEnemy.type != npc.type)
                            {
                                CurrentEnemy.type = npc.type;
                                AdaptiveDefenceStacks = 0f;
                            }
                            else
                            {
                                AdaptiveDefenceStacks += 0.0f;

                                if (AdaptiveDefenceStacks > 3)
                                    AdaptiveDefenceStacks = 3;

                            }
                        }
                    }
                }
            }
        }
    }
}
