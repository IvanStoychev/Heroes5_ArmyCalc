using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heroes5_ArmyCalc
{
    class Tier
    {
        /// <summary>
        /// Denotes which tier the object is for.
        /// </summary>
        public int Number;
        /// <summary>
        /// The gold cost for an unupgraded creature of this tier.
        /// </summary>
        public int BaseGoldCost;
        /// <summary>
        /// The gold cost for an upgraded creature of this tier.
        /// </summary>
        public int UpgradedGoldCost;
        /// <summary>
        /// This tier's weekly growth.
        /// </summary>
        public int WeeklyPopulation;
    }
}
