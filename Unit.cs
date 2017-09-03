using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heroes5_ArmyCalc
{
    public class Unit
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] ImgBytes { get; set; }
        public int BaseGoldCost { get; set; }
        public int UpgGoldCost { get; set; }
        public int Population { get; set; }

        public Unit()
        {
            Name = "";
        }
    }
}
