using ForgottenArts.Buffs.AdvancedBuffs;
using ForgottenArts.Buffs.BaseBuffs;
using ForgottenArts.Items.Projectiles;
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
    public class PlayerClass : ModPlayer
    {
        public bool inBlockStance = false;
        public bool inParryStance = false;
        private bool hasTakenDamage;
        private bool isBuffActive;
        public ParryStreak parryStreak;


        public override void ResetEffects()
        {
            Player.moveSpeed = 1f;
            CheckIfTookDamageDuringParry();
        }

        public override void PostUpdate()
        {
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
            else if(inBlockStance && IsFacingNPC(npc))
            {
                modifiers.SetMaxDamage((int)Math.Round(npc.damage * 0.7));
                modifiers.Knockback *= 0;
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            hasTakenDamage = true;
            if (inParryStance && IsFacingProjectile(proj))
            {
                PreformParryOnRanged(proj, ref modifiers);
            }
            else if (inBlockStance && IsFacingProjectile(proj))
            {
                modifiers.SetMaxDamage((int)Math.Round(proj.damage * 0.7));
                modifiers.Knockback *= 0;
            }
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
            hitNPC.Damage = (npc.damage + GetHeldItem().Multipliers(Player));
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

            GetHeldItem().ParryMeleeSkill(Player, npc);

            //Player effects \/
            modifiers.DisableSound();
            modifiers.SetMaxDamage(0);

            Player.ClearBuff(ModContent.BuffType<Buffs.BaseBuffs.ParryStance>());
            Player.AddBuff(ModContent.BuffType<Buffs.BaseBuffs.ShieldCooldown>(), 20);

            if (parryStreak != null)
            {
                Player.AddBuff(ModContent.BuffType<Buffs.AdvancedBuffs.ParryStreak>(), 600);
                parryStreak.count++;

                if (parryStreak.count >= 3)
                {
                    parryStreak.count = 3;
                }
            }
        }

        public void PreformParryOnRanged(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            SoundStyle parrySound = new SoundStyle("ForgottenArts/Sounds/Parry-Success")
            {
                Volume = 0.5f,
                PitchVariance = 0.2f
            };
            SoundEngine.PlaySound(parrySound);

            int damage = proj.damage;

            // Ensure the projectile is not already reflected to avoid infinite loops
            if (proj.GetGlobalProjectile<PlayerGlobalProjectile>().isReflected) return;

            // Reverse projectile direction
            proj.velocity *= -1;

            // Mark the projectile as reflected to change its behavior (see below)
            proj.GetGlobalProjectile<PlayerGlobalProjectile>().isReflected = true;

            // Optionally, change the owner of the projectile to the player, so it can damage enemies
            proj.owner = Player.whoAmI;

            proj.friendly = true;
            proj.damage = GetHeldItem().Multipliers(Player);

            //Player effects \/*****************************************************************************************************
            modifiers.DisableSound();
            modifiers.SetMaxDamage(0);

            Player.ClearBuff(ModContent.BuffType<Buffs.BaseBuffs.ParryStance>());
            Player.AddBuff(ModContent.BuffType<Buffs.BaseBuffs.ShieldCooldown>(), 20);

            GetHeldItem().ParryRangedSkill(Player, proj);

            if (parryStreak != null)
            {
                Player.AddBuff(ModContent.BuffType<Buffs.AdvancedBuffs.ParryStreak>(), 600);
                parryStreak.count++;

                if (parryStreak.count >= 3)
                {
                    parryStreak.count = 3;
                }
            }
        }

        public bool IsFacingNPC(NPC npc)
        {
            return (Player.direction == 1 && npc.Center.X > Player.Center.X) || (Player.direction == -1 && npc.Center.X < Player.Center.X);
        }

        public bool IsFacingProjectile(Projectile proj)
        {
            return (Player.direction == 1 && proj.Center.X > Player.Center.X) || (Player.direction == -1 && proj.Center.X < Player.Center.X);
        }
    }
}
