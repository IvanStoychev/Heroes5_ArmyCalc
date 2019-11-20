using System;
using System.Collections.Generic;
using System.Text;

namespace Heroes5_ArmyCalculator.Models
{
    /// <summary>
    /// Represents an ability from the game as an object.
    /// </summary>
    public class Ability
    {
        /// <summary>
        /// The primary key value for this ability.
        /// </summary>
        public int ID;

        /// <summary>
        /// The name of this ability.
        /// </summary>
        public string Name;

        /// <summary>
        /// The description of the effects of this ability.
        /// </summary>
        public string Description;
    }
}
