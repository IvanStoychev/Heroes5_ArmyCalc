using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heroes5_ArmyCalc
{
    public class Faction
    {
        public string Name { get; set; }
        public Unit[] Units = new Unit[8];

        public Faction(string factionName)
        {
            Name = factionName;
        }
    }
}
