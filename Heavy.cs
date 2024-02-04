using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ForgottenArts
{
    public class Heavy : DamageClass
    {
        public override void SetDefaultStats(Player player)
        {
            player.GetDamage<Heavy>() += 0;
            player.GetCritChance<Heavy>() += 0;
        }
    }
}
