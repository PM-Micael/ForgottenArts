using ForgottenArts.Buffs.AdvancedBuffs;
using ForgottenArts.Buffs.BaseBuffs;
using ForgottenArts.Items.Projectiles;
using ForgottenArts.Items.Shields;
using ForgottenArts.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Terraria.NPC;

namespace ForgottenArts
{
    public class PlayerClass : ModPlayer
    {
        public bool inBlockStance = false;
        public bool inParryStance = false;
        public bool usedParry = false;
        public bool parrySuccessful = false;
        public ParryStreak parryStreak;
        public int playerDefense = 0;
        public bool cannotTakeDamage = false;
        public float damageReduction = 1f;

        public int runningDuration = 0;
        public float Mph = 0;
        public float PlayerSpeed = 0;
        public float statDefense;

        // Endurence \/ ************************************************************************

        public int EndurenceMax;
        public float EndurenceCurrent = 0f;
        public float EndurenceRegenRate;
        public float BlockMultiplier = 0.5f;

        public float EndurenceToRemove = 0f;
        public float LastDamageBlocked = 0f;
        // Endurence /\ ************************************************************************
        public override void ResetEffects()
        {
            damageReduction = 1f;
            usedParry = false;
            cannotTakeDamage = false;
            Player.moveSpeed = 1f;
            // Endurence \/
            BlockMultiplier = 0.5f;

            EndurenceMax = Player.statLifeMax;
            EndurenceRegenRate = 1f;
        }

        public override void PostUpdate()
        {
            RemoveEndurence();

            //Regen 
            if(EndurenceCurrent < EndurenceMax)
               EndurenceCurrent += EndurenceRegenRate;

            if (EndurenceCurrent > EndurenceMax)
                EndurenceCurrent = EndurenceMax;

            statDefense = Player.statDefense;
            // Endurance /\

            PlayerSpeed = Math.Abs(Player.velocity.X) * 60 / 16; //Double job / fixt later but not now becasue cba

            playerDefense = Player.statDefense;
            inBlockStance = Player.HasBuff(ModContent.BuffType<Buffs.BaseBuffs.BlockStance>());
            inParryStance = Player.HasBuff(ModContent.BuffType<Buffs.BaseBuffs.ParryStance>());
            
            if (!Player.HasBuff(ModContent.BuffType<ParryStreak>()) && parryStreak != null)
            {
                parryStreak.count = 0;
            }
            

            CheckParry();

            if (!Player.HasBuff(ModContent.BuffType<BlockStance>()))
            {
                runningDuration = 0;
                Mph = 0;
            }
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {

            if (cannotTakeDamage || (inParryStance && IsFacingProjectile(proj)))
            {
                return false;
            }
            else if (inBlockStance && IsFacingProjectile(proj))
            {
                return false;
            }

            return true;
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {

            //**************************************************************

            if (cannotTakeDamage || (inParryStance && IsFacingNPC(npc)))
            {
                return false;
            }
            else if(inBlockStance && IsFacingNPC(npc))
            {
                return false;
            }

            return true;
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            for(int i = 0;i <= 10;i++)
            {
                if (Player.armor[i].ModItem is IDodgeChance dodge)
                {
                    bool result = dodge.RollForDodgeDamage(Player);

                    if(result)
                    {
                        Player.AddBuff(ModContent.BuffType<Buffs.BaseBuffs.CannotTakeDamage>(), 120);
                        modifiers.SetMaxDamage(0);
                        modifiers.Knockback *= 0;
                        modifiers.DisableSound();
                    }

                    break;
                }
                else if (inBlockStance && IsFacingNPC(npc)) //Will rarely trigger
                {
                    modifiers.SetMaxDamage((int)Math.Round((npc.damage * 0.7) * damageReduction));
                    modifiers.Knockback *= 0;
                }
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            for (int i = 0; i <= 10; i++)
            {
                if (Player.armor[i].ModItem is IDodgeChance dodge)
                {
                    bool result = dodge.RollForDodgeDamage(Player);

                    if (result)
                    {
                        Player.AddBuff(ModContent.BuffType<Buffs.BaseBuffs.CannotTakeDamage>(), 120);
                        modifiers.SetMaxDamage(0);
                        modifiers.Knockback *= 0;
                        modifiers.DisableSound();
                    }

                    break;
                }
                else if (inBlockStance && IsFacingProjectile(proj))
                {
                    modifiers.SetMaxDamage((int)Math.Round((proj.damage * 0.7) * damageReduction));
                    modifiers.Knockback *= 0;
                }
            }
        }

        //MY methods******************************************************************************************************************

        public void RemoveEndurence()
        {

            EndurenceToRemove = LastDamageBlocked - (statDefense * BlockMultiplier);

            if(EndurenceToRemove >= 0)
                EndurenceCurrent -= EndurenceToRemove;

            EndurenceToRemove = 0f;
            LastDamageBlocked = 0f;

            if (EndurenceCurrent < 0)
                EndurenceCurrent = 0;
        }

        public void CheckParry()
        {
            if (inParryStance)
            {
                PreformParry();
            }

            if (parrySuccessful)
            {
                parrySuccessful = false;
                usedParry = false;

                Player.ClearBuff(ModContent.BuffType<Buffs.BaseBuffs.ParryStance>());
                Player.AddBuff(ModContent.BuffType<Buffs.BaseBuffs.ShieldCooldown>(), 20);
                Player.AddBuff(ModContent.BuffType<Buffs.BaseBuffs.CannotTakeDamage>(), 50);
            }
            else if (usedParry && !inParryStance)
            {
                usedParry = false;

                Player.ClearBuff(ModContent.BuffType<Buffs.BaseBuffs.ParryStance>());
                Player.AddBuff(ModContent.BuffType<Buffs.BaseBuffs.ShieldCooldown>(), 180);
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

        public void PreformParry()
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.Distance(Player.Center) < GetHeldItem().BlockRadius && IsFacingNPC(npc) && !npc.friendly)
                {
                    Player.AddBuff(ModContent.BuffType<Buffs.BaseBuffs.CannotTakeDamage>(), 30);
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
                    npc.knockBackResist = 0;// npc.boss ? 0 : default;
                    direction *= npc.boss ? 10 : 15;//Knockback

                    npc.velocity = direction;
                    hitNPC.Damage = (npc.damage + GetHeldItem().Multipliers(Player));
                    hitNPC.DamageType = GetHeldItem().Item.DamageType;
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

                    if (parryStreak != null)
                    {
                        Player.AddBuff(ModContent.BuffType<Buffs.AdvancedBuffs.ParryStreak>(), 900); //15 seconds duration
                        parryStreak.count++;

                        if (parryStreak.count >= 3)
                        {
                            parryStreak.count = 3;
                        }
                    }

                    parrySuccessful = true;

                    break;
                }
            }

            foreach(Projectile proj in Main.projectile)
            {
                if (proj.Distance(Player.Center) < GetHeldItem().Item.width && IsFacingProjectile(proj) && !proj.friendly)
                {
                    parrySuccessful = true;
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
                    proj.owner = Player.whoAmI;
                    proj.damage = GetHeldItem().Multipliers(Player);

                    //Player effects \/*****************************************************************************************************

                    Player.ClearBuff(ModContent.BuffType<Buffs.BaseBuffs.ParryStance>());

                    GetHeldItem().ParryRangedSkill(Player, proj);

                    if (parryStreak != null)
                    {
                        Player.AddBuff(ModContent.BuffType<Buffs.AdvancedBuffs.ParryStreak>(), 900);
                        parryStreak.count++;

                        if (parryStreak.count >= 3)
                        {
                            parryStreak.count = 3;
                        }
                    }

                    break;
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
