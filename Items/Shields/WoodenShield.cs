using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Utilities.Terraria.Utilities;

namespace ForgottenArts.Items.Shields
{
    public class WoodenShield : Shield
    {
        public override int Multipliers(Player player)
        {
            float multiplier = 0.5f;
            int playerDefence = player.statDefense;
            int result = (int)Math.Round(playerDefence * multiplier);
            return result;
        }

        public override void ParryMeleeSkill(Player player, NPC npc)
        {

        }

        public override void ParryRangedSkill(Player player, Projectile proj)
        {

        }

        public override List<ShieldBuff> StatusEffects()
        {
            return null;
        }
    }
}
