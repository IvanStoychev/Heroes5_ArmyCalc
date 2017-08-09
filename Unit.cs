using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heroes5_ArmyCalc
{
    class Unit
    {
        public string Name { get; set; }
        public byte[] ImgBytes { get; set; }
        public int BaseGoldCost { get; set; }
        public int UpgGoldCost { get; set; }
        public int Population { get; set; }
    }
}
