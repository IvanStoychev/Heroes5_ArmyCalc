using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Heroes5_ArmyCalc
{
    public partial class FormMain : Form
    {
        /*
         * Introduction:
         * =============
         * In the game, for which this calculator is made - "Heroes of Might and Magic 5", there are a number of factions,
         * each capable of buying different creatures. The creatures of each faction are unique, but they all share the
         * same ranking system, being divided into "tiers" that range from 1 to 7.
         * - Each tier of creatures has three variations: one basic and two upgraded ones.
         * - All creatures have a weekly growth that is the same for the entire tier of each faction.
         * - Each creature costs gold to purchase. The upgraded variations cost the same, the basic one - less.
        */

        /// <summary>
        /// Holds the tier of the bonus dwelling for the selected faction.
        /// </summary>
        public int BonusDwellingTier1, BonusDwellingTier2;
        /// <summary>
        /// Array holding references to all the 'creature gold cost' labels of the form.
        /// Padded with an empty zero index for ease of use.
        /// </summary>
        public Label[] LabelsGold = new Label[8];
        /// <summary>
        /// Array holding references to all the 'creature population' labels of the form.
        /// Padded with an empty zero index for ease of use.
        /// </summary>
        public Label[] LabelsPopulation = new Label[8];
        /// <summary>
        /// Array holding references to all the 'creature total gold cost' labels of the form.
        /// Padded with an empty zero index for ease of use.
        /// </summary>
        public Label[] LabelsTotal = new Label[8];
        /// <summary>
        /// Array holding references to all the form's NumericUpDowns.
        /// Padded with an empty zero index for ease of use.
        /// </summary>
        public NumericUpDown[] NumericUpDowns = new NumericUpDown[8];
        /// <summary>
        /// Array of booleans that designate if an upgraded variant is selected or not for each creature tier
        /// </summary>
        public bool[] IsUpgraded = new bool[8];
        /// <summary>
        /// A list of the names of all the factions in the game.
        /// </summary>
        List<string> FactionList = new List<string> { "Academy", "Dungeon", "Fortress", "Haven", "Inferno", "Necropolis", "Stronghold", "Sylvan" };
        /// <summary>
        /// Holds all the data about the gold cost and population of each faction's units.
        /// </summary>
        DataTable Data = new DataTable();
        DataColumn Faction = new DataColumn("Faction", typeof(string));
        DataColumn Tier = new DataColumn("Tier", typeof(int));
        DataColumn CostBase = new DataColumn("CostBase", typeof(string));
        DataColumn CostUpg = new DataColumn("CostUpg", typeof(string));
        DataColumn Population = new DataColumn("Population", typeof(string));
        Dictionary<string, List<int>> CostBaseDictionary;
        Dictionary<string, List<int>> CostUpgDictionary;
        Dictionary<string, List<int>> PopulationDictionary;

        public FormMain()
        {
            InitializeComponent();
            LoadHardcodedData();
            AssignArrays();
            #region to be deleted
            //We assign data for all the factions to the array we will be working with.
            ////for (int i = 0; i < 8; i++)
            //{
            //    Town_Tier_Base_Gold[0, i] = Academy_Base_Gold_Tier[i];
            //    Town_Tier_Upg_Gold[0, i] = Academy_Upg_Gold_Tier[i];
            //    Town_Tier_Pop[0, i] = Academy_lblPopulation_Tier[i];

            //    Town_Tier_Base_Gold[1, i] = Dungeon_Base_Gold_Tier[i];
            //    Town_Tier_Upg_Gold[1, i] = Dungeon_Upg_Gold_Tier[i];
            //    Town_Tier_Pop[1, i] = Dungeon_lblPopulation_Tier[i];

            //    Town_Tier_Base_Gold[2, i] = Fortress_Base_Gold_Tier[i];
            //    Town_Tier_Upg_Gold[2, i] = Fortress_Upg_Gold_Tier[i];
            //    Town_Tier_Pop[2, i] = Fortress_lblPopulation_Tier[i];

            //    Town_Tier_Base_Gold[3, i] = Haven_Base_Gold_Tier[i];
            //    Town_Tier_Upg_Gold[3, i] = Haven_Upg_Gold_Tier[i];
            //    Town_Tier_Pop[3, i] = Haven_lblPopulation_Tier[i];

            //    Town_Tier_Base_Gold[4, i] = Inferno_Base_Gold_Tier[i];
            //    Town_Tier_Upg_Gold[4, i] = Inferno_Upg_Gold_Tier[i];
            //    Town_Tier_Pop[4, i] = Inferno_lblPopulation_Tier[i];

            //    Town_Tier_Base_Gold[5, i] = Necropolis_Base_Gold_Tier[i];
            //    Town_Tier_Upg_Gold[5, i] = Necropolis_Upg_Gold_Tier[i];
            //    Town_Tier_Pop[5, i] = Necropolis_lblPopulation_Tier[i];

            //    Town_Tier_Base_Gold[6, i] = Stronghold_Base_Gold_Tier[i];
            //    Town_Tier_Upg_Gold[6, i] = Stronghold_Upg_Gold_Tier[i];
            //    Town_Tier_Pop[6, i] = Stronghold_lblPopulation_Tier[i];

            //    Town_Tier_Base_Gold[7, i] = Sylvan_Base_Gold_Tier[i];
            //    Town_Tier_Upg_Gold[7, i] = Sylvan_Upg_Gold_Tier[i];
            //    Town_Tier_Pop[7, i] = Sylvan_lblPopulation_Tier[i];
            //}
            #endregion to be deleted
            cbFactionsList.SelectedItem = "Academy";
            UI_Update();
        }

        /// <summary>
        /// Initialises objects with hardcoded data.
        /// This method is a preparation for the migration to SQLite.
        /// </summary>
        void LoadHardcodedData()
        {
            // Load all the columns in the DataTable.
            Data.Columns.Add(Faction);
            Data.Columns.Add(Tier);
            Data.Columns.Add(CostBase);
            Data.Columns.Add(CostUpg);
            Data.Columns.Add(Population);

            // All these lists are padded with a zero as a first item for ease of use.
            // That way the population of an Academy tier 4 creature would be the 4th item of AcademyPopulation.
            List<int> AcademyCostBase = new List<int>(){ 0, 22, 45, 90, 250, 480, 1400, 3500 };
            List<int> AcademyCostUpg = new List<int>() { 0, 35, 70, 130, 340, 700, 1770, 4700 };
            List<int> AcademyPopulation = new List<int>() { 0, 20, 14, 9, 5, 3, 2, 1 };

            List<int> DungeonCostBase = new List<int>(){ 0, 60, 125, 140, 300, 550, 1400, 3000 };
            List<int> DungeonCostUpg = new List<int>(){ 0, 100, 175, 200, 450, 800, 1700, 3700 };
            List<int> DungeonPopulation = new List<int>(){ 0, 7, 5, 6, 4, 3, 2, 1 };

            List<int> FortressCostBase = new List<int>(){ 0, 24, 45, 130, 160, 470, 1300, 2700 };
            List<int> FortressCostUpg = new List<int>(){ 0, 40, 65, 185, 220, 700, 1700, 3400 };
            List<int> FortressPopulation = new List<int>(){ 0, 18, 14, 7, 6, 3, 2, 1 };

            List<int> HavenCostBase = new List<int>(){ 0, 15, 50, 85, 250, 600, 1300, 2800 };
            List<int> HavenCostUpg = new List<int>(){ 0, 25, 80, 130, 370, 850, 1700, 3500 };
            List<int> HavenPopulation = new List<int>(){ 0, 22, 12, 10, 5, 3, 2, 1 };

            List<int> InfernoCostBase = new List<int>(){ 0, 25, 40, 110, 240, 550, 1400, 2666 };
            List<int> InfernoCostUpg = new List<int>(){ 0, 45, 60, 160, 350, 780, 1666, 3666 };
            List<int> InfernoPopulation = new List<int>(){ 0, 16, 15, 8, 5, 3, 2, 1 };

            List<int> NecropolisCostBase = new List<int>(){ 0, 19, 40, 100, 250, 620, 1400, 1600 };
            List<int> NecropolisCostUpg = new List<int>(){ 0, 30, 60, 140, 380, 850, 1700, 1900 };
            List<int> NecropolisPopulation = new List<int>(){ 0, 20, 15, 9, 5, 3, 2, 1 };

            List<int> StrongholdCostBase = new List<int>(){ 0, 10, 50, 80, 260, 350, 1250, 2900 };
            List<int> StrongholdCostUpg = new List<int>(){ 0, 20, 70, 120, 360, 500, 1600, 3450 };
            List<int> StrongholdPopulation = new List<int>(){ 0, 25, 14, 11, 5, 5, 2, 1 };

            List<int> SylvanCostBase = new List<int>(){ 0, 35, 70, 120, 320, 630, 1100, 2500 };
            List<int> SylvanCostUpg = new List<int>(){ 0, 55, 120, 190, 440, 900, 1400, 3400 };
            List<int> SylvanPopulation = new List<int>(){ 0, 10, 9, 7, 4, 3, 2, 1 };

            // These Dictionaries allow foreach iteration and loading of data in the subsequent loop.
            CostBaseDictionary = new Dictionary<string, List<int>>()
            {
                { "Academy", AcademyCostBase },
                { "Dungeon", DungeonCostBase },
                { "Fortress", FortressCostBase },
                { "Haven", HavenCostBase },
                { "Inferno", InfernoCostBase },
                { "Necropolis", NecropolisCostBase },
                { "Stronghold", StrongholdCostBase },
                { "Sylvan", SylvanCostBase }
            };
            CostUpgDictionary = new Dictionary<string, List<int>>()
            {
                { "Academy", AcademyCostUpg },
                { "Dungeon", DungeonCostUpg },
                { "Fortress", FortressCostUpg },
                { "Haven", HavenCostUpg },
                { "Inferno", InfernoCostUpg },
                { "Necropolis", NecropolisCostUpg },
                { "Stronghold", StrongholdCostUpg },
                { "Sylvan", SylvanCostUpg }
            };
            PopulationDictionary = new Dictionary<string, List<int>>()
            {
                { "Academy", AcademyPopulation },
                { "Dungeon", DungeonPopulation },
                { "Fortress", FortressPopulation },
                { "Haven", HavenPopulation },
                { "Inferno", InfernoPopulation },
                { "Necropolis", NecropolisPopulation },
                { "Stronghold", StrongholdPopulation },
                { "Sylvan", SylvanPopulation }
            };

            // Load the hardcoded values.
            foreach (var faction in FactionList)
            {
                for (int i = 1; i <= 7; i++)
                {
                    DataRow dr = Data.NewRow();
                    dr[Faction] = faction;
                    dr[Tier] = i;
                    dr[CostBase] = CostBaseDictionary[faction][i];
                    dr[CostUpg] = CostUpgDictionary[faction][i];
                    dr[Population] = PopulationDictionary[faction][i];
                    Data.Rows.Add(dr);
                }
            }
        }

        /// <summary>
        /// Assigns form controls to members of arrays for ease of access.
        /// </summary>
        void AssignArrays()
        {
            LabelsPopulation[0] = null;
            LabelsGold[0] = null;
            LabelsTotal[0] = null;
            NumericUpDowns[0] = null;
            for (int i = 1; i < 8; i++)
            {
                LabelsPopulation[i] = (Label)Controls.Find("lblPopulation_Tier" + i, true).FirstOrDefault();
                LabelsGold[i] = (Label)Controls.Find("lblGoldPrice_Tier" + i, true).FirstOrDefault();
                LabelsTotal[i] = (Label)Controls.Find("lblTotal_Tier" + i, true).FirstOrDefault();
                NumericUpDowns[i] = (NumericUpDown)Controls.Find("udTier" + i, true).FirstOrDefault();
            }
        }

        /// <summary>
        /// Updates the UI to the appropriate faction's.
        /// </summary>
        public void UI_Update()
        {
            //Since the "Dungeon" faction has a set of specific controls we check if that is the selected faction
            //and if the corresponding checkbox is checked. If one of these conditions is not satisfied
            //the controls are not visible.
            if (Convert.ToString(cbFactionsList.SelectedItem) == "Dungeon" && chkDwelling1.Checked == true)
            {
                udExtraBloodMaiden.Visible = true;
                udExtraMinotaur.Visible = true;
                lblExtraBloodMaiden.Visible = true;
                lblExtraMinotaur.Visible = true;
            }
            else
            {
                udExtraBloodMaiden.Visible = false;
                udExtraMinotaur.Visible = false;
                lblExtraBloodMaiden.Visible = false;
                lblExtraMinotaur.Visible = false;
            }

            //Sets label texts and images to the currently selected faction
            //================================================================================
            //First determine which faction is selected and from that access the appropriate index/values in the data array

            string FactionName = (string)cbFactionsList.SelectedItem;
            int Gold_Total = 0;

            //This loop sets the labels for Creature Gold cost, Creature Population,
            //Cost of selected creatures and Total Gold.
            for (int i = 1; i <= 7; i++)
            {
                //Whenever the image of an upgraded creature is clicked, the boolean variable "Upg" is set to "true"
                //this "if" checkes whether "Upg" is true and changes the gold cost of the creature appropriately
                if (IsUpgraded[i] == true)
                {
                    LabelsGold[i].Text = CostUpgDictionary[FactionName][i].ToString();
                }
                else
                {
                    LabelsGold[i].Text = CostBaseDictionary[FactionName][i].ToString();
                }

                //This "if" sets the creature populations and UpDown controls' maximums in accordance
                //to a checked Castle or Citadel
				if (chkCastle.Checked == true)
				{
					LabelsPopulation[i].Text = Convert.ToString(PopulationDictionary[FactionName][i] * (int)udLimitPopulation.Value * 2);
				}
				else
				{
					if (chkCitadel.Checked == true)
					{
						LabelsPopulation[i].Text = Convert.ToString(Math.Truncate(PopulationDictionary[FactionName][i] * (int)udLimitPopulation.Value * (decimal)1.5));
					}
					else
					{
						LabelsPopulation[i].Text = Convert.ToString(PopulationDictionary[FactionName][i] * (int)udLimitPopulation.Value);
					}
				}

                //The following two lines set the label text for the total Gold cost of every creature tier and
                //the Total Gold amount stored in the "Gold_Total" variable
                LabelsTotal[i].Text = Convert.ToString(Convert.ToInt32(LabelsGold[i].Text) * NumericUpDowns[i].Value);
                Gold_Total = Gold_Total + Convert.ToInt32(LabelsTotal[i].Text);
                if (chkLimitPopulation.Checked == true) NumericUpDowns[i].Maximum = Convert.ToInt32(LabelsPopulation[i].Text);
            }


            //Dwelling_Checked();
            //if (Dwelling_Tier_A != 0 && Dwelling_CheckBox1.Checked == true) lblPopulation_Tier[Dwelling_Tier_A].Text = Convert.ToString(Convert.ToInt32(lblPopulation_Tier[Dwelling_Tier_A].Text) + (Dwelling[Faction, 0] * (int)udCreatures_WeekLimit.Value));
            //if (Dwelling_Tier_B != 0 && (Dwelling_CheckBox2.Checked == true || (Convert.ToString(Factions.SelectedItem) == "Dungeon" && Dwelling_CheckBox1.Checked == true))) lblPopulation_Tier[Dwelling_Tier_B].Text = Convert.ToString(Convert.ToInt32(lblPopulation_Tier[Dwelling_Tier_B].Text) + (Dwelling[Faction, 1] * (int)udCreatures_WeekLimit.Value));
            
            //The Total Gold text label is updated with the appropriate value and the check for the gold constraint is performed
            Label_GoldTotal.Text = Convert.ToString(Gold_Total);
			Gold_Maximum();
            BonusDwellingTier1 = 0;
            BonusDwellingTier2 = 0;
        }

        //This method sets which creature tiers should benefit from extra population, should the bonus dwelling be checked
        //public void Dwelling_Checked()
        //{
        //    switch (Convert.ToString(Factions.SelectedItem))
        //    {
        //        case "Academy":
        //            Dwelling_Tier_A = 5;
        //            Dwelling_Tier_B = 0;
        //            Faction = 1;
        //            break;
        //        case "Dungeon":
        //            Dwelling_Tier_A = 2;
        //            Dwelling_Tier_B = 3;
        //            Faction = 2;
        //            if (Dwelling_CheckBox1.Checked == false)
        //            {
        //                Dungeon_Ritual_UpDown1.Value = 0;
        //                Dungeon_Ritual_UpDown2.Value = 0;
        //            }
        //            Dwelling[Faction, 0] = (int)Dungeon_Ritual_UpDown1.Value;
        //            Dwelling[Faction, 1] = (int)Dungeon_Ritual_UpDown2.Value;
        //            break;
        //        case "Fortress":
        //            Dwelling_Tier_A = 4;
        //            Dwelling_Tier_B = 5;
        //            Faction = 3;
        //            break;
        //        case "Haven":
        //            Dwelling_Tier_A = 1;
        //            Dwelling_Tier_B = 0;
        //            Faction = 4;
        //            break;
        //        case "Inferno":
        //            Dwelling_Tier_A = 2;
        //            Dwelling_Tier_B = 5;
        //            Faction = 5;
        //            break;
        //        case "Necropolis":
        //            Dwelling_Tier_A = 1;
        //            Dwelling_Tier_B = 7;
        //            Faction = 6;
        //            break;
        //        case "Stronghold":
        //            Dwelling_Tier_A = 1;
        //            Dwelling_Tier_B = 0;
        //            Faction = 7;
        //            break;
        //        case "Sylvan":
        //            Dwelling_Tier_A = 1;
        //            Dwelling_Tier_B = 6;
        //            Faction = 8;
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //This method checks whether the set maximum of gold has been exceeded.
        public void Gold_Maximum()
        {
            if ((Convert.ToInt32(Label_GoldTotal.Text) > udLimitGold.Value) && (chkLimitGold.Checked == true))
            {
                lblLimitGoldExceeded.Visible = true;
            }
            else
            {
                lblLimitGoldExceeded.Visible = false;
            }
        }

        #region Current_Methods
        //These methods change the creature pictures and bonus dwelling text for each faction
        public void Current_Academy()
        {
            pbTier1_01.Image = Properties.Resources.Academy_Tier1_01;
            pbTier1_02.Image = Properties.Resources.Academy_Tier1_02;
            pbTier1_03.Image = Properties.Resources.Academy_Tier1_03;
            pbTier2_01.Image = Properties.Resources.Academy_Tier2_01;
            pbTier2_02.Image = Properties.Resources.Academy_Tier2_02;
            pbTier2_03.Image = Properties.Resources.Academy_Tier2_03;
            pbTier3_01.Image = Properties.Resources.Academy_Tier3_01;
            pbTier3_02.Image = Properties.Resources.Academy_Tier3_02;
            pbTier3_03.Image = Properties.Resources.Academy_Tier3_03;
            pbTier4_01.Image = Properties.Resources.Academy_Tier4_01;
            pbTier4_02.Image = Properties.Resources.Academy_Tier4_02;
            pbTier4_03.Image = Properties.Resources.Academy_Tier4_03;
            pbTier5_01.Image = Properties.Resources.Academy_Tier5_01;
            pbTier5_02.Image = Properties.Resources.Academy_Tier5_02;
            pbTier5_03.Image = Properties.Resources.Academy_Tier5_03;
            pbTier6_01.Image = Properties.Resources.Academy_Tier6_01;
            pbTier6_02.Image = Properties.Resources.Academy_Tier6_02;
            pbTier6_03.Image = Properties.Resources.Academy_Tier6_03;
            pbTier7_01.Image = Properties.Resources.Academy_Tier7_01;
            pbTier7_02.Image = Properties.Resources.Academy_Tier7_02;
            pbTier7_03.Image = Properties.Resources.Academy_Tier7_03;
            chkDwelling1.Text = "Treasure Cave";
            chkDwelling1.Visible = true;
            chkDwelling2.Text = "";
            chkDwelling2.Visible = false;
        }
        public void Current_Dungeon()
        {
            pbTier1_01.Image = Properties.Resources.Dungeon_Tier1_01;
            pbTier1_02.Image = Properties.Resources.Dungeon_Tier1_02;
            pbTier1_03.Image = Properties.Resources.Dungeon_Tier1_03;
            pbTier2_01.Image = Properties.Resources.Dungeon_Tier2_01;
            pbTier2_02.Image = Properties.Resources.Dungeon_Tier2_02;
            pbTier2_03.Image = Properties.Resources.Dungeon_Tier2_03;
            pbTier3_01.Image = Properties.Resources.Dungeon_Tier3_01;
            pbTier3_02.Image = Properties.Resources.Dungeon_Tier3_02;
            pbTier3_03.Image = Properties.Resources.Dungeon_Tier3_03;
            pbTier4_01.Image = Properties.Resources.Dungeon_Tier4_01;
            pbTier4_02.Image = Properties.Resources.Dungeon_Tier4_02;
            pbTier4_03.Image = Properties.Resources.Dungeon_Tier4_03;
            pbTier5_01.Image = Properties.Resources.Dungeon_Tier5_01;
            pbTier5_02.Image = Properties.Resources.Dungeon_Tier5_02;
            pbTier5_03.Image = Properties.Resources.Dungeon_Tier5_03;
            pbTier6_01.Image = Properties.Resources.Dungeon_Tier6_01;
            pbTier6_02.Image = Properties.Resources.Dungeon_Tier6_02;
            pbTier6_03.Image = Properties.Resources.Dungeon_Tier6_03;
            pbTier7_01.Image = Properties.Resources.Dungeon_Tier7_01;
            pbTier7_02.Image = Properties.Resources.Dungeon_Tier7_02;
            pbTier7_03.Image = Properties.Resources.Dungeon_Tier7_03;
            chkDwelling1.Text = "Ritual Pit";
            chkDwelling1.Visible = true;
            chkDwelling2.Text = "";
            chkDwelling2.Visible = false;
        }
        public void Current_Fortress()
        {
            pbTier1_01.Image = Properties.Resources.Fortress_Tier1_01;
            pbTier1_02.Image = Properties.Resources.Fortress_Tier1_02;
            pbTier1_03.Image = Properties.Resources.Fortress_Tier1_03;
            pbTier2_01.Image = Properties.Resources.Fortress_Tier2_01;
            pbTier2_02.Image = Properties.Resources.Fortress_Tier2_02;
            pbTier2_03.Image = Properties.Resources.Fortress_Tier2_03;
            pbTier3_01.Image = Properties.Resources.Fortress_Tier3_01;
            pbTier3_02.Image = Properties.Resources.Fortress_Tier3_02;
            pbTier3_03.Image = Properties.Resources.Fortress_Tier3_03;
            pbTier4_01.Image = Properties.Resources.Fortress_Tier4_01;
            pbTier4_02.Image = Properties.Resources.Fortress_Tier4_02;
            pbTier4_03.Image = Properties.Resources.Fortress_Tier4_03;
            pbTier5_01.Image = Properties.Resources.Fortress_Tier5_01;
            pbTier5_02.Image = Properties.Resources.Fortress_Tier5_02;
            pbTier5_03.Image = Properties.Resources.Fortress_Tier5_03;
            pbTier6_01.Image = Properties.Resources.Fortress_Tier6_01;
            pbTier6_02.Image = Properties.Resources.Fortress_Tier6_02;
            pbTier6_03.Image = Properties.Resources.Fortress_Tier6_03;
            pbTier7_01.Image = Properties.Resources.Fortress_Tier7_01;
            pbTier7_02.Image = Properties.Resources.Fortress_Tier7_02;
            pbTier7_03.Image = Properties.Resources.Fortress_Tier7_03;
            chkDwelling1.Text = "Wrestler's Arena";
            chkDwelling1.Visible = true;
            chkDwelling2.Text = "Runic Sanctuary";
            chkDwelling2.Visible = true;
        }
        public void Current_Haven()
        {
            pbTier1_01.Image = Properties.Resources.Haven_Tier1_01;
            pbTier1_02.Image = Properties.Resources.Haven_Tier1_02;
            pbTier1_03.Image = Properties.Resources.Haven_Tier1_03;
            pbTier2_01.Image = Properties.Resources.Haven_Tier2_01;
            pbTier2_02.Image = Properties.Resources.Haven_Tier2_02;
            pbTier2_03.Image = Properties.Resources.Haven_Tier2_03;
            pbTier3_01.Image = Properties.Resources.Haven_Tier3_01;
            pbTier3_02.Image = Properties.Resources.Haven_Tier3_02;
            pbTier3_03.Image = Properties.Resources.Haven_Tier3_03;
            pbTier4_01.Image = Properties.Resources.Haven_Tier4_01;
            pbTier4_02.Image = Properties.Resources.Haven_Tier4_02;
            pbTier4_03.Image = Properties.Resources.Haven_Tier4_03;
            pbTier5_01.Image = Properties.Resources.Haven_Tier5_01;
            pbTier5_02.Image = Properties.Resources.Haven_Tier5_02;
            pbTier5_03.Image = Properties.Resources.Haven_Tier5_03;
            pbTier6_01.Image = Properties.Resources.Haven_Tier6_01;
            pbTier6_02.Image = Properties.Resources.Haven_Tier6_02;
            pbTier6_03.Image = Properties.Resources.Haven_Tier6_03;
            pbTier7_01.Image = Properties.Resources.Haven_Tier7_01;
            pbTier7_02.Image = Properties.Resources.Haven_Tier7_02;
            pbTier7_03.Image = Properties.Resources.Haven_Tier7_03;
            chkDwelling1.Text = "Farms";
            chkDwelling1.Visible = true;
            chkDwelling2.Text = "";
            chkDwelling2.Visible = false;
        }
        public void Current_Inferno()
        {
            pbTier1_01.Image = Properties.Resources.Inferno_Tier1_01;
            pbTier1_02.Image = Properties.Resources.Inferno_Tier1_02;
            pbTier1_03.Image = Properties.Resources.Inferno_Tier1_03;
            pbTier2_01.Image = Properties.Resources.Inferno_Tier2_01;
            pbTier2_02.Image = Properties.Resources.Inferno_Tier2_02;
            pbTier2_03.Image = Properties.Resources.Inferno_Tier2_03;
            pbTier3_01.Image = Properties.Resources.Inferno_Tier3_01;
            pbTier3_02.Image = Properties.Resources.Inferno_Tier3_02;
            pbTier3_03.Image = Properties.Resources.Inferno_Tier3_03;
            pbTier4_01.Image = Properties.Resources.Inferno_Tier4_01;
            pbTier4_02.Image = Properties.Resources.Inferno_Tier4_02;
            pbTier4_03.Image = Properties.Resources.Inferno_Tier4_03;
            pbTier5_01.Image = Properties.Resources.Inferno_Tier5_01;
            pbTier5_02.Image = Properties.Resources.Inferno_Tier5_02;
            pbTier5_03.Image = Properties.Resources.Inferno_Tier5_03;
            pbTier6_01.Image = Properties.Resources.Inferno_Tier6_01;
            pbTier6_02.Image = Properties.Resources.Inferno_Tier6_02;
            pbTier6_03.Image = Properties.Resources.Inferno_Tier6_03;
            pbTier7_01.Image = Properties.Resources.Inferno_Tier7_01;
            pbTier7_02.Image = Properties.Resources.Inferno_Tier7_02;
            pbTier7_03.Image = Properties.Resources.Inferno_Tier7_03;
            chkDwelling1.Text = "Spawn of Chaos";
            chkDwelling1.Visible = true;
            chkDwelling2.Text = "Halls of Horror";
            chkDwelling2.Visible = true;
        }
        public void Current_Necropolis()
        {
            pbTier1_01.Image = Properties.Resources.Necropolis_Tier1_01;
            pbTier1_02.Image = Properties.Resources.Necropolis_Tier1_02;
            pbTier1_03.Image = Properties.Resources.Necropolis_Tier1_03;
            pbTier2_01.Image = Properties.Resources.Necropolis_Tier2_01;
            pbTier2_02.Image = Properties.Resources.Necropolis_Tier2_02;
            pbTier2_03.Image = Properties.Resources.Necropolis_Tier2_03;
            pbTier3_01.Image = Properties.Resources.Necropolis_Tier3_01;
            pbTier3_02.Image = Properties.Resources.Necropolis_Tier3_02;
            pbTier3_03.Image = Properties.Resources.Necropolis_Tier3_03;
            pbTier4_01.Image = Properties.Resources.Necropolis_Tier4_01;
            pbTier4_02.Image = Properties.Resources.Necropolis_Tier4_02;
            pbTier4_03.Image = Properties.Resources.Necropolis_Tier4_03;
            pbTier5_01.Image = Properties.Resources.Necropolis_Tier5_01;
            pbTier5_02.Image = Properties.Resources.Necropolis_Tier5_02;
            pbTier5_03.Image = Properties.Resources.Necropolis_Tier5_03;
            pbTier6_01.Image = Properties.Resources.Necropolis_Tier6_01;
            pbTier6_02.Image = Properties.Resources.Necropolis_Tier6_02;
            pbTier6_03.Image = Properties.Resources.Necropolis_Tier6_03;
            pbTier7_01.Image = Properties.Resources.Necropolis_Tier7_01;
            pbTier7_02.Image = Properties.Resources.Necropolis_Tier7_02;
            pbTier7_03.Image = Properties.Resources.Necropolis_Tier7_03;
            chkDwelling1.Text = "Unearthed Graves";
            chkDwelling1.Visible = true;
            chkDwelling2.Text = "Dragon Tombstone";
            chkDwelling2.Visible = true;
        }
        public void Current_Stronghold()
        {
            pbTier1_01.Image = Properties.Resources.Stronghold_Tier1_01;
            pbTier1_02.Image = Properties.Resources.Stronghold_Tier1_02;
            pbTier1_03.Image = Properties.Resources.Stronghold_Tier1_03;
            pbTier2_01.Image = Properties.Resources.Stronghold_Tier2_01;
            pbTier2_02.Image = Properties.Resources.Stronghold_Tier2_02;
            pbTier2_03.Image = Properties.Resources.Stronghold_Tier2_03;
            pbTier3_01.Image = Properties.Resources.Stronghold_Tier3_01;
            pbTier3_02.Image = Properties.Resources.Stronghold_Tier3_02;
            pbTier3_03.Image = Properties.Resources.Stronghold_Tier3_03;
            pbTier4_01.Image = Properties.Resources.Stronghold_Tier4_01;
            pbTier4_02.Image = Properties.Resources.Stronghold_Tier4_02;
            pbTier4_03.Image = Properties.Resources.Stronghold_Tier4_03;
            pbTier5_01.Image = Properties.Resources.Stronghold_Tier5_01;
            pbTier5_02.Image = Properties.Resources.Stronghold_Tier5_02;
            pbTier5_03.Image = Properties.Resources.Stronghold_Tier5_03;
            pbTier6_01.Image = Properties.Resources.Stronghold_Tier6_01;
            pbTier6_02.Image = Properties.Resources.Stronghold_Tier6_02;
            pbTier6_03.Image = Properties.Resources.Stronghold_Tier6_03;
            pbTier7_01.Image = Properties.Resources.Stronghold_Tier7_01;
            pbTier7_02.Image = Properties.Resources.Stronghold_Tier7_02;
            pbTier7_03.Image = Properties.Resources.Stronghold_Tier7_03;
            chkDwelling1.Text = "Garbage Pile";
            chkDwelling1.Visible = true;
            chkDwelling2.Text = "";
            chkDwelling2.Visible = false;
        }
        public void Current_Sylvan()
        {
            pbTier1_01.Image = Properties.Resources.Sylvan_Tier1_01;
            pbTier1_02.Image = Properties.Resources.Sylvan_Tier1_02;
            pbTier1_03.Image = Properties.Resources.Sylvan_Tier1_03;
            pbTier2_01.Image = Properties.Resources.Sylvan_Tier2_01;
            pbTier2_02.Image = Properties.Resources.Sylvan_Tier2_02;
            pbTier2_03.Image = Properties.Resources.Sylvan_Tier2_03;
            pbTier3_01.Image = Properties.Resources.Sylvan_Tier3_01;
            pbTier3_02.Image = Properties.Resources.Sylvan_Tier3_02;
            pbTier3_03.Image = Properties.Resources.Sylvan_Tier3_03;
            pbTier4_01.Image = Properties.Resources.Sylvan_Tier4_01;
            pbTier4_02.Image = Properties.Resources.Sylvan_Tier4_02;
            pbTier4_03.Image = Properties.Resources.Sylvan_Tier4_03;
            pbTier5_01.Image = Properties.Resources.Sylvan_Tier5_01;
            pbTier5_02.Image = Properties.Resources.Sylvan_Tier5_02;
            pbTier5_03.Image = Properties.Resources.Sylvan_Tier5_03;
            pbTier6_01.Image = Properties.Resources.Sylvan_Tier6_01;
            pbTier6_02.Image = Properties.Resources.Sylvan_Tier6_02;
            pbTier6_03.Image = Properties.Resources.Sylvan_Tier6_03;
            pbTier7_01.Image = Properties.Resources.Sylvan_Tier7_01;
            pbTier7_02.Image = Properties.Resources.Sylvan_Tier7_02;
            pbTier7_03.Image = Properties.Resources.Sylvan_Tier7_03;
            chkDwelling1.Text = "Blooming Grove";
            chkDwelling1.Visible = true;
            chkDwelling2.Text = "Treant Saplings";
            chkDwelling2.Visible = true;
        }
        #endregion

        private void UpDown(object sender, EventArgs e)
        {
            UI_Update();
        }
        private void Citadel_Check_Change(object sender, EventArgs e)
        {
            if (chkCastle.Checked == true && chkCitadel.Checked == false)
            {
                chkCastle.Checked = false;
            }
            UI_Update();
        }
        private void Castle_Check_Change(object sender, EventArgs e)
        {
            if (chkCastle.Checked == true && chkCitadel.Checked == false)
            {
                chkCitadel.Checked = true;
            }
            UI_Update();
        }
        private void Factions_SelectedIndexChanged(object sender, EventArgs e)
        {
            UI_Update();
            ComboBox Clicked = (ComboBox)sender;
            chkDwelling1.Checked = false;
            chkDwelling2.Checked = false;
            switch (Convert.ToString(Clicked.SelectedItem))
            {
                case "Haven":
                    Current_Haven();
                    break;
                case "Necropolis":
                    Current_Necropolis();
                    break;
                case "Sylvan":
                    Current_Sylvan();
                    break;
                case "Stronghold":
                    Current_Stronghold();
                    break;
                case "Fortress":
                    Current_Fortress();
                    break;
                case "Dungeon":
                    Current_Dungeon();
                    break;
                case "Inferno":
                    Current_Inferno();
                    break;
                case "Academy":
                    Current_Academy();
                    break;
                default:
                    break;
            }
        }
        private void Check_WeekLimit_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLimitPopulation.Checked == true)
            {
                udLimitPopulation.Enabled = true;
            }
            else
            {
                udLimitPopulation.Enabled = false;
                udLimitPopulation.Value = 1;
            }
            UI_Update();
        }
        private void Check_GoldLimit_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLimitGold.Checked == true)
            {
                udLimitGold.Enabled = true;
                Gold_Maximum();
            }
            else
            {
                udLimitGold.Enabled = false;
                lblLimitGoldExceeded.Visible = false;
            }
        }
        private void udCreatures_GoldLimit_ValueChanged(object sender, EventArgs e)
        {
            Gold_Maximum();
        }
        
        //The following methods move the "frame" image to the image control that raised the event
        //and set the "Upg" boolean to true or false, depending on which image was clicked.
        //This could be achieved with a separate method for each PictureBox (unprofessional) a more practical
        //variant would be to have two methods: one for setting the boolean to "true", another for "false"
        //but I have chosen to completely automate and optimize the process.
        private void Picture_Click(object sender, EventArgs e)
        {
            //We first assign the clicked control and get its name.
            Control Clicked = (Control)sender;
            string Control_Name = Clicked.Name;

            //We locate the string index of the word "Tier" in order to get the number of its tier and order,
            //since all PictureBoxes are named in the fashion "pbTierX_0Y", where X is the corresponding
            //creature's tier and Y is the order of that PictureBox, either 1, 2 or 3, with 2 and 3 being
            //upgraded versions of 1.
            int String_Tier_Index = Control_Name.IndexOf("Tier") + 4;
            int Control_Tier = Convert.ToInt32(Control_Name.Substring(String_Tier_Index, 1));
            int Control_Order = Convert.ToInt32(Control_Name.Substring(String_Tier_Index + 3, 1));

            //We find the Frame of the clicked PictureBox's row, which corresponds to the clicked PictureBox
            //tier number.
            string Frame_Name = "Frame" + Control_Tier;
            PictureBox Frame = (PictureBox)Controls.Find(Frame_Name, true).FirstOrDefault();
            Frame.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);

            //We set the "Upg" boolean of the corresponding tier to "false" if it is the first (leftmost) picture
            //or "true" if not.
            if (Control_Order == 1)
            {
                IsUpgraded[Control_Tier] = false;
            }
            else
            {
                IsUpgraded[Control_Tier] = true;
            }

            UI_Update();
        }
    }
}