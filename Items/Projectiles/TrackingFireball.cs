using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ForgottenArts.Items.Projectiles
{
    internal class TrackingFireball : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.aiStyle = -1; // Custom AI
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = ModContent.GetInstance<Heavy>();
            Projectile.tileCollide = false; // So it doesn't disappear upon hitting tiles
            Projectile.ignoreWater = true; // Ignore water
        }

        public override void AI()
        {
            // Target closest NPC
            float targetMaxDistance = 1000f; // Max distance to search for enemies
            NPC target = null;
            float targetDistance = targetMaxDistance;
            foreach (NPC npc in Main.npc)
            {
                float distance = Vector2.Distance(npc.Center, Projectile.Center);
                if (distance < targetDistance && npc.CanBeChasedBy())
                {
                    targetDistance = distance;
                    target = npc;
                }
            }

            if (target != null)
            {
                // Calculate direction towards target and move there
                float speed = 10f; // Speed of the fireball
                Vector2 move = target.Center - Projectile.Center;
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                move *= speed / magnitude;
                Projectile.velocity = move;
            }
        }
    }
}
