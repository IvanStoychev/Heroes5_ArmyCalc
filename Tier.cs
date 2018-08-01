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
        public int GoldCost_Base;
        /// <summary>
        /// The gold cost for an upgraded creature of this tier.
        /// </summary>
        public int GoldCost_Upgraded;
        /// <summary>
        /// This tier's base weekly growth.
        /// </summary>
        public int WeeklyPopulation_Base;
        /// <summary>
        /// This tier's bonus weekly growth.
        /// Gained from skills, town or neutral dwellings.
        /// </summary>
        public int WeeklyPopulation_Bonus;
    }
}
