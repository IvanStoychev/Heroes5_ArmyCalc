﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heroes5_ArmyCalc
{
    class Faction
    {
        /// <summary>
        /// The name of the faction in the game.
        /// </summary>
        public string Name;
        /// <summary>
        /// An array representing information about each tier of this faction.
        /// Zero-padded for ease of use.
        /// </summary>
        public Tier[] Tiers = new Tier[8];
        /// <summary>
        /// The tier whose population the faction's first bonus dwelling boosts.
        /// </summary>
        public int Dwelling1_Tier;
        /// <summary>
        /// The tier whose population the faction's second bonus dwelling boosts.
        /// </summary>
        public int Dwelling2_Tier;
    }
}