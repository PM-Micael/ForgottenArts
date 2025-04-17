using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ForgottenArts.TEST
{
    public class LaserBeam : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;  // Width of the laser
            Projectile.height = 4; // Height of the laser
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;  // Infinite penetration
            Projectile.timeLeft = 600;  // It will last for 10 seconds if not reset
            Projectile.tileCollide = false; // No collision with tiles
            Projectile.ignoreWater = true;  // Ignore water
        }

        // Updating the laser's position, speed, and behavior
        public override void AI()
        {
            // Make the laser stretch out in the direction the player is facing
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity = new Vector2(16f, 0f).RotatedBy(Projectile.rotation);  // Shoot straight in the direction the player is facing

            // Every frame, reset the laser to last forever until canceled
            Projectile.timeLeft = 600;
        }

        // Optional: If you want a trail or effects
        public override void PostDraw(Color lightColor)
        {
            // Example of drawing the laser with a glow (use your own texture or logic)
            //Terraria.Graphics.Renderers.EffectsSystem.DrawLaserEffect(Projectile.Center, Projectile.rotation, 1f);
        }
    }
}

