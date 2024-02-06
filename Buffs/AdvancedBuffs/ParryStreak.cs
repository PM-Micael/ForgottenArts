using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ForgottenArts.Buffs.AdvancedBuffs
{
    public class ParryStreak : ModBuff
    {
        public int count = 0;
        public bool powerUp = false;
        public override void Update(Player player, ref int buffIndex)
        {
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();

            if(count >= 3)
            {
                powerUp = true;
            }
            else
            {
                powerUp= false;
            }
            
        }

        public void ResetStreak()
        {
            count = 0;
        }
    }
}
