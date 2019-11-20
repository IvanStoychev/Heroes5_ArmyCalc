using System;
using System.Collections.Generic;
using System.Text;

namespace Heroes5_ArmyCalculator.Models
{
    public class Creature
    {
        public int ID;
        public string Faction;
        public string Name;
        public int Tier;
        public int Upgrade;
        public int GoldCost;
        public int PopulationWeekly;
        public int Attack;
        public int Defense;
        public int DamageMin;
        public int DamageMax;
        public int Initiative;
        public int Speed;
        public int Health;
        public int Mana;
        public int Shots;
        public int Experience;
        public int PowerRating;
        public List<string> Abilities;
    }
}
