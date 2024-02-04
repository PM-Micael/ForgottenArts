using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgottenArts.Items.Shields
{
    internal class WoodenShield : Shield
    {
        public override float Multipliers()
        {
            return 0.5f;
        }

        public override void ParryMeleeSkill()
        {
            throw new NotImplementedException();
        }

        public override List<ShieldBuffs> StatusEffects()
        {
            throw new NotImplementedException();
        }
    }
}
