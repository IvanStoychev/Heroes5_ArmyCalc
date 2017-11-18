using System.Drawing;

namespace Heroes5_ArmyCalc
{
    public class Unit
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Image Image { get; set; }
        public int BaseGoldCost { get; set; }
        public int UpgGoldCost { get; set; }
        public int PopulationWeekly { get; set; }

        public Unit(string name, string description, Image image, int baseGoldCost, int upgGoldCost, int populationWeekly)
        {
            Name = name;
            Description = description;
            Image = image;
            BaseGoldCost = baseGoldCost;
            UpgGoldCost = upgGoldCost;
            PopulationWeekly = populationWeekly;
        }
    }
}
