using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ForgottenArts.Items.Projectiles
{
    internal class PlayerGlobalProjectile : GlobalProjectile
    {
        public bool isReflected = false;

        public override bool InstancePerEntity => true;

        public override void SetDefaults(Projectile entity)
        {
            isReflected = false;
        }
    }
}
