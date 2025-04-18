using ForgottenArts.Buffs.BaseBuffs;
using ForgottenArts.Items.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenArts.Items.Shields
{
    public class MoltenShield : Shield
    {
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 15);
            recipe.AddIngredient(ItemID.Obsidian, 15);
            recipe.AddIngredient(ItemID.JungleSpores, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override void SetDefaults()
        {
            Item.defense = 5;

            base.SetDefaults();
        }

        public override List<ShieldBuff> StatusEffects()
        {
            List<ShieldBuff> buffs = new List<ShieldBuff>();
            buffs.Add(new ShieldBuff(BuffID.OnFire, 600));
            return buffs;
        }

        public override int Multipliers(Player player)
        {
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();
            int multiplier = 1;
            int playerDefense = playerClass.playerDefense;
            int result = playerDefense * multiplier;
            return result;
        }

        public override void ParryMeleeSkill(Player player, NPC npc)
        {
            PlayerClass modPlayer = player.GetModPlayer<PlayerClass>();

            //Projectile.NewProjectile(player.GetSource_OnHit(npc), npc.position, Vector2.Zero, ProjectileID.Grenade, Multipliers(player), 0, player.whoAmI);
        }

        public override void ParryRangedSkill(Player player, Projectile proj)
        {
            int projectileType = ModContent.ProjectileType<TrackingFireball>(); // Your custom fireball projectile
            float projectileSpeed = 10f; // Speed of the projectile
            int projectileDamage = 300; // Damage of the projectile
            float projectileKnockback = 1f; // Knockback of the projectile


            // Determine the direction to shoot the fireball (e.g., towards the cursor)
            Vector2 direction = Main.MouseWorld - player.Center;
            direction.Normalize();

            // Spawn the projectile
            Projectile.NewProjectile(player.GetSource_ItemUse(this.Item), player.Center, direction * projectileSpeed, projectileType, projectileDamage, projectileKnockback, player.whoAmI);
            proj.Kill();
        }

        public override void BlockMeleeSkill(Player player, NPC npc)
        {

        }

        public override void BlockRangedSkill(Player player, Projectile proj)
        {
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();

            if (playerClass.parryStreak != null)
            {
                if (playerClass.parryStreak.count == 3) //Trigger powerUp
                {
                    var directionToCursor = Main.MouseWorld - player.Center;
                    directionToCursor.Normalize();
                    float speed = proj.velocity.Length();
                    proj.velocity = directionToCursor * speed;
                    proj.friendly = true;
                    proj.owner = player.whoAmI;
                    //playerClass.GetHeldItem().PowerUpSkill(player, proj);
                    ParryRangedSkill(player, proj);
                }
                else
                {
                    proj.Kill();
                }
            }
            else
            {
                proj.Kill();
            }
        }

        public override void BlockNone(Player player)
        {

        }

        public override int ArmorCheck()
        {
            return 33;
        }

        public override int MaxContactDamageBlock()
        {
            throw new NotImplementedException();
        }

        public override int MaxContactDamageParry()
        {
            throw new NotImplementedException();
        }

        public override int MaxProjectileDamageBlock()
        {
            throw new NotImplementedException();
        }

        public override int MaxProjectileDamageParry()
        {
            throw new NotImplementedException();
        }
    }
}