using ForgottenArts.Buffs.AdvancedBuffs;
using ForgottenArts.Items.Shields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
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
        public bool shieldOnCooldown = false;
        public ParryStreak parryStreak;

        public override void PostUpdate()
        {

            inBlockStance = Player.HasBuff(ModContent.BuffType<Buffs.BaseBuffs.BlockStance>());
            inParryStance = Player.HasBuff(ModContent.BuffType<Buffs.BaseBuffs.ParryStance>());
            shieldOnCooldown = Player.HasBuff(ModContent.BuffType<Buffs.BaseBuffs.ShieldCooldown>());
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
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
            
        }

        public void GetMouseInputs()
        {

        }

        //MY methods******************************************************************************************************************

        public void CheckParryStance()
        {
            bool playerInParryStance = Player.HasBuff(ModContent.BuffType<Buffs.BaseBuffs.ParryStance>());

            if (parrySuccessful)
            {
                parryStreak.count = 600;
                Player.ClearBuff(ModContent.BuffType<Buffs.BaseBuffs.ParryStance>());
                Player.AddBuff(ModContent.BuffType<Buffs.AdvancedBuffs.ParryStreak>(), 600);
                Player.AddBuff(ModContent.BuffType<Buffs.BaseBuffs.ShieldCooldown>(), 20);

                if (parryStreak != null)
                {
                    parryStreak.count++;

                    if (parryStreak.count >= 3)
                    {
                        parryStreak.count = 3;
                    }
                }
            }
            else if(inParryStanceLastFrame && !playerInParryStance) //True if Player had P.S last frame but not now.
            {

            }

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
            SoundStyle parrySound = new SoundStyle("LostArts/Sounds/Parry-Success")
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
            foreach(var debuff in shieldDebuffs)
            {
                npc.AddBuff(debuff.buffID, debuff.duration);
            }

            //Player effects \/
            HitInfo hitPlayer = new HitInfo();
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
