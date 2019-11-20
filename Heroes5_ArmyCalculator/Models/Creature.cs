using System;
using System.Collections.Generic;
using System.Text;

namespace Heroes5_ArmyCalculator.Models
{
    /// <summary>
    /// Represents a creture from the game as an object.
    /// </summary>
    public class Creature
    {
        /// <summary>
        /// The primary key value for this creature.
        /// </summary>
        public int ID;

        /// <summary>
        /// The faction name this creature belongs to.
        /// </summary>
        public string Faction;

        /// <summary>
        /// The name of this creature.
        /// </summary>
        public string Name;

        /// <summary>
        /// The tier this creature belongs to.
        /// </summary>
        public int Tier;

        /// <summary>
        /// Which version of the creature this object represents -
        /// regular, upgraded or expansion upgrade.
        /// </summary>
        public int Upgrade;

        /// <summary>
        /// The cost to recruit one of this creature.
        /// </summary>
        public int GoldCost;

        /// <summary>
        /// The amount of creatures that become available
        /// for recruitment at the start of each week.
        /// </summary>
        public int PopulationWeekly;

        /// <summary>
        /// The attack value of this creature.
        /// </summary>
        public int Attack;

        /// <summary>
        /// The defense value of this creature.
        /// </summary>
        public int Defense;

        /// <summary>
        /// The minimum amount of damage each creature deals.
        /// </summary>
        public int DamageMin;

        /// <summary>
        /// The maximum amount of damage each creature deals.
        /// </summary>
        public int DamageMax;

        /// <summary>
        /// The initiative value of this creature.
        /// </summary>
        public int Initiative;

        /// <summary>
        /// The maximum amount of squares this creature can move per turn.
        /// </summary>
        public int Speed;

        /// <summary>
        /// The maximum health points this creature has.
        /// </summary>
        public int Health;

        /// <summary>
        /// The maximum mana points this creature has.
        /// </summary>
        public int Mana;

        /// <summary>
        /// The maximum ammunition reserves of this creature.
        /// </summary>
        public int Shots;

        /// <summary>
        /// The amount of experience points awarded for killing this creature.
        /// </summary>
        public int Experience;

        /// <summary>
        /// The power rating of this creautre.
        /// </summary>
        public int PowerRating;

        /// <summary>
        /// The list of abilities this creature possesses.
        /// </summary>
        public List<Ability> Abilities;
    }
}
