﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ForgottenArts.Buffs.AdvancedBuffs
{
    public class ParryStreak
    {
        public int count = 0;

        public void ResetStreak()
        {
            count = 0;
        }
    }
}
