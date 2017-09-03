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
        public Unit[] Units { get; set; }

        public Faction(string factionName)
        {
            Name = factionName;
            Units = new Unit[8];
            for (int i = 1; i < 8; i++)
            {
                Units[i] = new Unit();
            }
            Units[0] = null;
        }
    }
}
