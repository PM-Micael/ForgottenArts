using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenArts.Items.Projectiles
{
    internal class LaserProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.aiStyle = 0; // Custom AI
            Projectile.friendly = true;
            Projectile.penetrate = 100;
            Projectile.DamageType = ModContent.GetInstance<Heavy>();
            Projectile.tileCollide = true; // So it doesn't disappear upon hitting tiles
            Projectile.ignoreWater = true; // Ignore water
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.light = 1f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.buffImmune[24])
            {
                target.AddBuff(BuffID.Ichor, 300);
            }
        }
    }
}
