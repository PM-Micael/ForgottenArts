using ForgottenArts.Buffs.AdvancedBuffs;
using System.Numerics;
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

            float knockbackStrength = playerClass.PlayerSpeed > 0? 10f : 1f;
            float radius = playerClass.GetHeldItem().BlockRadius;

            playerClass.GetHeldItem().BlockNone(player);

            foreach(NPC npc in Main.npc)
            {
                if(npc.active && !npc.friendly && npc.Distance(player.Center) < radius)
                {
                    if (playerClass.IsFacingNPC(npc))
                    {
                        if (buffs != null)
                        {
                            if (buffs.Count > 0)//Applies status effects
                            {
                                foreach (var buff in buffs)
                                {
                                    npc.AddBuff(buff.buffID, buff.duration);
                                }
                            }
                        }

                        HitInfo hitInfo = new HitInfo();

                        if (!npc.boss && npc.type != NPCID.EaterofWorldsHead && npc.damage <= playerClass.playerDefense * 5)//Applies knockback if they are not a boss
                        {
                            var direction = npc.Center - player.Center;
                            direction.Normalize();
                            direction *= knockbackStrength;
                            npc.velocity = direction;
                            hitInfo.Damage = playerClass.playerDefense; //AddScaling
                        }
                        else
                        {
                            hitInfo.Damage = 1;
                        }

                        if(playerClass.PlayerSpeed > 0)
                        {
                            npc.StrikeNPC(hitInfo);
                        }
                    }
                }
            }

            foreach(Projectile proj in Main.projectile) //Not neccesary finalized in terms of damage ased on armor level / shield type
            {
                if (proj.active && !proj.friendly && proj.Distance(player.Center) < radius)
                {
                    if (playerClass.IsFacingProjectile(proj))
                    {
                        if(proj.damage <= playerClass.playerDefense * 5)
                        {
                            playerClass.GetHeldItem().BlockRangedSkill(player, proj);
                        }
                        else
                        {
                            proj.damage -= playerClass.playerDefense;
                        }
                    }
                }
            }
        }
    }
}
