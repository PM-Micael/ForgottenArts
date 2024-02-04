using ForgottenArts.Buffs.AdvancedBuffs;
using ForgottenArts.Buffs.BaseBuffs;
using ForgottenArts.Items.Shields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.NPC;

namespace ForgottenArts
{
    internal class PlayerClass : ModPlayer
    {
        public bool inBlockStance = false;
        public bool inParryStance = false;
        public bool inParryStanceLastFrame = false;
        public bool parrySuccessful = false;
        public ParryStreak parryStreak;

        //********************
        private bool hasTakenDamage;
        private bool isBuffActive;
        //********************

        public override void ResetEffects()
        {
            CheckIfTookDamageDuringParry();
        }

        public override void PostUpdate()
        {
            CheckParryStance();

            inBlockStance = Player.HasBuff(ModContent.BuffType<Buffs.BaseBuffs.BlockStance>());
            inParryStance = Player.HasBuff(ModContent.BuffType<Buffs.BaseBuffs.ParryStance>());
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            hasTakenDamage = true;
            if (inParryStance && IsFacingNPC(npc))
            {
                PreformParryOnMelee(npc, ref modifiers);
            }
            else if (!inParryStance)
            {

            }
            else if(inBlockStance && IsFacingNPC(npc))
            {
                modifiers.SetMaxDamage((int)Math.Round(npc.damage * 0.7));
                modifiers.Knockback *= 0;
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            
        }

        public void GetMouseInputs()
        {

        }
        //MY methods******************************************************************************************************************

        public void CheckIfTookDamageDuringParry()
        {
            // This method runs every tick, so it's a good place to check the status of the buff
            if (Player.HasBuff(ModContent.BuffType<ParryStance>()))
            {
                if (!isBuffActive)
                {
                    // Buff has just been applied
                    isBuffActive = true;
                    hasTakenDamage = false; // Reset the damage flag since the buff is active
                }
            }
            else if (isBuffActive)
            {
                // Buff has just expired
                isBuffActive = false;
                if (!hasTakenDamage)
                {
                    // Player did not take damage during the buff's duration
                    // Implement your logic here for what happens in this case

                    Player.AddBuff(ModContent.BuffType<ShieldCooldown>(), 180);
                }
            }
        } 

        public void CheckParryStance()
        {
            bool playerInParryStance = Player.HasBuff(ModContent.BuffType<Buffs.BaseBuffs.ParryStance>());

            if (parrySuccessful)
            {
                parrySuccessful = false;
                Player.ClearBuff(ModContent.BuffType<Buffs.BaseBuffs.ParryStance>());
                Player.AddBuff(ModContent.BuffType<Buffs.BaseBuffs.ShieldCooldown>(), 20);

                if (parryStreak != null)
                {
                    parryStreak.count = 600;
                    Player.AddBuff(ModContent.BuffType<Buffs.AdvancedBuffs.ParryStreakDuration>(), 600);
                    parryStreak.count++;

                    if (parryStreak.count >= 3)
                    {
                        parryStreak.count = 3;
                    }
                }
            }
            /*
            else if(inParryStanceLastFrame && !playerInParryStance) //True if Player had P.S last frame but not now.
            {
                Player.AddBuff(ModContent.BuffType<Buffs.BaseBuffs.ShieldCooldown>(), 180);

                if (parryStreak != null)
                {
                    parryStreak.count = 0;
                    Player.ClearBuff(ModContent.BuffType<Buffs.AdvancedBuffs.ParryStreakDuration>());
                }
            }
            */

            inParryStanceLastFrame = playerInParryStance;
        }
        public Shield GetHeldItem()
        {
            if(Player.HeldItem.ModItem is Shield shield)
            {
                return shield;
            }
            return null;
        }

        public void PreformParryOnMelee(NPC npc, ref Player.HurtModifiers modifiers)
        {
            parrySuccessful = true;
            SoundStyle parrySound = new SoundStyle("ForgottenArts/Sounds/Parry-Success")
            {
                Volume = 0.5f,
                PitchVariance = 0.2f
            };
            SoundEngine.PlaySound(parrySound);
            HitInfo hitNPC = new HitInfo();

            int playerDirection = Player.direction;
            var direction = npc.Center - Player.Center;
            direction.Normalize();
            if (npc.boss) //Decides knockback
            {
                direction *= 10f;
            }
            else
            {
                direction *= 10f;
            }
            npc.velocity = direction;
            hitNPC.Damage = (int)(npc.damage * GetHeldItem().Multipliers()) + (Player.statDefense * GetHeldItem().Multipliers());
            hitNPC.DamageType = GetHeldItem().Item.DamageType;
            hitNPC.Knockback *= 5;//Might not be needed
            npc.StrikeNPC(hitNPC);

            var shieldDebuffs = GetHeldItem().StatusEffects();
            if (shieldDebuffs != null)
            {
                foreach (var debuff in shieldDebuffs)
                {
                    npc.AddBuff(debuff.buffID, debuff.duration);
                }
            }

            //Player effects \/
            modifiers.DisableSound();
            modifiers.SetMaxDamage(0);
        }

        private bool IsFacingNPC(NPC npc)
        {
            return (Player.direction == 1 && npc.Center.X > Player.Center.X) || (Player.direction == -1 && npc.Center.X < Player.Center.X);
        }

        private bool IsFacingProjectile(Projectile proj)
        {
            return (Player.direction == 1 && proj.Center.X > Player.Center.X) || (Player.direction == -1 && proj.Center.X < Player.Center.X);
        }
    }
}
