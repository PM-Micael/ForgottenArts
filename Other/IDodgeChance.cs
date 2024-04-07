using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ForgottenArts.Other
{
    public interface IDodgeChance
    {
        public bool RollForDodgeDamage(Player player);

    }
}
