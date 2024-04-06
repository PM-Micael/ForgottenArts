using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ForgottenArts.Items.Projectiles
{
    internal class TrackingCursedFlame : TrackingFireball
    {
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.buffImmune[39])
            {
                target.AddBuff(39, 300);
            }
        }
    }
}
