using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ForgottenArts.Buffs.AdvancedBuffs
{
    public class ParryStreakDduration : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();
            player.statDefense += 5;
        }
    } 
}
