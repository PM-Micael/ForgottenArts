﻿using ForgottenArts.Buffs.AdvancedBuffs;
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
using Terraria.WorldBuilding;
using static Terraria.NPC;

namespace ForgottenArts
{
    public class PlayerClass : ModPlayer
    {
        public bool inBlockStance = false;
        public bool inParryStance = false;
        private bool isBuffActive;
        public ParryStreak parryStreak;
        public int playerDefense = 0;
        public bool cannotTakeDamage = false;

        public bool usedParry = false;
        public bool parrySuccessful = false;


        public override void ResetEffects()
        {
            usedParry = false;
            cannotTakeDamage = false;
            Player.moveSpeed = 1f;
        }

        public override void PostUpdate()
        {
            playerDefense = Player.statDefense;
            inBlockStance = Player.HasBuff(ModContent.BuffType<Buffs.BaseBuffs.BlockStance>());
            inParryStance = Player.HasBuff(ModContent.BuffType<Buffs.BaseBuffs.ParryStance>());
            if (!Player.HasBuff(ModContent.BuffType<ParryStreak>()))
            {
                parryStreak.count = 0;
            }

            CheckParry();
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
            if(cannotTakeDamage || (inParryStance && IsFacingNPC(npc)))
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
            if(inBlockStance && IsFacingNPC(npc))
            {
                modifiers.SetMaxDamage((int)Math.Round(npc.damage * 0.7));
                modifiers.Knockback *= 0;
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (inBlockStance && IsFacingProjectile(proj))
            {
                modifiers.SetMaxDamage((int)Math.Round(proj.damage * 0.7));
                modifiers.Knockback *= 0;
            }
        }

        //MY methods******************************************************************************************************************

        public void CheckParry()
        {
            if (inParryStance)
            {
                PreformParryOnMelee();
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

        public void PreformParryOnMelee() //outdated name
        {
            foreach(NPC npc in Main.npc)
            {
                if (npc.Distance(Player.Center) < GetHeldItem().Item.width && IsFacingNPC(npc))
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
                    npc.knockBackResist = npc.boss ? 0 : default;
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
                        Player.AddBuff(ModContent.BuffType<Buffs.AdvancedBuffs.ParryStreak>(), 900);
                        parryStreak.count++;

                        if (parryStreak.count >= 3)
                        {
                            parryStreak.count = 3;
                        }
                    }

                    parrySuccessful = true;
                }
            }

            foreach(Projectile proj in Main.projectile)
            {
                if (proj.Distance(Player.Center) < GetHeldItem().Item.width && IsFacingProjectile(proj))
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
