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
    public partial class Form1 : Form
    {
        public int Dwelling_Tier_A, Dwelling_Tier_B;//, Faction;       //The "Dwelling Tier" variables hold the tier of the
                                                    //creature the selected faction's bonus dwelling is for
                                                    /*
                                                     * Introduction:
                                                     * =============
                                                     * In the game for which this calculator is made, Heroes of Might and Magic 5, there are a number of factions,
                                                     * each capable of buying different creatures. The creatures of each faction are unique, but they all share the
                                                     * same ranking system, being divided into "tiers" that range from 1 to 7.
                                                     * - Each tier of creatures has three variations: one basic and two upgraded ones.
                                                     * - All creatures have a weekly growth that is the same for all variations of a faction's tier.
                                                     * - Each creature costs gold to purchase. The upgraded variations cost the same, the basic one - less.
                                                    */
        public Label[] LabelsGold = new Label[8];                    //Array to hold all the 'creature gold cost' labels of the form
        public Label[] LabelsPopulation = new Label[8];                     //Array to hold all the 'creature population' labels of the form
        public Label[] LabelsTotal = new Label[8];                   //Array to hold all the 'creature total gold cost' labels of the form
        public NumericUpDown[] NumericUpDowns = new NumericUpDown[8];  //Array to hold all the form's NumericUpDowns
        public bool[] IsUpgraded = new bool[8];                            //Array of booleans that designate if an upgraded variant is selected or not for each creature tier

        //A list of all the factions in the game
        List<string> FactionList = new List<string> { "Academy", "Dungeon", "Fortress", "Haven", "Inferno", "Necropolis", "Stronghold", "Sylvan" };

        public Form1()
        {
            FactionBaseCost.Add("Academy", new int[] { 0, 22, 45, 90, 250, 480, 1400, 3500 });
            FactionUpgCost.Add("Academy", new int[] { 0, 35, 70, 130, 340, 700, 1770, 4700 });
            FactionPopulation.Add("Academy", new int[] { 0, 20, 14, 9, 5, 3, 2, 1 });

            FactionBaseCost.Add("Dungeon", new int[] { 0, 60, 125, 140, 300, 550, 1400, 3000 });
            FactionUpgCost.Add("Dungeon", new int[] { 0, 100, 175, 200, 450, 800, 1700, 3700 });
            FactionPopulation.Add("Dungeon", new int[] { 0, 7, 5, 6, 4, 3, 2, 1 });

            FactionBaseCost.Add("Fortress", new int[] { 0, 24, 45, 130, 160, 470, 1300, 2700 });
            FactionUpgCost.Add("Fortress", new int[] { 0, 40, 65, 185, 220, 700, 1700, 3400 });
            FactionPopulation.Add("Fortress", new int[] { 0, 18, 14, 7, 6, 3, 2, 1 });

            FactionBaseCost.Add("Haven", new int[] { 0, 15, 50, 85, 250, 600, 1300, 2800 });
            FactionUpgCost.Add("Haven", new int[] { 0, 25, 80, 130, 370, 850, 1700, 3500 });
            FactionPopulation.Add("Haven", new int[] { 0, 22, 12, 10, 5, 3, 2, 1 });

            FactionBaseCost.Add("Inferno", new int[] { 0, 25, 40, 110, 240, 550, 1400, 2666 });
            FactionUpgCost.Add("Inferno", new int[] { 0, 45, 60, 160, 350, 780, 1666, 3666 });
            FactionPopulation.Add("Inferno", new int[] { 0, 16, 15, 8, 5, 3, 2, 1 });

            FactionBaseCost.Add("Necropolis", new int[] { 0, 19, 40, 100, 250, 620, 1400, 1600 });
            FactionUpgCost.Add("Necropolis", new int[] { 0, 30, 60, 140, 380, 850, 1700, 1900 });
            FactionPopulation.Add("Necropolis", new int[] { 0, 20, 15, 9, 5, 3, 2, 1 });

            FactionBaseCost.Add("Stronghold", new int[] { 0, 10, 50, 80, 260, 350, 1250, 2900 });
            FactionUpgCost.Add("Stronghold", new int[] { 0, 20, 70, 120, 360, 500, 1600, 3450 });
            FactionPopulation.Add("Stronghold", new int[] { 0, 25, 14, 11, 5, 5, 2, 1 });

            FactionBaseCost.Add("Sylvan", new int[] { 0, 35, 70, 120, 320, 630, 1100, 2500 });
            FactionUpgCost.Add("Sylvan", new int[] { 0, 55, 120, 190, 440, 900, 1400, 3400 });
            FactionPopulation.Add("Sylvan", new int[] { 0, 10, 9, 7, 4, 3, 2, 1 });

            foreach (var faction in FactionList)
            {
                for (int i = 1; i < 8; i++)
                {
                    fac.Units[i].BaseGoldCost = FactionBaseCost[faction][i];
                    fac.Units[i].UpgGoldCost = FactionUpgCost[faction][i];
                    fac.Units[i].Population = FactionPopulation[faction][i];
                }
            }

            InitializeComponent();
            //We bind the labels showing each creature's cost, it's weekly population and the total cost of the chosen
            //amount and the numeric updowns to arrays, so we can access them easily.
            LabelsPopulation[0] = null;
            LabelsGold[0] = null;
            LabelsTotal[0] = null;
            NumericUpDowns[0] = null;
            for (int i = 1; i < 8; i++)
            {
                LabelsPopulation[i] = (Label)Controls.Find("Pop_Tier" + i, true).FirstOrDefault();
                LabelsGold[i] = (Label)Controls.Find("Gold_Tier" + i, true).FirstOrDefault();
                LabelsTotal[i] = (Label)Controls.Find("Total_Tier" + i, true).FirstOrDefault();
                NumericUpDowns[i] = (NumericUpDown)Controls.Find("UpDown_Tier" + i, true).FirstOrDefault();
            }

            #region to be deleted
            //We assign data for all the factions to the array we will be working with.
            ////for (int i = 0; i < 8; i++)
            //{
            //    Town_Tier_Base_Gold[0, i] = Academy_Base_Gold_Tier[i];
            //    Town_Tier_Upg_Gold[0, i] = Academy_Upg_Gold_Tier[i];
            //    Town_Tier_Pop[0, i] = Academy_Pop_Tier[i];

            //    Town_Tier_Base_Gold[1, i] = Dungeon_Base_Gold_Tier[i];
            //    Town_Tier_Upg_Gold[1, i] = Dungeon_Upg_Gold_Tier[i];
            //    Town_Tier_Pop[1, i] = Dungeon_Pop_Tier[i];

            //    Town_Tier_Base_Gold[2, i] = Fortress_Base_Gold_Tier[i];
            //    Town_Tier_Upg_Gold[2, i] = Fortress_Upg_Gold_Tier[i];
            //    Town_Tier_Pop[2, i] = Fortress_Pop_Tier[i];

            //    Town_Tier_Base_Gold[3, i] = Haven_Base_Gold_Tier[i];
            //    Town_Tier_Upg_Gold[3, i] = Haven_Upg_Gold_Tier[i];
            //    Town_Tier_Pop[3, i] = Haven_Pop_Tier[i];

            //    Town_Tier_Base_Gold[4, i] = Inferno_Base_Gold_Tier[i];
            //    Town_Tier_Upg_Gold[4, i] = Inferno_Upg_Gold_Tier[i];
            //    Town_Tier_Pop[4, i] = Inferno_Pop_Tier[i];

            //    Town_Tier_Base_Gold[5, i] = Necropolis_Base_Gold_Tier[i];
            //    Town_Tier_Upg_Gold[5, i] = Necropolis_Upg_Gold_Tier[i];
            //    Town_Tier_Pop[5, i] = Necropolis_Pop_Tier[i];

            //    Town_Tier_Base_Gold[6, i] = Stronghold_Base_Gold_Tier[i];
            //    Town_Tier_Upg_Gold[6, i] = Stronghold_Upg_Gold_Tier[i];
            //    Town_Tier_Pop[6, i] = Stronghold_Pop_Tier[i];

            //    Town_Tier_Base_Gold[7, i] = Sylvan_Base_Gold_Tier[i];
            //    Town_Tier_Upg_Gold[7, i] = Sylvan_Upg_Gold_Tier[i];
            //    Town_Tier_Pop[7, i] = Sylvan_Pop_Tier[i];
            //}
            #endregion to be deleted
            Factions.SelectedItem = "Academy";
            UI_Update();
        }

        //This method updates all UI elements
        public void UI_Update()
        {
            //Since the "Dungeon" faction has a set of specific controls we check if that is the selected faction
            //and if the corresponding checkbox is checked. If one of these conditions is not satisfied
            //the controls are not visible.
            if (Convert.ToString(Factions.SelectedItem) == "Dungeon" && Dwelling_CheckBox1.Checked == true)
            {
                Dungeon_Ritual_UpDown1.Visible = true;
                Dungeon_Ritual_UpDown2.Visible = true;
                Dungeon_Label_Blood.Visible = true;
                Dungeon_Label_Minotaur.Visible = true;
            }
            else
            {
                Dungeon_Ritual_UpDown1.Visible = false;
                Dungeon_Ritual_UpDown2.Visible = false;
                Dungeon_Label_Blood.Visible = false;
                Dungeon_Label_Minotaur.Visible = false;
            }

            //Sets label texts and images to the currently selected faction
            //================================================================================
            //First determine which faction is selected and from that access the appropriate index/values in the data array

            string FactionName = (string)Factions.SelectedItem;
            int Faction_Index = Town[(string)Factions.SelectedItem];
            int Gold_Total = 0;

            //This loop sets the labels for Creature Gold cost, Creature Population,
            //Cost of selected creatures and Total Gold.
            for (int i = 1; i < 8; i++)
            {
                //Whenever the image of an upgraded creature is clicked, the boolean variable "Upg" is set to "true"
                //this "if" checkes whether "Upg" is true and changes the gold cost of the creature appropriately
                if (IsUpgraded[i] == true)
                {
                    LabelsGold[i].Text = FactionUpgCost[FactionName][i].ToString();
                }
                else
                {
                    LabelsGold[i].Text = FactionBaseCost[FactionName][i].ToString();
                }

                //This "if" sets the creature populations and UpDown controls' maximums in accordance
                //to a checked Castle or Citadel
				if (Check_Castle.Checked == true)
				{
					LabelsPopulation[i].Text = Convert.ToString(Town_Tier_Pop[Faction_Index, i] * (int)UpDown_WeekLimit.Value * 2);
				}
				else
				{
					if (Check_Citadel.Checked == true)
					{
						LabelsPopulation[i].Text = Convert.ToString(Math.Truncate(Town_Tier_Pop[Faction_Index, i] * (int)UpDown_WeekLimit.Value * (decimal)1.5));
					}
					else
					{
						LabelsPopulation[i].Text = Convert.ToString(Town_Tier_Pop[Faction_Index, i] * (int)UpDown_WeekLimit.Value);
					}
				}

                //The following two lines set the label text for the total Gold cost of every creature tier and
                //the Total Gold amount stored in the "Gold_Total" variable
                LabelsTotal[i].Text = Convert.ToString(Convert.ToInt32(LabelsGold[i].Text) * NumericUpDowns[i].Value);
                Gold_Total = Gold_Total + Convert.ToInt32(LabelsTotal[i].Text);
                if (Check_WeekLimit.Checked == true) NumericUpDowns[i].Maximum = Convert.ToInt32(LabelsPopulation[i].Text);
            }


            //Dwelling_Checked();
            //if (Dwelling_Tier_A != 0 && Dwelling_CheckBox1.Checked == true) Pop_Tier[Dwelling_Tier_A].Text = Convert.ToString(Convert.ToInt32(Pop_Tier[Dwelling_Tier_A].Text) + (Dwelling[Faction, 0] * (int)UpDown_WeekLimit.Value));
            //if (Dwelling_Tier_B != 0 && (Dwelling_CheckBox2.Checked == true || (Convert.ToString(Factions.SelectedItem) == "Dungeon" && Dwelling_CheckBox1.Checked == true))) Pop_Tier[Dwelling_Tier_B].Text = Convert.ToString(Convert.ToInt32(Pop_Tier[Dwelling_Tier_B].Text) + (Dwelling[Faction, 1] * (int)UpDown_WeekLimit.Value));
            
            //The Total Gold text label is updated with the appropriate value and the check for the gold constraint is performed
            Label_GoldTotal.Text = Convert.ToString(Gold_Total);
			Gold_Maximum();
            Dwelling_Tier_A = 0;
            Dwelling_Tier_B = 0;
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
            if ((Convert.ToInt32(Label_GoldTotal.Text) > UpDown_GoldLimit.Value) && (Check_GoldLimit.Checked == true))
            {
                Label_GoldLimitExceeded.Visible = true;
            }
            else
            {
                Label_GoldLimitExceeded.Visible = false;
            }
        }

        #region Current_Methods
        //These methods change the creature pictures and bonus dwelling text for each faction
        public void Current_Academy()
        {
            Picture_Tier1_01.Image = Properties.Resources.Academy_Tier1_01;
            Picture_Tier1_02.Image = Properties.Resources.Academy_Tier1_02;
            Picture_Tier1_03.Image = Properties.Resources.Academy_Tier1_03;
            Picture_Tier2_01.Image = Properties.Resources.Academy_Tier2_01;
            Picture_Tier2_02.Image = Properties.Resources.Academy_Tier2_02;
            Picture_Tier2_03.Image = Properties.Resources.Academy_Tier2_03;
            Picture_Tier3_01.Image = Properties.Resources.Academy_Tier3_01;
            Picture_Tier3_02.Image = Properties.Resources.Academy_Tier3_02;
            Picture_Tier3_03.Image = Properties.Resources.Academy_Tier3_03;
            Picture_Tier4_01.Image = Properties.Resources.Academy_Tier4_01;
            Picture_Tier4_02.Image = Properties.Resources.Academy_Tier4_02;
            Picture_Tier4_03.Image = Properties.Resources.Academy_Tier4_03;
            Picture_Tier5_01.Image = Properties.Resources.Academy_Tier5_01;
            Picture_Tier5_02.Image = Properties.Resources.Academy_Tier5_02;
            Picture_Tier5_03.Image = Properties.Resources.Academy_Tier5_03;
            Picture_Tier6_01.Image = Properties.Resources.Academy_Tier6_01;
            Picture_Tier6_02.Image = Properties.Resources.Academy_Tier6_02;
            Picture_Tier6_03.Image = Properties.Resources.Academy_Tier6_03;
            Picture_Tier7_01.Image = Properties.Resources.Academy_Tier7_01;
            Picture_Tier7_02.Image = Properties.Resources.Academy_Tier7_02;
            Picture_Tier7_03.Image = Properties.Resources.Academy_Tier7_03;
            Dwelling_CheckBox1.Text = "Treasure Cave";
            Dwelling_CheckBox1.Visible = true;
            Dwelling_CheckBox2.Text = "";
            Dwelling_CheckBox2.Visible = false;
        }
        public void Current_Dungeon()
        {
            Picture_Tier1_01.Image = Properties.Resources.Dungeon_Tier1_01;
            Picture_Tier1_02.Image = Properties.Resources.Dungeon_Tier1_02;
            Picture_Tier1_03.Image = Properties.Resources.Dungeon_Tier1_03;
            Picture_Tier2_01.Image = Properties.Resources.Dungeon_Tier2_01;
            Picture_Tier2_02.Image = Properties.Resources.Dungeon_Tier2_02;
            Picture_Tier2_03.Image = Properties.Resources.Dungeon_Tier2_03;
            Picture_Tier3_01.Image = Properties.Resources.Dungeon_Tier3_01;
            Picture_Tier3_02.Image = Properties.Resources.Dungeon_Tier3_02;
            Picture_Tier3_03.Image = Properties.Resources.Dungeon_Tier3_03;
            Picture_Tier4_01.Image = Properties.Resources.Dungeon_Tier4_01;
            Picture_Tier4_02.Image = Properties.Resources.Dungeon_Tier4_02;
            Picture_Tier4_03.Image = Properties.Resources.Dungeon_Tier4_03;
            Picture_Tier5_01.Image = Properties.Resources.Dungeon_Tier5_01;
            Picture_Tier5_02.Image = Properties.Resources.Dungeon_Tier5_02;
            Picture_Tier5_03.Image = Properties.Resources.Dungeon_Tier5_03;
            Picture_Tier6_01.Image = Properties.Resources.Dungeon_Tier6_01;
            Picture_Tier6_02.Image = Properties.Resources.Dungeon_Tier6_02;
            Picture_Tier6_03.Image = Properties.Resources.Dungeon_Tier6_03;
            Picture_Tier7_01.Image = Properties.Resources.Dungeon_Tier7_01;
            Picture_Tier7_02.Image = Properties.Resources.Dungeon_Tier7_02;
            Picture_Tier7_03.Image = Properties.Resources.Dungeon_Tier7_03;
            Dwelling_CheckBox1.Text = "Ritual Pit";
            Dwelling_CheckBox1.Visible = true;
            Dwelling_CheckBox2.Text = "";
            Dwelling_CheckBox2.Visible = false;
        }
        public void Current_Fortress()
        {
            Picture_Tier1_01.Image = Properties.Resources.Fortress_Tier1_01;
            Picture_Tier1_02.Image = Properties.Resources.Fortress_Tier1_02;
            Picture_Tier1_03.Image = Properties.Resources.Fortress_Tier1_03;
            Picture_Tier2_01.Image = Properties.Resources.Fortress_Tier2_01;
            Picture_Tier2_02.Image = Properties.Resources.Fortress_Tier2_02;
            Picture_Tier2_03.Image = Properties.Resources.Fortress_Tier2_03;
            Picture_Tier3_01.Image = Properties.Resources.Fortress_Tier3_01;
            Picture_Tier3_02.Image = Properties.Resources.Fortress_Tier3_02;
            Picture_Tier3_03.Image = Properties.Resources.Fortress_Tier3_03;
            Picture_Tier4_01.Image = Properties.Resources.Fortress_Tier4_01;
            Picture_Tier4_02.Image = Properties.Resources.Fortress_Tier4_02;
            Picture_Tier4_03.Image = Properties.Resources.Fortress_Tier4_03;
            Picture_Tier5_01.Image = Properties.Resources.Fortress_Tier5_01;
            Picture_Tier5_02.Image = Properties.Resources.Fortress_Tier5_02;
            Picture_Tier5_03.Image = Properties.Resources.Fortress_Tier5_03;
            Picture_Tier6_01.Image = Properties.Resources.Fortress_Tier6_01;
            Picture_Tier6_02.Image = Properties.Resources.Fortress_Tier6_02;
            Picture_Tier6_03.Image = Properties.Resources.Fortress_Tier6_03;
            Picture_Tier7_01.Image = Properties.Resources.Fortress_Tier7_01;
            Picture_Tier7_02.Image = Properties.Resources.Fortress_Tier7_02;
            Picture_Tier7_03.Image = Properties.Resources.Fortress_Tier7_03;
            Dwelling_CheckBox1.Text = "Wrestler's Arena";
            Dwelling_CheckBox1.Visible = true;
            Dwelling_CheckBox2.Text = "Runic Sanctuary";
            Dwelling_CheckBox2.Visible = true;
        }
        public void Current_Haven()
        {
            Picture_Tier1_01.Image = Properties.Resources.Haven_Tier1_01;
            Picture_Tier1_02.Image = Properties.Resources.Haven_Tier1_02;
            Picture_Tier1_03.Image = Properties.Resources.Haven_Tier1_03;
            Picture_Tier2_01.Image = Properties.Resources.Haven_Tier2_01;
            Picture_Tier2_02.Image = Properties.Resources.Haven_Tier2_02;
            Picture_Tier2_03.Image = Properties.Resources.Haven_Tier2_03;
            Picture_Tier3_01.Image = Properties.Resources.Haven_Tier3_01;
            Picture_Tier3_02.Image = Properties.Resources.Haven_Tier3_02;
            Picture_Tier3_03.Image = Properties.Resources.Haven_Tier3_03;
            Picture_Tier4_01.Image = Properties.Resources.Haven_Tier4_01;
            Picture_Tier4_02.Image = Properties.Resources.Haven_Tier4_02;
            Picture_Tier4_03.Image = Properties.Resources.Haven_Tier4_03;
            Picture_Tier5_01.Image = Properties.Resources.Haven_Tier5_01;
            Picture_Tier5_02.Image = Properties.Resources.Haven_Tier5_02;
            Picture_Tier5_03.Image = Properties.Resources.Haven_Tier5_03;
            Picture_Tier6_01.Image = Properties.Resources.Haven_Tier6_01;
            Picture_Tier6_02.Image = Properties.Resources.Haven_Tier6_02;
            Picture_Tier6_03.Image = Properties.Resources.Haven_Tier6_03;
            Picture_Tier7_01.Image = Properties.Resources.Haven_Tier7_01;
            Picture_Tier7_02.Image = Properties.Resources.Haven_Tier7_02;
            Picture_Tier7_03.Image = Properties.Resources.Haven_Tier7_03;
            Dwelling_CheckBox1.Text = "Farms";
            Dwelling_CheckBox1.Visible = true;
            Dwelling_CheckBox2.Text = "";
            Dwelling_CheckBox2.Visible = false;
        }
        public void Current_Inferno()
        {
            Picture_Tier1_01.Image = Properties.Resources.Inferno_Tier1_01;
            Picture_Tier1_02.Image = Properties.Resources.Inferno_Tier1_02;
            Picture_Tier1_03.Image = Properties.Resources.Inferno_Tier1_03;
            Picture_Tier2_01.Image = Properties.Resources.Inferno_Tier2_01;
            Picture_Tier2_02.Image = Properties.Resources.Inferno_Tier2_02;
            Picture_Tier2_03.Image = Properties.Resources.Inferno_Tier2_03;
            Picture_Tier3_01.Image = Properties.Resources.Inferno_Tier3_01;
            Picture_Tier3_02.Image = Properties.Resources.Inferno_Tier3_02;
            Picture_Tier3_03.Image = Properties.Resources.Inferno_Tier3_03;
            Picture_Tier4_01.Image = Properties.Resources.Inferno_Tier4_01;
            Picture_Tier4_02.Image = Properties.Resources.Inferno_Tier4_02;
            Picture_Tier4_03.Image = Properties.Resources.Inferno_Tier4_03;
            Picture_Tier5_01.Image = Properties.Resources.Inferno_Tier5_01;
            Picture_Tier5_02.Image = Properties.Resources.Inferno_Tier5_02;
            Picture_Tier5_03.Image = Properties.Resources.Inferno_Tier5_03;
            Picture_Tier6_01.Image = Properties.Resources.Inferno_Tier6_01;
            Picture_Tier6_02.Image = Properties.Resources.Inferno_Tier6_02;
            Picture_Tier6_03.Image = Properties.Resources.Inferno_Tier6_03;
            Picture_Tier7_01.Image = Properties.Resources.Inferno_Tier7_01;
            Picture_Tier7_02.Image = Properties.Resources.Inferno_Tier7_02;
            Picture_Tier7_03.Image = Properties.Resources.Inferno_Tier7_03;
            Dwelling_CheckBox1.Text = "Spawn of Chaos";
            Dwelling_CheckBox1.Visible = true;
            Dwelling_CheckBox2.Text = "Halls of Horror";
            Dwelling_CheckBox2.Visible = true;
        }
        public void Current_Necropolis()
        {
            Picture_Tier1_01.Image = Properties.Resources.Necropolis_Tier1_01;
            Picture_Tier1_02.Image = Properties.Resources.Necropolis_Tier1_02;
            Picture_Tier1_03.Image = Properties.Resources.Necropolis_Tier1_03;
            Picture_Tier2_01.Image = Properties.Resources.Necropolis_Tier2_01;
            Picture_Tier2_02.Image = Properties.Resources.Necropolis_Tier2_02;
            Picture_Tier2_03.Image = Properties.Resources.Necropolis_Tier2_03;
            Picture_Tier3_01.Image = Properties.Resources.Necropolis_Tier3_01;
            Picture_Tier3_02.Image = Properties.Resources.Necropolis_Tier3_02;
            Picture_Tier3_03.Image = Properties.Resources.Necropolis_Tier3_03;
            Picture_Tier4_01.Image = Properties.Resources.Necropolis_Tier4_01;
            Picture_Tier4_02.Image = Properties.Resources.Necropolis_Tier4_02;
            Picture_Tier4_03.Image = Properties.Resources.Necropolis_Tier4_03;
            Picture_Tier5_01.Image = Properties.Resources.Necropolis_Tier5_01;
            Picture_Tier5_02.Image = Properties.Resources.Necropolis_Tier5_02;
            Picture_Tier5_03.Image = Properties.Resources.Necropolis_Tier5_03;
            Picture_Tier6_01.Image = Properties.Resources.Necropolis_Tier6_01;
            Picture_Tier6_02.Image = Properties.Resources.Necropolis_Tier6_02;
            Picture_Tier6_03.Image = Properties.Resources.Necropolis_Tier6_03;
            Picture_Tier7_01.Image = Properties.Resources.Necropolis_Tier7_01;
            Picture_Tier7_02.Image = Properties.Resources.Necropolis_Tier7_02;
            Picture_Tier7_03.Image = Properties.Resources.Necropolis_Tier7_03;
            Dwelling_CheckBox1.Text = "Unearthed Graves";
            Dwelling_CheckBox1.Visible = true;
            Dwelling_CheckBox2.Text = "Dragon Tombstone";
            Dwelling_CheckBox2.Visible = true;
        }
        public void Current_Stronghold()
        {
            Picture_Tier1_01.Image = Properties.Resources.Stronghold_Tier1_01;
            Picture_Tier1_02.Image = Properties.Resources.Stronghold_Tier1_02;
            Picture_Tier1_03.Image = Properties.Resources.Stronghold_Tier1_03;
            Picture_Tier2_01.Image = Properties.Resources.Stronghold_Tier2_01;
            Picture_Tier2_02.Image = Properties.Resources.Stronghold_Tier2_02;
            Picture_Tier2_03.Image = Properties.Resources.Stronghold_Tier2_03;
            Picture_Tier3_01.Image = Properties.Resources.Stronghold_Tier3_01;
            Picture_Tier3_02.Image = Properties.Resources.Stronghold_Tier3_02;
            Picture_Tier3_03.Image = Properties.Resources.Stronghold_Tier3_03;
            Picture_Tier4_01.Image = Properties.Resources.Stronghold_Tier4_01;
            Picture_Tier4_02.Image = Properties.Resources.Stronghold_Tier4_02;
            Picture_Tier4_03.Image = Properties.Resources.Stronghold_Tier4_03;
            Picture_Tier5_01.Image = Properties.Resources.Stronghold_Tier5_01;
            Picture_Tier5_02.Image = Properties.Resources.Stronghold_Tier5_02;
            Picture_Tier5_03.Image = Properties.Resources.Stronghold_Tier5_03;
            Picture_Tier6_01.Image = Properties.Resources.Stronghold_Tier6_01;
            Picture_Tier6_02.Image = Properties.Resources.Stronghold_Tier6_02;
            Picture_Tier6_03.Image = Properties.Resources.Stronghold_Tier6_03;
            Picture_Tier7_01.Image = Properties.Resources.Stronghold_Tier7_01;
            Picture_Tier7_02.Image = Properties.Resources.Stronghold_Tier7_02;
            Picture_Tier7_03.Image = Properties.Resources.Stronghold_Tier7_03;
            Dwelling_CheckBox1.Text = "Garbage Pile";
            Dwelling_CheckBox1.Visible = true;
            Dwelling_CheckBox2.Text = "";
            Dwelling_CheckBox2.Visible = false;
        }
        public void Current_Sylvan()
        {
            Picture_Tier1_01.Image = Properties.Resources.Sylvan_Tier1_01;
            Picture_Tier1_02.Image = Properties.Resources.Sylvan_Tier1_02;
            Picture_Tier1_03.Image = Properties.Resources.Sylvan_Tier1_03;
            Picture_Tier2_01.Image = Properties.Resources.Sylvan_Tier2_01;
            Picture_Tier2_02.Image = Properties.Resources.Sylvan_Tier2_02;
            Picture_Tier2_03.Image = Properties.Resources.Sylvan_Tier2_03;
            Picture_Tier3_01.Image = Properties.Resources.Sylvan_Tier3_01;
            Picture_Tier3_02.Image = Properties.Resources.Sylvan_Tier3_02;
            Picture_Tier3_03.Image = Properties.Resources.Sylvan_Tier3_03;
            Picture_Tier4_01.Image = Properties.Resources.Sylvan_Tier4_01;
            Picture_Tier4_02.Image = Properties.Resources.Sylvan_Tier4_02;
            Picture_Tier4_03.Image = Properties.Resources.Sylvan_Tier4_03;
            Picture_Tier5_01.Image = Properties.Resources.Sylvan_Tier5_01;
            Picture_Tier5_02.Image = Properties.Resources.Sylvan_Tier5_02;
            Picture_Tier5_03.Image = Properties.Resources.Sylvan_Tier5_03;
            Picture_Tier6_01.Image = Properties.Resources.Sylvan_Tier6_01;
            Picture_Tier6_02.Image = Properties.Resources.Sylvan_Tier6_02;
            Picture_Tier6_03.Image = Properties.Resources.Sylvan_Tier6_03;
            Picture_Tier7_01.Image = Properties.Resources.Sylvan_Tier7_01;
            Picture_Tier7_02.Image = Properties.Resources.Sylvan_Tier7_02;
            Picture_Tier7_03.Image = Properties.Resources.Sylvan_Tier7_03;
            Dwelling_CheckBox1.Text = "Blooming Grove";
            Dwelling_CheckBox1.Visible = true;
            Dwelling_CheckBox2.Text = "Treant Saplings";
            Dwelling_CheckBox2.Visible = true;
        }
        #endregion

        private void UpDown(object sender, EventArgs e)
        {
            UI_Update();
        }
        private void Citadel_Check_Change(object sender, EventArgs e)
        {
            if (Check_Castle.Checked == true && Check_Citadel.Checked == false)
            {
                Check_Castle.Checked = false;
            }
            UI_Update();
        }
        private void Castle_Check_Change(object sender, EventArgs e)
        {
            if (Check_Castle.Checked == true && Check_Citadel.Checked == false)
            {
                Check_Citadel.Checked = true;
            }
            UI_Update();
        }
        private void Factions_SelectedIndexChanged(object sender, EventArgs e)
        {
            UI_Update();
            ComboBox Clicked = (ComboBox)sender;
            Dwelling_CheckBox1.Checked = false;
            Dwelling_CheckBox2.Checked = false;
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
            if (Check_WeekLimit.Checked == true)
            {
                UpDown_WeekLimit.Enabled = true;
            }
            else
            {
                UpDown_WeekLimit.Enabled = false;
                UpDown_WeekLimit.Value = 1;
            }
            UI_Update();
        }
        private void Check_GoldLimit_CheckedChanged(object sender, EventArgs e)
        {
            if (Check_GoldLimit.Checked == true)
            {
                UpDown_GoldLimit.Enabled = true;
                Gold_Maximum();
            }
            else
            {
                UpDown_GoldLimit.Enabled = false;
                Label_GoldLimitExceeded.Visible = false;
            }
        }
        private void UpDown_GoldLimit_ValueChanged(object sender, EventArgs e)
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
            //since all PictureBoxes are named in the fashion "Picture_TierX_0Y", where X is the corresponding
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

        public void InitFactions()
        {
            foreach (var factionName in FactionList)
            {
                Faction faction = new Faction(factionName);
            }
        }
    }
}