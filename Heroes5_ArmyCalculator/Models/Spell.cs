using System;
using System.Collections.Generic;
using System.Text;

namespace Heroes5_ArmyCalculator.Models
{
    /// <summary>
    /// Represents a spell from the game as an object.
    /// </summary>
    public class Spell
    {
        /// <summary>
        /// The primary key value for this spell.
        /// </summary>
        public int ID;

        /// <summary>
        /// The school of magic this spell belongs to.
        /// </summary>
        public string School;

        /// <summary>
        /// The level of the spell.
        /// </summary>
        public int Level;

        /// <summary>
        /// The name of the spell.
        /// </summary>
        public string Name;

        /// <summary>
        /// A descriptions of the effects of the spell.
        /// </summary>
        public string Description;

        /// <summary>
        /// The cost, in manapoints, of this spell.
        /// </summary>
        public int Cost;

        /// <summary>
        /// The effects of the spell when cast with no mastery.
        /// </summary>
        public string NoMastery;

        /// <summary>
        /// The effects of the spell when cast with basic mastery.
        /// </summary>
        public string BasicMastery;

        /// <summary>
        /// The effects of the spell when cast with advanced mastery.
        /// </summary>
        public string AdvancedMastery;

        /// <summary>
        /// The effects of the spell when cast with expert mastery.
        /// </summary>
        public string ExpertMastery;
    }
}
