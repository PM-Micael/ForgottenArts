using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ForgottenArts.Buffs.BaseBuffs
{
    public class ParryStance : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();
            playerClass.inParryStance = true;
        }
    }
}
