using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgottenArts
{
    public class ShieldBuff
    {
        public int buffID { get; set; }
        public int duration { get; set; }

        public ShieldBuff(int buffID, int duration)
        {
            this.buffID = buffID;
            this.duration = duration;
        }
    }
}
