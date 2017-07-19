using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//To-do:
//Put all "currents" in one method, loop them all
//put a "remember valus" checkbox and set it so that choosing a faction either nulls the values in the updowns or not
//fix citadel
//add special creature population bonuses
//add faction themes to the window

namespace Heroes5_ArmyCalc
{
    public partial class Form1 : Form
    {
        public int[] Current_Base_Gold_Tier = new int[8];
        public int[] Current_Upg_Gold_Tier = new int[8];
        public int[] Current_Pop_Tier = new int[8];
        public Label[] Gold_Tier = new Label[8];
        public Label[] Pop_Tier = new Label[8];

        //The arrays represent the gold cost or population value for each tier of creatures of each faction
        //For ease of handling the arrays are size 8, so that it can be convenient to call on
        //array member [3] for the value for the tier 3 creture, for that purpose member [0] is padded with a zero
        public string[] Town = {"Academy", "Dungeon", "Fortress", "Haven", "Inferno", "Necropolis", "Stronghold", "Sylvan"};

        public int[] Academy_Base_Gold_Tier = {0, 22, 45, 90, 250, 480, 1400, 3500};
        public int[] Academy_Upg_Gold_Tier = {0, 35, 70, 130, 340, 700, 1770, 4700};
        public int[] Academy_Pop_Tier = {0, 20, 14, 9, 5, 3, 2, 1};

        public int[] Dungeon_Base_Gold_Tier = {0, 60, 125, 140, 300, 550, 1400, 3000};
        public int[] Dungeon_Upg_Gold_Tier = {0, 100, 175, 200, 450, 800, 1700, 3700};
        public int[] Dungeon_Pop_Tier = {0, 7, 5, 6, 4, 3, 2, 1};

        public int[] Sylvan_Base_Gold_Tier = {0, 35, 70, 120, 320, 630, 1100, 2500};
        public int[] Sylvan_Upg_Gold_Tier = {0, 55, 120, 190, 440, 900, 1400, 3400};
        public int[] Sylvan_Pop_Tier = {0, 10, 9, 7, 4, 3, 2, 1};

        public int[] Stronghold_Base_Gold_Tier = {0, 10, 50, 80, 260, 350, 1250, 2900};
        public int[] Stronghold_Upg_Gold_Tier = {0, 20, 70, 120, 360, 500, 1600, 3450};
        public int[] Stronghold_Pop_Tier = {0, 25, 14, 11, 5, 5, 2, 1};

        public int[] Fortress_Base_Gold_Tier = {0, 24, 45, 130, 160, 470, 1300, 2700};
        public int[] Fortress_Upg_Gold_Tier = {0, 40, 65, 185, 220, 700, 1700, 3400};
        public int[] Fortress_Pop_Tier = {0, 18, 14, 7, 6, 3, 2, 1};

        public int[] Necropolis_Base_Gold_Tier = {0, 19, 40, 100, 250, 620, 1400, 1600};
        public int[] Necropolis_Upg_Gold_Tier = {0, 30, 60, 140, 380, 850, 1700, 1900};
        public int[] Necropolis_Pop_Tier = {0, 20, 15, 9, 5, 3, 2, 1};

        public int[] Inferno_Base_Gold_Tier = {0, 25, 40, 110, 240, 550, 1400, 2666};
        public int[] Inferno_Upg_Gold_Tier = {0, 45, 60, 160, 350, 780, 1666, 3666};
        public int[] Inferno_Pop_Tier = {0, 16, 15, 8, 5, 3, 2, 1};

        public int[] Haven_Base_Gold_Tier = {0, 15, 50, 85, 250, 600, 1300, 2800};
        public int[] Haven_Upg_Gold_Tier = {0, 25, 80, 130, 370, 850, 1700, 3500};
        public int[] Haven_Pop_Tier = {0, 22, 12, 10, 5, 3, 2, 1};

        //The purpose of this dictionary is to ease the access of information for each faction
        Dictionary<string, int> Info = new Dictionary<string, int>();

        public Form1()
        {
            InitializeComponent();
            Frame1.SendToBack();
            Frame2.SendToBack();
            Frame3.SendToBack();
            Frame4.SendToBack();
            Frame5.SendToBack();
            Frame6.SendToBack();
            Frame7.SendToBack();
            Factions.SelectedItem = "Academy";
            //DialogResult result;
            //string msg = (string)Factions.SelectedItem;
            //result = MessageBox.Show(msg);
            //Current_Haven();
            Pop_Tier[0] = null;
            Pop_Tier[1] = this.Pop_Tier1;
            Pop_Tier[2] = this.Pop_Tier2;
            Pop_Tier[3] = this.Pop_Tier3;
            Pop_Tier[4] = this.Pop_Tier4;
            Pop_Tier[5] = this.Pop_Tier5;
            Pop_Tier[6] = this.Pop_Tier6;
            Pop_Tier[7] = this.Pop_Tier7;
            Gold_Tier[0] = null;
            Gold_Tier[1] = this.Gold_Tier1;
            Gold_Tier[2] = this.Gold_Tier2;
            Gold_Tier[3] = this.Gold_Tier3;
            Gold_Tier[4] = this.Gold_Tier4;
            Gold_Tier[5] = this.Gold_Tier5;
            Gold_Tier[6] = this.Gold_Tier6;
            Gold_Tier[7] = this.Gold_Tier7;
        }

        public void Info_Set()
        {
            for (int k = 0; k < 8; k++)
            {
                for (int i = 1; i < 8; i++)
                {
                    string key = Town[k] + "_Base_Gold_Tier" + i;
                    Info.Add(key, Haven_Base_Gold_Tier[i]);
                }
            }
        }

        public void UI_Reset()
        {
            //Sets label texts and images to the currently selected faction
            Frame1.Location = new Point(Picture_Tier1_01.Left - 3, Picture_Tier1_01.Top - 3);
            Frame2.Location = new Point(Picture_Tier2_01.Left - 3, Picture_Tier2_01.Top - 3);
            Frame3.Location = new Point(Picture_Tier3_01.Left - 3, Picture_Tier3_01.Top - 3);
            Frame4.Location = new Point(Picture_Tier4_01.Left - 3, Picture_Tier4_01.Top - 3);
            Frame5.Location = new Point(Picture_Tier5_01.Left - 3, Picture_Tier5_01.Top - 3);
            Frame6.Location = new Point(Picture_Tier6_01.Left - 3, Picture_Tier6_01.Top - 3);
            Frame7.Location = new Point(Picture_Tier7_01.Left - 3, Picture_Tier7_01.Top - 3);
            for (int i = 1; i < 8; i++)
            {
                Gold_Tier[i].Text = Convert.ToString(Current_Base_Gold_Tier[i]);
                Pop_Tier[i].Text = Convert.ToString(Current_Pop_Tier[i]);
            }
        }

        public void Current_Faction()
        {
            for (int i = 1; i < 8; i++)
            {
                Current_Base_Gold_Tier[i] = Haven_Base_Gold_Tier[i];
                Current_Upg_Gold_Tier[i] = Haven_Upg_Gold_Tier[i];
                Current_Pop_Tier[i] = Haven_Pop_Tier[i];
            }

            Current_Base_Gold_Tier[1] = Haven_Base_Gold_Tier[1];
            Current_Base_Gold_Tier[2] = Haven_Base_Gold_Tier[2];
            Current_Base_Gold_Tier[3] = Haven_Base_Gold_Tier[3];
            Current_Base_Gold_Tier[4] = Haven_Base_Gold_Tier[4];
            Current_Base_Gold_Tier[5] = Haven_Base_Gold_Tier[5];
            Current_Base_Gold_Tier[6] = Haven_Base_Gold_Tier[6];
            Current_Base_Gold_Tier[7] = Haven_Base_Gold_Tier[7];
            Current_Upg_Gold_Tier[1] = Haven_Upg_Gold_Tier[1];
            Current_Upg_Gold_Tier[2] = Haven_Upg_Gold_Tier[2];
            Current_Upg_Gold_Tier[3] = Haven_Upg_Gold_Tier[3];
            Current_Upg_Gold_Tier[4] = Haven_Upg_Gold_Tier[4];
            Current_Upg_Gold_Tier[5] = Haven_Upg_Gold_Tier[5];
            Current_Upg_Gold_Tier[6] = Haven_Upg_Gold_Tier[6];
            Current_Upg_Gold_Tier[7] = Haven_Upg_Gold_Tier[7];
            Current_Pop_Tier[1] = Haven_Pop_Tier[1];
            Current_Pop_Tier[2] = Haven_Pop_Tier[2];
            Current_Pop_Tier[3] = Haven_Pop_Tier[3];
            Current_Pop_Tier[4] = Haven_Pop_Tier[4];
            Current_Pop_Tier[5] = Haven_Pop_Tier[5];
            Current_Pop_Tier[6] = Haven_Pop_Tier[6];
            Current_Pop_Tier[7] = Haven_Pop_Tier[7];
            UI_Reset();
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
        }
        public void Current_Academy()
        {
            Current_Base_Gold_Tier[1] = Academy_Base_Gold_Tier[1];
            Current_Base_Gold_Tier[2] = Academy_Base_Gold_Tier[2];
            Current_Base_Gold_Tier[3] = Academy_Base_Gold_Tier[3];
            Current_Base_Gold_Tier[4] = Academy_Base_Gold_Tier[4];
            Current_Base_Gold_Tier[5] = Academy_Base_Gold_Tier[5];
            Current_Base_Gold_Tier[6] = Academy_Base_Gold_Tier[6];
            Current_Base_Gold_Tier[7] = Academy_Base_Gold_Tier[7];
            Current_Upg_Gold_Tier[1] = Academy_Upg_Gold_Tier[1];
            Current_Upg_Gold_Tier[2] = Academy_Upg_Gold_Tier[2];
            Current_Upg_Gold_Tier[3] = Academy_Upg_Gold_Tier[3];
            Current_Upg_Gold_Tier[4] = Academy_Upg_Gold_Tier[4];
            Current_Upg_Gold_Tier[5] = Academy_Upg_Gold_Tier[5];
            Current_Upg_Gold_Tier[6] = Academy_Upg_Gold_Tier[6];
            Current_Upg_Gold_Tier[7] = Academy_Upg_Gold_Tier[7];
            Current_Pop_Tier[1] = Academy_Pop_Tier[1];
            Current_Pop_Tier[2] = Academy_Pop_Tier[2];
            Current_Pop_Tier[3] = Academy_Pop_Tier[3];
            Current_Pop_Tier[4] = Academy_Pop_Tier[4];
            Current_Pop_Tier[5] = Academy_Pop_Tier[5];
            Current_Pop_Tier[6] = Academy_Pop_Tier[6];
            Current_Pop_Tier[7] = Academy_Pop_Tier[7];
            UI_Reset();
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
        }
        public void Current_Dungeon()
        {
            Current_Base_Gold_Tier[1] = Dungeon_Base_Gold_Tier[1];
            Current_Base_Gold_Tier[2] = Dungeon_Base_Gold_Tier[2];
            Current_Base_Gold_Tier[3] = Dungeon_Base_Gold_Tier[3];
            Current_Base_Gold_Tier[4] = Dungeon_Base_Gold_Tier[4];
            Current_Base_Gold_Tier[5] = Dungeon_Base_Gold_Tier[5];
            Current_Base_Gold_Tier[6] = Dungeon_Base_Gold_Tier[6];
            Current_Base_Gold_Tier[7] = Dungeon_Base_Gold_Tier[7];
            Current_Upg_Gold_Tier[1] = Dungeon_Upg_Gold_Tier[1];
            Current_Upg_Gold_Tier[2] = Dungeon_Upg_Gold_Tier[2];
            Current_Upg_Gold_Tier[3] = Dungeon_Upg_Gold_Tier[3];
            Current_Upg_Gold_Tier[4] = Dungeon_Upg_Gold_Tier[4];
            Current_Upg_Gold_Tier[5] = Dungeon_Upg_Gold_Tier[5];
            Current_Upg_Gold_Tier[6] = Dungeon_Upg_Gold_Tier[6];
            Current_Upg_Gold_Tier[7] = Dungeon_Upg_Gold_Tier[7];
            Current_Pop_Tier[1] = Dungeon_Pop_Tier[1];
            Current_Pop_Tier[2] = Dungeon_Pop_Tier[2];
            Current_Pop_Tier[3] = Dungeon_Pop_Tier[3];
            Current_Pop_Tier[4] = Dungeon_Pop_Tier[4];
            Current_Pop_Tier[5] = Dungeon_Pop_Tier[5];
            Current_Pop_Tier[6] = Dungeon_Pop_Tier[6];
            Current_Pop_Tier[7] = Dungeon_Pop_Tier[7];
            UI_Reset();
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
        }
        public void Current_Inferno()
        {
            Current_Base_Gold_Tier[1] = Inferno_Base_Gold_Tier[1];
            Current_Base_Gold_Tier[2] = Inferno_Base_Gold_Tier[2];
            Current_Base_Gold_Tier[3] = Inferno_Base_Gold_Tier[3];
            Current_Base_Gold_Tier[4] = Inferno_Base_Gold_Tier[4];
            Current_Base_Gold_Tier[5] = Inferno_Base_Gold_Tier[5];
            Current_Base_Gold_Tier[6] = Inferno_Base_Gold_Tier[6];
            Current_Base_Gold_Tier[7] = Inferno_Base_Gold_Tier[7];
            Current_Upg_Gold_Tier[1] = Inferno_Upg_Gold_Tier[1];
            Current_Upg_Gold_Tier[2] = Inferno_Upg_Gold_Tier[2];
            Current_Upg_Gold_Tier[3] = Inferno_Upg_Gold_Tier[3];
            Current_Upg_Gold_Tier[4] = Inferno_Upg_Gold_Tier[4];
            Current_Upg_Gold_Tier[5] = Inferno_Upg_Gold_Tier[5];
            Current_Upg_Gold_Tier[6] = Inferno_Upg_Gold_Tier[6];
            Current_Upg_Gold_Tier[7] = Inferno_Upg_Gold_Tier[7];
            Current_Pop_Tier[1] = Inferno_Pop_Tier[1];
            Current_Pop_Tier[2] = Inferno_Pop_Tier[2];
            Current_Pop_Tier[3] = Inferno_Pop_Tier[3];
            Current_Pop_Tier[4] = Inferno_Pop_Tier[4];
            Current_Pop_Tier[5] = Inferno_Pop_Tier[5];
            Current_Pop_Tier[6] = Inferno_Pop_Tier[6];
            Current_Pop_Tier[7] = Inferno_Pop_Tier[7];
            UI_Reset();
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
        }
        public void Current_Sylvan()
        {
            Current_Base_Gold_Tier[1] = Sylvan_Base_Gold_Tier[1];
            Current_Base_Gold_Tier[2] = Sylvan_Base_Gold_Tier[2];
            Current_Base_Gold_Tier[3] = Sylvan_Base_Gold_Tier[3];
            Current_Base_Gold_Tier[4] = Sylvan_Base_Gold_Tier[4];
            Current_Base_Gold_Tier[5] = Sylvan_Base_Gold_Tier[5];
            Current_Base_Gold_Tier[6] = Sylvan_Base_Gold_Tier[6];
            Current_Base_Gold_Tier[7] = Sylvan_Base_Gold_Tier[7];
            Current_Upg_Gold_Tier[1] = Sylvan_Upg_Gold_Tier[1];
            Current_Upg_Gold_Tier[2] = Sylvan_Upg_Gold_Tier[2];
            Current_Upg_Gold_Tier[3] = Sylvan_Upg_Gold_Tier[3];
            Current_Upg_Gold_Tier[4] = Sylvan_Upg_Gold_Tier[4];
            Current_Upg_Gold_Tier[5] = Sylvan_Upg_Gold_Tier[5];
            Current_Upg_Gold_Tier[6] = Sylvan_Upg_Gold_Tier[6];
            Current_Upg_Gold_Tier[7] = Sylvan_Upg_Gold_Tier[7];
            Current_Pop_Tier[1] = Sylvan_Pop_Tier[1];
            Current_Pop_Tier[2] = Sylvan_Pop_Tier[2];
            Current_Pop_Tier[3] = Sylvan_Pop_Tier[3];
            Current_Pop_Tier[4] = Sylvan_Pop_Tier[4];
            Current_Pop_Tier[5] = Sylvan_Pop_Tier[5];
            Current_Pop_Tier[6] = Sylvan_Pop_Tier[6];
            Current_Pop_Tier[7] = Sylvan_Pop_Tier[7];
            UI_Reset();
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
        }
        public void Current_Fortress()
        {
            Current_Base_Gold_Tier[1] = Fortress_Base_Gold_Tier[1];
            Current_Base_Gold_Tier[2] = Fortress_Base_Gold_Tier[2];
            Current_Base_Gold_Tier[3] = Fortress_Base_Gold_Tier[3];
            Current_Base_Gold_Tier[4] = Fortress_Base_Gold_Tier[4];
            Current_Base_Gold_Tier[5] = Fortress_Base_Gold_Tier[5];
            Current_Base_Gold_Tier[6] = Fortress_Base_Gold_Tier[6];
            Current_Base_Gold_Tier[7] = Fortress_Base_Gold_Tier[7];
            Current_Upg_Gold_Tier[1] = Fortress_Upg_Gold_Tier[1];
            Current_Upg_Gold_Tier[2] = Fortress_Upg_Gold_Tier[2];
            Current_Upg_Gold_Tier[3] = Fortress_Upg_Gold_Tier[3];
            Current_Upg_Gold_Tier[4] = Fortress_Upg_Gold_Tier[4];
            Current_Upg_Gold_Tier[5] = Fortress_Upg_Gold_Tier[5];
            Current_Upg_Gold_Tier[6] = Fortress_Upg_Gold_Tier[6];
            Current_Upg_Gold_Tier[7] = Fortress_Upg_Gold_Tier[7];
            Current_Pop_Tier[1] = Fortress_Pop_Tier[1];
            Current_Pop_Tier[2] = Fortress_Pop_Tier[2];
            Current_Pop_Tier[3] = Fortress_Pop_Tier[3];
            Current_Pop_Tier[4] = Fortress_Pop_Tier[4];
            Current_Pop_Tier[5] = Fortress_Pop_Tier[5];
            Current_Pop_Tier[6] = Fortress_Pop_Tier[6];
            Current_Pop_Tier[7] = Fortress_Pop_Tier[7];
            UI_Reset();
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
        }
        public void Current_Stronghold()
        {
            Current_Base_Gold_Tier[1] = Stronghold_Base_Gold_Tier[1];
            Current_Base_Gold_Tier[2] = Stronghold_Base_Gold_Tier[2];
            Current_Base_Gold_Tier[3] = Stronghold_Base_Gold_Tier[3];
            Current_Base_Gold_Tier[4] = Stronghold_Base_Gold_Tier[4];
            Current_Base_Gold_Tier[5] = Stronghold_Base_Gold_Tier[5];
            Current_Base_Gold_Tier[6] = Stronghold_Base_Gold_Tier[6];
            Current_Base_Gold_Tier[7] = Stronghold_Base_Gold_Tier[7];
            Current_Upg_Gold_Tier[1] = Stronghold_Upg_Gold_Tier[1];
            Current_Upg_Gold_Tier[2] = Stronghold_Upg_Gold_Tier[2];
            Current_Upg_Gold_Tier[3] = Stronghold_Upg_Gold_Tier[3];
            Current_Upg_Gold_Tier[4] = Stronghold_Upg_Gold_Tier[4];
            Current_Upg_Gold_Tier[5] = Stronghold_Upg_Gold_Tier[5];
            Current_Upg_Gold_Tier[6] = Stronghold_Upg_Gold_Tier[6];
            Current_Upg_Gold_Tier[7] = Stronghold_Upg_Gold_Tier[7];
            Current_Pop_Tier[1] = Stronghold_Pop_Tier[1];
            Current_Pop_Tier[2] = Stronghold_Pop_Tier[2];
            Current_Pop_Tier[3] = Stronghold_Pop_Tier[3];
            Current_Pop_Tier[4] = Stronghold_Pop_Tier[4];
            Current_Pop_Tier[5] = Stronghold_Pop_Tier[5];
            Current_Pop_Tier[6] = Stronghold_Pop_Tier[6];
            Current_Pop_Tier[7] = Stronghold_Pop_Tier[7];
            UI_Reset();
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
        }
        public void Current_Necropolis()
        {
            Current_Base_Gold_Tier[1] = Necropolis_Base_Gold_Tier[1];
            Current_Base_Gold_Tier[2] = Necropolis_Base_Gold_Tier[2];
            Current_Base_Gold_Tier[3] = Necropolis_Base_Gold_Tier[3];
            Current_Base_Gold_Tier[4] = Necropolis_Base_Gold_Tier[4];
            Current_Base_Gold_Tier[5] = Necropolis_Base_Gold_Tier[5];
            Current_Base_Gold_Tier[6] = Necropolis_Base_Gold_Tier[6];
            Current_Base_Gold_Tier[7] = Necropolis_Base_Gold_Tier[7];
            Current_Upg_Gold_Tier[1] = Necropolis_Upg_Gold_Tier[1];
            Current_Upg_Gold_Tier[2] = Necropolis_Upg_Gold_Tier[2];
            Current_Upg_Gold_Tier[3] = Necropolis_Upg_Gold_Tier[3];
            Current_Upg_Gold_Tier[4] = Necropolis_Upg_Gold_Tier[4];
            Current_Upg_Gold_Tier[5] = Necropolis_Upg_Gold_Tier[5];
            Current_Upg_Gold_Tier[6] = Necropolis_Upg_Gold_Tier[6];
            Current_Upg_Gold_Tier[7] = Necropolis_Upg_Gold_Tier[7];
            Current_Pop_Tier[1] = Necropolis_Pop_Tier[1];
            Current_Pop_Tier[2] = Necropolis_Pop_Tier[2];
            Current_Pop_Tier[3] = Necropolis_Pop_Tier[3];
            Current_Pop_Tier[4] = Necropolis_Pop_Tier[4];
            Current_Pop_Tier[5] = Necropolis_Pop_Tier[5];
            Current_Pop_Tier[6] = Necropolis_Pop_Tier[6];
            Current_Pop_Tier[7] = Necropolis_Pop_Tier[7];
            UI_Reset();
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
        }

        private void UpDown_Tier1_ValueChanged(object sender, EventArgs e)
        {
            UpDown();
            Gold_Maximum();
        }
        private void UpDown_Tier2_ValueChanged(object sender, EventArgs e)
        {
            UpDown();
            Gold_Maximum();
        }
        private void UpDown_Tier3_ValueChanged(object sender, EventArgs e)
        {
            UpDown();
            Gold_Maximum();
        }
        private void UpDown_Tier4_ValueChanged(object sender, EventArgs e)
        {
            UpDown();
            Gold_Maximum();
        }
        private void UpDown_Tier5_ValueChanged(object sender, EventArgs e)
        {
            UpDown();
            Gold_Maximum();
        }
        private void UpDown_Tier6_ValueChanged(object sender, EventArgs e)
        {
            UpDown();
            Gold_Maximum();
        }
        private void UpDown_Tier7_ValueChanged(object sender, EventArgs e)
        {
            UpDown();
            Gold_Maximum();
        }

        public void UpDown()
        {
            Label_Tier1.Text = Convert.ToString(Convert.ToInt32(Gold_Tier1.Text) * UpDown_Tier1.Value);
            Label_Tier2.Text = Convert.ToString(Convert.ToInt32(Gold_Tier2.Text) * UpDown_Tier2.Value);
            Label_Tier3.Text = Convert.ToString(Convert.ToInt32(Gold_Tier3.Text) * UpDown_Tier3.Value);
            Label_Tier4.Text = Convert.ToString(Convert.ToInt32(Gold_Tier4.Text) * UpDown_Tier4.Value);
            Label_Tier5.Text = Convert.ToString(Convert.ToInt32(Gold_Tier5.Text) * UpDown_Tier5.Value);
            Label_Tier6.Text = Convert.ToString(Convert.ToInt32(Gold_Tier6.Text) * UpDown_Tier6.Value);
            Label_Tier7.Text = Convert.ToString(Convert.ToInt32(Gold_Tier7.Text) * UpDown_Tier7.Value);
            Label_GoldTotal.Text = Convert.ToString(Convert.ToInt32(Label_Tier1.Text) + Convert.ToInt32(Label_Tier2.Text) + Convert.ToInt32(Label_Tier3.Text) + Convert.ToInt32(Label_Tier4.Text) + Convert.ToInt32(Label_Tier5.Text) + Convert.ToInt32(Label_Tier6.Text) + Convert.ToInt32(Label_Tier7.Text));
        }

        private void Check_Citadel_CheckedChanged(object sender, EventArgs e)
        {
            if (Check_Citadel.Checked == true)
            {
                Current_Pop_Tier[1] = (int)(Current_Pop_Tier[1] * 1.5);
                Current_Pop_Tier[2] = (int)(Current_Pop_Tier[1] * 1.5);
                Current_Pop_Tier[3] = (int)(Current_Pop_Tier[1] * 1.5);
                Current_Pop_Tier[4] = (int)(Current_Pop_Tier[1] * 1.5);
                Current_Pop_Tier[5] = (int)(Current_Pop_Tier[1] * 1.5);
                Current_Pop_Tier[6] = (int)(Current_Pop_Tier[1] * 1.5);
                Current_Pop_Tier[7] = (int)(Current_Pop_Tier[1] * 1.5);
                Pop_Tier1.Text = Convert.ToString(Current_Pop_Tier[1]);
                Pop_Tier2.Text = Convert.ToString(Current_Pop_Tier[2]);
                Pop_Tier3.Text = Convert.ToString(Current_Pop_Tier[3]);
                Pop_Tier4.Text = Convert.ToString(Current_Pop_Tier[4]);
                Pop_Tier5.Text = Convert.ToString(Current_Pop_Tier[5]);
                Pop_Tier6.Text = Convert.ToString(Current_Pop_Tier[6]);
                Pop_Tier7.Text = Convert.ToString(Current_Pop_Tier[7]);
            }
            else
            {
                Pop_Tier1.Text = Convert.ToString(Current_Pop_Tier[1]);
                Pop_Tier2.Text = Convert.ToString(Current_Pop_Tier[2]);
                Pop_Tier3.Text = Convert.ToString(Current_Pop_Tier[3]);
                Pop_Tier4.Text = Convert.ToString(Current_Pop_Tier[4]);
                Pop_Tier5.Text = Convert.ToString(Current_Pop_Tier[5]);
                Pop_Tier6.Text = Convert.ToString(Current_Pop_Tier[6]);
                Pop_Tier7.Text = Convert.ToString(Current_Pop_Tier[7]);
            }
            UpDown_WeekLimit_ValueChanged(null, null);
        }
        private void Check_Castle_CheckedChanged(object sender, EventArgs e)
        {
            if (Check_Castle.Checked == true)
            {
                Check_Citadel.Enabled = false;
                Check_Citadel.Checked = false;
                Current_Pop_Tier[1] = Current_Pop_Tier[1] * 2;
                Current_Pop_Tier[2] = Current_Pop_Tier[2] * 2;
                Current_Pop_Tier[3] = Current_Pop_Tier[3] * 2;
                Current_Pop_Tier[4] = Current_Pop_Tier[4] * 2;
                Current_Pop_Tier[5] = Current_Pop_Tier[5] * 2;
                Current_Pop_Tier[6] = Current_Pop_Tier[6] * 2;
                Current_Pop_Tier[7] = Current_Pop_Tier[7] * 2;
                Pop_Tier1.Text = Convert.ToString(Current_Pop_Tier[1]);
                Pop_Tier2.Text = Convert.ToString(Current_Pop_Tier[2]);
                Pop_Tier3.Text = Convert.ToString(Current_Pop_Tier[3]);
                Pop_Tier4.Text = Convert.ToString(Current_Pop_Tier[4]);
                Pop_Tier5.Text = Convert.ToString(Current_Pop_Tier[5]);
                Pop_Tier6.Text = Convert.ToString(Current_Pop_Tier[6]);
                Pop_Tier7.Text = Convert.ToString(Current_Pop_Tier[7]);
            }
            else
            {
                Check_Citadel.Enabled = true;
                Current_Pop_Tier[1] = Current_Pop_Tier[1] / 2;
                Current_Pop_Tier[2] = Current_Pop_Tier[2] / 2;
                Current_Pop_Tier[3] = Current_Pop_Tier[3] / 2;
                Current_Pop_Tier[4] = Current_Pop_Tier[4] / 2;
                Current_Pop_Tier[5] = Current_Pop_Tier[5] / 2;
                Current_Pop_Tier[6] = Current_Pop_Tier[6] / 2;
                Current_Pop_Tier[7] = Current_Pop_Tier[7] / 2;
                Pop_Tier1.Text = Convert.ToString(Current_Pop_Tier[1]);
                Pop_Tier2.Text = Convert.ToString(Current_Pop_Tier[2]);
                Pop_Tier3.Text = Convert.ToString(Current_Pop_Tier[3]);
                Pop_Tier4.Text = Convert.ToString(Current_Pop_Tier[4]);
                Pop_Tier5.Text = Convert.ToString(Current_Pop_Tier[5]);
                Pop_Tier6.Text = Convert.ToString(Current_Pop_Tier[6]);
                Pop_Tier7.Text = Convert.ToString(Current_Pop_Tier[7]);
            }
            UpDown_WeekLimit_ValueChanged(null, null);
            Creature_Maximum();
        }

        private void Picture_Tier1_01_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame1.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier1.Text = Convert.ToString(Current_Base_Gold_Tier[1]);
            UpDown();
        }
        private void Picture_Tier1_02_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame1.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier1.Text = Convert.ToString(Current_Upg_Gold_Tier[1]);
            UpDown();
        }
        private void Picture_Tier1_03_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame1.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier1.Text = Convert.ToString(Current_Upg_Gold_Tier[1]);
            UpDown();
        }
        private void Picture_Tier2_01_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame2.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier2.Text = Convert.ToString(Current_Base_Gold_Tier[2]);
            UpDown();
        }
        private void Picture_Tier2_02_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame2.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier2.Text = Convert.ToString(Current_Upg_Gold_Tier[2]);
            UpDown();
        }
        private void Picture_Tier2_03_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame2.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier2.Text = Convert.ToString(Current_Upg_Gold_Tier[2]);
            UpDown();
        }
        private void Picture_Tier3_01_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame3.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier3.Text = Convert.ToString(Current_Base_Gold_Tier[3]);
            UpDown();
        }
        private void Picture_Tier3_02_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame3.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier3.Text = Convert.ToString(Current_Upg_Gold_Tier[3]);
            UpDown();
        }
        private void Picture_Tier3_03_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame3.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier3.Text = Convert.ToString(Current_Upg_Gold_Tier[3]);
            UpDown();
        }
        private void Picture_Tier4_01_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame4.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier4.Text = Convert.ToString(Current_Base_Gold_Tier[4]);
            UpDown();
        }
        private void Picture_Tier4_02_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame4.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier4.Text = Convert.ToString(Current_Upg_Gold_Tier[4]);
            UpDown();
        }
        private void Picture_Tier4_03_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame4.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier4.Text = Convert.ToString(Current_Upg_Gold_Tier[4]);
            UpDown();
        }
        private void Picture_Tier5_01_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame5.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier5.Text = Convert.ToString(Current_Base_Gold_Tier[5]);
            UpDown();
        }
        private void Picture_Tier5_02_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame5.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier5.Text = Convert.ToString(Current_Upg_Gold_Tier[5]);
            UpDown();
        }
        private void Picture_Tier5_03_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame5.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier5.Text = Convert.ToString(Current_Upg_Gold_Tier[5]);
            UpDown();
        }
        private void Picture_Tier6_01_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame6.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier6.Text = Convert.ToString(Current_Base_Gold_Tier[6]);
            UpDown();
        }
        private void Picture_Tier6_02_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame6.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier6.Text = Convert.ToString(Current_Upg_Gold_Tier[6]);
            UpDown();
        }
        private void Picture_Tier6_03_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame6.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier6.Text = Convert.ToString(Current_Upg_Gold_Tier[6]);
            UpDown();
        }
        private void Picture_Tier7_01_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame7.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier7.Text = Convert.ToString(Current_Base_Gold_Tier[7]);
            UpDown();
        }
        private void Picture_Tier7_02_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame7.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier7.Text = Convert.ToString(Current_Upg_Gold_Tier[7]);
            UpDown();
        }
        private void Picture_Tier7_03_Click(object sender, EventArgs e)
        {
            Control Clicked = (Control)sender;
            Frame7.Location = new Point(Clicked.Left - 3, Clicked.Top - 3);
            Gold_Tier7.Text = Convert.ToString(Current_Upg_Gold_Tier[7]);
            UpDown();
        }

        private void Factions_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Clicked = (ComboBox)sender;
            switch (Convert.ToString(Clicked.SelectedItem))
            {
                case "Haven":
                    //Current_Haven();
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
                Pop_Tier1.Text = Convert.ToString(Current_Pop_Tier[1]);
                Pop_Tier2.Text = Convert.ToString(Current_Pop_Tier[2]);
                Pop_Tier3.Text = Convert.ToString(Current_Pop_Tier[3]);
                Pop_Tier4.Text = Convert.ToString(Current_Pop_Tier[4]);
                Pop_Tier5.Text = Convert.ToString(Current_Pop_Tier[5]);
                Pop_Tier6.Text = Convert.ToString(Current_Pop_Tier[6]);
                Pop_Tier7.Text = Convert.ToString(Current_Pop_Tier[7]);
            }
            Creature_Maximum();
        }
        private void UpDown_WeekLimit_ValueChanged(object sender, EventArgs e)
        {
            Pop_Tier1.Text = Convert.ToString(Current_Pop_Tier[1] * UpDown_WeekLimit.Value);
            Pop_Tier2.Text = Convert.ToString(Current_Pop_Tier[2] * UpDown_WeekLimit.Value);
            Pop_Tier3.Text = Convert.ToString(Current_Pop_Tier[3] * UpDown_WeekLimit.Value);
            Pop_Tier4.Text = Convert.ToString(Current_Pop_Tier[4] * UpDown_WeekLimit.Value);
            Pop_Tier5.Text = Convert.ToString(Current_Pop_Tier[5] * UpDown_WeekLimit.Value);
            Pop_Tier6.Text = Convert.ToString(Current_Pop_Tier[6] * UpDown_WeekLimit.Value);
            Pop_Tier7.Text = Convert.ToString(Current_Pop_Tier[7] * UpDown_WeekLimit.Value);
            Creature_Maximum();
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

        public void Creature_Maximum()
        {
            if (Check_WeekLimit.Checked == true)
            {
                UpDown_Tier1.Maximum = Convert.ToInt32(Pop_Tier1.Text);
                UpDown_Tier2.Maximum = Convert.ToInt32(Pop_Tier2.Text);
                UpDown_Tier3.Maximum = Convert.ToInt32(Pop_Tier3.Text);
                UpDown_Tier4.Maximum = Convert.ToInt32(Pop_Tier4.Text);
                UpDown_Tier5.Maximum = Convert.ToInt32(Pop_Tier5.Text);
                UpDown_Tier6.Maximum = Convert.ToInt32(Pop_Tier6.Text);
                UpDown_Tier7.Maximum = Convert.ToInt32(Pop_Tier7.Text);
            }
            else
            {
                UpDown_Tier1.Maximum = 99999999999;
                UpDown_Tier2.Maximum = 99999999999;
                UpDown_Tier3.Maximum = 99999999999;
                UpDown_Tier4.Maximum = 99999999999;
                UpDown_Tier5.Maximum = 99999999999;
                UpDown_Tier6.Maximum = 99999999999;
                UpDown_Tier7.Maximum = 99999999999;
            }
        }
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


    }
}