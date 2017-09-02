﻿using System;
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
		public int[,] Town_Tier_Base_Gold = new int[8,8];           //The gold cost of each basic creature of each faction
		public int[,] Town_Tier_Upg_Gold = new int[8,8];            //The gold cost of upgraded creatures of each faction
        public int[,] Town_Tier_Pop = new int[8,8];                 //The weekly population of each tier of creatures of each faction
        public Label[] Gold_Tier = new Label[8];                    //Array to hold all the 'creature gold cost' labels of the form
        public Label[] Pop_Tier = new Label[8];                     //Array to hold all the 'creature population' labels of the form
        public Label[] Total_Tier = new Label[8];                   //Array to hold all the 'creature total gold cost' labels of the form
        public NumericUpDown[] UpDown_Tier = new NumericUpDown[8];  //Array to hold all the form's NumericUpDowns
        public bool[] Upg = new bool[8];                            //Array of booleans that designate if an upgraded variant is selected or not for each creature tier

        //This Dictionary<string, int[,,]> holds all the values for the gold cost of a faction's units
        //The format is the following: Dictionary<FactionName, [Base Unit Gold cost, Upgraded Unit Gold cost, Unit Population]>
        //For ease of use the array is padded with zero values at index 0 (so that Tier 3's data is at index 3, for example)
        public Dictionary<string, int[,,]> FactionData = new Dictionary<string, int[,,]>();

        //This array holds  each faction's population increase dwelling, it is two-dimentional, since some factions have two such dwellings.
        //Array padded with zero for ease of handling and for usage in case Dwelling checkbox is not checked.
        //-------------------------------
        //The dwellings are as follows:
        //Academy - Treasure Cave (Djinn)
        //Dungeon - Ritual Pit (Custom)
        //Fortress - Wrestler's Arena (Brawler) / Runic Sanctuary (Rune Priest)
        //Haven - Farms (Peasant)
        //Inferno - Spawn of Chaos (Horned Deamon) / Halls of Horror (Hell Charger)
        //Necropolis - Unearthed Graves (Skeleton) / Dragon Tombstone (Bone Dragon)
        //Stronghold - Garbage Pile (Goblin)
        //Sylvan - Blooming Grove (Pixie) / Treant Saplings (Treant)
        public int[,] Dwelling = { { 0, 0 }, { 2, 0 }, { 0, 0 }, { 4, 1 }, { 5, 0 }, { 2, 1 }, { 6, 1 }, { 6, 0 }, { 4, 1 } };

        //The purpose of this dictionary is to ease the access of information for each faction
        Dictionary<string, int> Town = new Dictionary<string, int>() {{"Academy", 0}, {"Dungeon", 1}, {"Fortress", 2}, {"Haven", 3}, {"Inferno", 4}, {"Necropolis", 5}, {"Stronghold", 6}, {"Sylvan", 7}};

        //A list of all the factions in the game
        List<string> FactionList = new List<string> { "Academy", "Dungeon", "Fortress", "Haven", "Inferno", "Necropolis", "Stronghold", "Sylvan" };

        public Form1()
        {
            FactionData.Add("Academy", new int[0, 0, 0]);
            FactionData.Add("Academy", new int[22, 35, 20]);
            FactionData.Add("Academy", new int[45, 70, 14]);
            FactionData.Add("Academy", new int[90, 130, 9]);
            FactionData.Add("Academy", new int[250, 340, 5]);
            FactionData.Add("Academy", new int[480, 700, 3]);
            FactionData.Add("Academy", new int[1400, 1770, 2]);
            FactionData.Add("Academy", new int[3500, 4700, 1]);
            FactionData.Add("Dungeon", new int[0, 0, 0]);
            FactionData.Add("Dungeon", new int[60, 100, 7]);
            FactionData.Add("Dungeon", new int[125, 175, 5]);
            FactionData.Add("Dungeon", new int[140, 200, 6]);
            FactionData.Add("Dungeon", new int[300, 450, 4]);
            FactionData.Add("Dungeon", new int[550, 800, 3]);
            FactionData.Add("Dungeon", new int[1400, 1700, 2]);
            FactionData.Add("Dungeon", new int[3000, 3700, 1]);
            FactionData.Add("Fortress", new int[0, 0, 0]);
            FactionData.Add("Fortress", new int[24, 40, 18]);
            FactionData.Add("Fortress", new int[45, 65, 14]);
            FactionData.Add("Fortress", new int[130, 185, 7]);
            FactionData.Add("Fortress", new int[160, 220, 6]);
            FactionData.Add("Fortress", new int[470, 700, 3]);
            FactionData.Add("Fortress", new int[1300, 1700, 2]);
            FactionData.Add("Fortress", new int[2700, 3400, 1]);
            FactionData.Add("Haven", new int[0, 0, 0]);
            FactionData.Add("Haven", new int[15, 25, 22]);
            FactionData.Add("Haven", new int[50, 80, 12]);
            FactionData.Add("Haven", new int[85, 130, 10]);
            FactionData.Add("Haven", new int[250, 370, 5]);
            FactionData.Add("Haven", new int[600, 850, 3]);
            FactionData.Add("Haven", new int[1300, 1700, 2]);
            FactionData.Add("Haven", new int[2800, 3500, 1]);
            FactionData.Add("Inferno", new int[0, 0, 0]);
            FactionData.Add("Inferno", new int[25, 45, 16]);
            FactionData.Add("Inferno", new int[40, 60, 15]);
            FactionData.Add("Inferno", new int[110, 160, 8]);
            FactionData.Add("Inferno", new int[240, 350, 5]);
            FactionData.Add("Inferno", new int[550, 780, 3]);
            FactionData.Add("Inferno", new int[1400, 1666, 2]);
            FactionData.Add("Inferno", new int[2666, 3666, 1]);
            FactionData.Add("Necropolis", new int[0, 0, 0]);
            FactionData.Add("Necropolis", new int[19, 30, 20]);
            FactionData.Add("Necropolis", new int[40, 60, 15]);
            FactionData.Add("Necropolis", new int[100, 140, 9]);
            FactionData.Add("Necropolis", new int[250, 380, 5]);
            FactionData.Add("Necropolis", new int[620, 850, 3]);
            FactionData.Add("Necropolis", new int[1400, 1700, 2]);
            FactionData.Add("Necropolis", new int[1600, 1900, 1]);
            FactionData.Add("Stronghold", new int[0, 0, 0]);
            FactionData.Add("Stronghold", new int[10, 20, 25]);
            FactionData.Add("Stronghold", new int[50, 70, 14]);
            FactionData.Add("Stronghold", new int[80, 120, 11]);
            FactionData.Add("Stronghold", new int[260, 360, 5]);
            FactionData.Add("Stronghold", new int[350, 500, 5]);
            FactionData.Add("Stronghold", new int[1250, 1600, 2]);
            FactionData.Add("Stronghold", new int[2900, 3450, 1]);
            FactionData.Add("Sylvan", new int[0, 0, 0]);
            FactionData.Add("Sylvan", new int[35, 55, 10]);
            FactionData.Add("Sylvan", new int[70, 120, 9]);
            FactionData.Add("Sylvan", new int[120, 190, 7]);
            FactionData.Add("Sylvan", new int[320, 440, 4]);
            FactionData.Add("Sylvan", new int[630, 900, 3]);
            FactionData.Add("Sylvan", new int[1100, 1400, 2]);
            FactionData.Add("Sylvan", new int[2500, 3400, 1]);

            InitializeComponent();
            //We bind the labels showing each creature's cost, it's weekly population and the total cost of the chosen
            //amount and the numeric updowns to an array, so we can access them easily.
            Pop_Tier[0] = null;
            Gold_Tier[0] = null;
            Total_Tier[0] = null;
            UpDown_Tier[0] = null;
            for (int i = 1; i < 8; i++)
            {
                Pop_Tier[i] = (Label)Controls.Find("Pop_Tier" + i, true).FirstOrDefault();
                Gold_Tier[i] = (Label)Controls.Find("Gold_Tier" + i, true).FirstOrDefault();
                Total_Tier[i] = (Label)Controls.Find("Total_Tier" + i, true).FirstOrDefault();
                UpDown_Tier[i] = (NumericUpDown)Controls.Find("UpDown_Tier" + i, true).FirstOrDefault();
            }

            //We assign data for all the factions to the array we will be working with.
            for (int i = 0; i < 8; i++)
            {
                Town_Tier_Base_Gold[0, i] = Academy_Base_Gold_Tier[i];
                Town_Tier_Upg_Gold[0, i] = Academy_Upg_Gold_Tier[i];
                Town_Tier_Pop[0, i] = Academy_Pop_Tier[i];

                Town_Tier_Base_Gold[1, i] = Dungeon_Base_Gold_Tier[i];
                Town_Tier_Upg_Gold[1, i] = Dungeon_Upg_Gold_Tier[i];
                Town_Tier_Pop[1, i] = Dungeon_Pop_Tier[i];

                Town_Tier_Base_Gold[2, i] = Fortress_Base_Gold_Tier[i];
                Town_Tier_Upg_Gold[2, i] = Fortress_Upg_Gold_Tier[i];
                Town_Tier_Pop[2, i] = Fortress_Pop_Tier[i];

                Town_Tier_Base_Gold[3, i] = Haven_Base_Gold_Tier[i];
                Town_Tier_Upg_Gold[3, i] = Haven_Upg_Gold_Tier[i];
                Town_Tier_Pop[3, i] = Haven_Pop_Tier[i];

                Town_Tier_Base_Gold[4, i] = Inferno_Base_Gold_Tier[i];
                Town_Tier_Upg_Gold[4, i] = Inferno_Upg_Gold_Tier[i];
                Town_Tier_Pop[4, i] = Inferno_Pop_Tier[i];

                Town_Tier_Base_Gold[5, i] = Necropolis_Base_Gold_Tier[i];
                Town_Tier_Upg_Gold[5, i] = Necropolis_Upg_Gold_Tier[i];
                Town_Tier_Pop[5, i] = Necropolis_Pop_Tier[i];

                Town_Tier_Base_Gold[6, i] = Stronghold_Base_Gold_Tier[i];
                Town_Tier_Upg_Gold[6, i] = Stronghold_Upg_Gold_Tier[i];
                Town_Tier_Pop[6, i] = Stronghold_Pop_Tier[i];

                Town_Tier_Base_Gold[7, i] = Sylvan_Base_Gold_Tier[i];
                Town_Tier_Upg_Gold[7, i] = Sylvan_Upg_Gold_Tier[i];
                Town_Tier_Pop[7, i] = Sylvan_Pop_Tier[i];
            }
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
            int Faction_Index = Town[(string)Factions.SelectedItem];
            int Gold_Total = 0;

            //This loop sets the labels for Creature Gold cost, Creature Population,
            //Cost of selected creatures and Total Gold.
            for (int i = 1; i < 8; i++)
            {
                //Whenever the image of an upgraded creature is clicked, the boolean variable "Upg" is set to "true"
                //this "if" checkes whether "Upg" is true and changes the gold cost of the creature appropriately
                if (Upg[i] == true)
                {
                    Gold_Tier[i].Text = Convert.ToString(Town_Tier_Upg_Gold[Faction_Index, i]);
                }
                else
                {
                    Gold_Tier[i].Text = Convert.ToString(Town_Tier_Base_Gold[Faction_Index, i]);
                }

                //This "if" sets the creature populations and UpDown controls' maximums in accordance
                //to a checked Castle or Citadel
				if (Check_Castle.Checked == true)
				{
					Pop_Tier[i].Text = Convert.ToString(Town_Tier_Pop[Faction_Index, i] * (int)UpDown_WeekLimit.Value * 2);
				}
				else
				{
					if (Check_Citadel.Checked == true)
					{
						Pop_Tier[i].Text = Convert.ToString(Math.Truncate(Town_Tier_Pop[Faction_Index, i] * (int)UpDown_WeekLimit.Value * (decimal)1.5));
					}
					else
					{
						Pop_Tier[i].Text = Convert.ToString(Town_Tier_Pop[Faction_Index, i] * (int)UpDown_WeekLimit.Value);
					}
				}

                //The following two lines set the label text for the total Gold cost of every creature tier and
                //the Total Gold amount stored in the "Gold_Total" variable
                Total_Tier[i].Text = Convert.ToString(Convert.ToInt32(Gold_Tier[i].Text) * UpDown_Tier[i].Value);
                Gold_Total = Gold_Total + Convert.ToInt32(Total_Tier[i].Text);
                if (Check_WeekLimit.Checked == true) UpDown_Tier[i].Maximum = Convert.ToInt32(Pop_Tier[i].Text);
            }


            Dwelling_Checked();
            if (Dwelling_Tier_A != 0 && Dwelling_CheckBox1.Checked == true) Pop_Tier[Dwelling_Tier_A].Text = Convert.ToString(Convert.ToInt32(Pop_Tier[Dwelling_Tier_A].Text) + (Dwelling[Faction, 0] * (int)UpDown_WeekLimit.Value));
            if (Dwelling_Tier_B != 0 && (Dwelling_CheckBox2.Checked == true || (Convert.ToString(Factions.SelectedItem) == "Dungeon" && Dwelling_CheckBox1.Checked == true))) Pop_Tier[Dwelling_Tier_B].Text = Convert.ToString(Convert.ToInt32(Pop_Tier[Dwelling_Tier_B].Text) + (Dwelling[Faction, 1] * (int)UpDown_WeekLimit.Value));
            
            //The Total Gold text label is updated with the appropriate value and the check for the gold constraint is performed
            Label_GoldTotal.Text = Convert.ToString(Gold_Total);
			Gold_Maximum();
            Dwelling_Tier_A = 0;
            Dwelling_Tier_B = 0;
        }

        //This method sets which creature tiers should benefit from extra population, should the bonus dwelling be checked
        public void Dwelling_Checked()
        {
            switch (Convert.ToString(Factions.SelectedItem))
            {
                case "Academy":
                    Dwelling_Tier_A = 5;
                    Dwelling_Tier_B = 0;
                    Faction = 1;
                    break;
                case "Dungeon":
                    Dwelling_Tier_A = 2;
                    Dwelling_Tier_B = 3;
                    Faction = 2;
                    if (Dwelling_CheckBox1.Checked == false)
                    {
                        Dungeon_Ritual_UpDown1.Value = 0;
                        Dungeon_Ritual_UpDown2.Value = 0;
                    }
                    Dwelling[Faction, 0] = (int)Dungeon_Ritual_UpDown1.Value;
                    Dwelling[Faction, 1] = (int)Dungeon_Ritual_UpDown2.Value;
                    break;
                case "Fortress":
                    Dwelling_Tier_A = 4;
                    Dwelling_Tier_B = 5;
                    Faction = 3;
                    break;
                case "Haven":
                    Dwelling_Tier_A = 1;
                    Dwelling_Tier_B = 0;
                    Faction = 4;
                    break;
                case "Inferno":
                    Dwelling_Tier_A = 2;
                    Dwelling_Tier_B = 5;
                    Faction = 5;
                    break;
                case "Necropolis":
                    Dwelling_Tier_A = 1;
                    Dwelling_Tier_B = 7;
                    Faction = 6;
                    break;
                case "Stronghold":
                    Dwelling_Tier_A = 1;
                    Dwelling_Tier_B = 0;
                    Faction = 7;
                    break;
                case "Sylvan":
                    Dwelling_Tier_A = 1;
                    Dwelling_Tier_B = 6;
                    Faction = 8;
                    break;
                default:
                    break;
            }
        }

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
                Upg[Control_Tier] = false;
            }
            else
            {
                Upg[Control_Tier] = true;
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