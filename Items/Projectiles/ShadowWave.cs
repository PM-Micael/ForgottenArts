﻿using Microsoft.Xna.Framework;
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
    internal class ShadowWave : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.DamageType = ModContent.GetInstance<Heavy>();
            Projectile.tileCollide = false; // So it doesn't disappear upon hitting tiles
            Projectile.ignoreWater = true; // Ignore water
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(target.type != NPCID.DD2LanePortal)
            {
                target.AddBuff(BuffID.ShadowFlame, 600);
            }
        }
    }
}
