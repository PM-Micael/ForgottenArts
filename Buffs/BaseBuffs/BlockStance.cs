﻿using ForgottenArts.Buffs.AdvancedBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.NPC;

namespace ForgottenArts.Buffs.BaseBuffs
{
    public class BlockStance : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
        }
        
        public override void Update(Player player, ref int buffIndex)
        {
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();
            var buffs = playerClass.GetHeldItem().StatusEffects();

            float knockbackStrength = 10f;
            float radius = 40f;

            foreach(NPC npc in Main.npc)
            {
                if(npc.active && !npc.friendly && npc.Distance(player.Center) < radius)
                {
                    if (playerClass.IsFacingNPC(npc))
                    {
                        if (buffs != null)
                        {
                            player.AddBuff(BuffID.Ironskin, 120);
                            if (buffs.Count > 0)//Applies status effects
                            {
                                player.AddBuff(BuffID.Regeneration, 120);
                                foreach (var buff in buffs)
                                {
                                    npc.AddBuff(buff.buffID, buff.duration);
                                }
                            }
                        }

                        HitInfo hitInfo = new HitInfo();

                        if(!npc.boss && npc.type != NPCID.EaterofWorldsHead)//Applies knockback if they are not a boss
                        {
                            var direction = npc.Center - player.Center;
                            direction.Normalize();
                            direction *= knockbackStrength;
                            npc.velocity = direction;
                        }
                        hitInfo.Damage = player.statDefense; //AddScaling
                        npc.StrikeNPC(hitInfo);
                    }
                }
            }

            foreach(Projectile proj in Main.projectile)
            {
                if(proj.active && !proj.friendly && proj.Distance(player.Center) < radius)
                {
                    if (playerClass.IsFacingProjectile(proj))
                    {
                        var direction = proj.Center - player.Center;
                        direction.Normalize();
                        direction *= knockbackStrength;
                        proj.velocity = direction;

                        if(playerClass.parryStreak != null)
                        {
                            if (playerClass.parryStreak.count == 3)
                            {
                                proj.friendly = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
