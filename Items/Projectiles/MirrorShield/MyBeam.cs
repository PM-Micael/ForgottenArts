using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.NPC;

namespace ForgottenArts.Items.Projectiles.MirrorShield
{
    public class MyBeam : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1; // Custom AI style
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1; // Infinite penetration
            Projectile.timeLeft = 2; // Time left will be set dynamically
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 2;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[0] = 1f;
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.netUpdate = true;
            }

            // Continually create the beam
            Vector2 position = Projectile.Center + Projectile.velocity * (Projectile.ai[1] - 1f);
            Dust.NewDust(position, Projectile.width, Projectile.height, DustID.RedTorch);

            // Extend the beam
            Projectile.ai[1] += 1f;

            // Reduce the time left to keep the projectile active
            Projectile.timeLeft = 2;

            // Check collision with NPCs
            Rectangle projectileRect = new Rectangle((int)position.X, (int)position.Y, Projectile.width, Projectile.height);

            HitInfo hitInfo = new HitInfo();
            hitInfo.Damage = Projectile.damage;
            hitInfo.Knockback = Projectile.knockBack;
            hitInfo.HitDirection = Projectile.direction;

            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.getRect().Intersects(projectileRect))
                {
                    npc.StrikeNPC(hitInfo);
                }
            }
        }
    }
}
