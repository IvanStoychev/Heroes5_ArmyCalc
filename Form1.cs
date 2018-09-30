using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Heroes5_ArmyCalc
{
    public partial class FormMain : Form
    {
        /* Introduction:
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
        public Label[] LabelsGoldArray = new Label[8];
        /// <summary>
        /// Array holding references to all the 'creature population' labels of the form.
        /// Padded with an empty zero index for ease of use.
        /// </summary>
        public Label[] LabelsPopulationArray = new Label[8];
        /// <summary>
        /// Array holding references to all the 'creature total gold cost' labels of the form.
        /// Padded with an empty zero index for ease of use.
        /// </summary>
        public Label[] LabelsTotalArray = new Label[8];
        /// <summary>
        /// Array holding references to all the creature tiers' NumericUpDowns.
        /// Padded with an empty zero index for ease of use.
        /// </summary>
        public NumericUpDown[] NumericUpDownsArray = new NumericUpDown[8];
        /// <summary>
        /// Array of booleans that designate if an upgraded variant is selected or not for each creature tier
        /// </summary>
        public bool[] IsUpgraded = new bool[8];
        /// <summary>
        /// A List with information about all faction units in the game.
        /// </summary>
        List<Unit> Units = new List<Unit>();

        public FormMain()
        {
            InitializeComponent();
            LoadUnitData();
            AssignArrays();
            cbFaction.SelectedItem = "Academy";
            pbCreature_Tier1_01.Image = (Image)Properties.Resources.ResourceManager.GetObject("Sylvan_Tier7_01");
        }

        /// <summary>
        /// Loads the unit data from the database.
        /// </summary>
        void LoadUnitData()
        {
            using (ModelEntities db = new ModelEntities())
            {
                Units = (from unit in db.Units
                        select new Unit
                        {
                            ID = unit.ID,
                            NameBase = unit.NameBase,
                            NameLeft = unit.NameLeft,
                            NameRight = unit.NameRight,
                            Faction = unit.Faction,
                            Tier = unit.Tier,
                            Description = unit.Description,
                            ImageBase = unit.ImageBase,
                            ImageLeft = unit.ImageLeft,
                            ImageRight = unit.ImageRight,
                            GoldCostBase = unit.GoldCostBase,
                            GoldCostUpg = unit.GoldCostUpg,
                            PopulationBase = unit.PopulationBase
                        }).ToList();
            }
        }

        /// <summary>
        /// Assigns form controls to elements of arrays for ease of access.
        /// </summary>
        void AssignArrays()
        {
            LabelsPopulationArray[0] = null;
            LabelsGoldArray[0] = null;
            LabelsTotalArray[0] = null;
            NumericUpDownsArray[0] = null;
            for (int i = 1; i <= 7; i++)
            {
                LabelsPopulationArray[i] = (Label)Controls.Find("lblPopulation_Tier" + i, true).FirstOrDefault();
                LabelsGoldArray[i] = (Label)Controls.Find("lblGoldPrice_Tier" + i, true).FirstOrDefault();
                LabelsTotalArray[i] = (Label)Controls.Find("lblTotal_Tier" + i, true).FirstOrDefault();
                NumericUpDownsArray[i] = (NumericUpDown)Controls.Find("udCreatureAmount_Tier" + i, true).FirstOrDefault();
            }
        }

        /// <summary>
        /// Updates the faction dwellings and every tier's cost, population
        /// and cost of selected creatures labels.
        /// </summary>
        public void LabelsUpdate()
        {
            string FactionName = (string)cbFaction.SelectedItem;
            int Gold_Total = 0;

            // The "Dungeon" faction has a set of specific controls that are only visible if it is the
            // selected faction and the corresponding checkbox is checked.
            udExtraBloodMaiden.Visible = FactionName == "Dungeon" && chkDwelling1.Checked;
            udExtraMinotaur.Visible = FactionName == "Dungeon" && chkDwelling1.Checked;
            lblExtraBloodMaiden.Visible = FactionName == "Dungeon" && chkDwelling1.Checked;
            lblExtraMinotaur.Visible = FactionName == "Dungeon" && chkDwelling1.Checked;

            // Sets the text for each tier's creature gold cost, creature population,
            // total cost of selected creatures and the gold cost of all chosen creatures.
            for (int i = 1; i <= 7; i++)
            {
                Unit currentUnit = GetUnit(FactionName, i);
                // Whenever the image of an upgraded creature is clicked, the boolean variable "IsUpgraded"
                // for that tier is set to "true". Here a check is made whether "IsUpgraded" is true and
                // the gold cost of the creature is changed appropriately.
                if (IsUpgraded[i])
                    LabelsGoldArray[i].Text = currentUnit.GoldCostUpg.ToString();
                else
                    LabelsGoldArray[i].Text = currentUnit.GoldCostBase.ToString();

                // Sets the labels for creature population in accordance with a checked Castle or Citadel.
                if (chkCastle.Checked)
				{
					LabelsPopulationArray[i].Text = Convert.ToString(currentUnit.PopulationBase * udWeeksLimit.Value * 2);
				}
				else if (chkCitadel.Checked)
				{
					LabelsPopulationArray[i].Text = Convert.ToString(Math.Floor((int)currentUnit.PopulationBase * (int)udWeeksLimit.Value * 1.5));
                }
				else
				{
					LabelsPopulationArray[i].Text = Convert.ToString(currentUnit.PopulationBase * udWeeksLimit.Value);
				}

                // Each creature's total gold cost is calculated and the Label's text set.
                LabelsTotalArray[i].Text = Convert.ToString(Convert.ToInt32(LabelsGoldArray[i].Text) * NumericUpDownsArray[i].Value);

                // Each tier's total cost is added to the gost cost of all creatures.
                Gold_Total += Convert.ToInt32(LabelsTotalArray[i].Text);
            }

            //Dwelling_Checked();
            //if (Dwelling_Tier_A != 0 && Dwelling_CheckBox1.Checked == true) lblPopulation_Tier[Dwelling_Tier_A].Text = Convert.ToString(Convert.ToInt32(lblPopulation_Tier[Dwelling_Tier_A].Text) + (Dwelling[Faction, 0] * (int)udCreatures_WeekLimit.Value));
            //if (Dwelling_Tier_B != 0 && (Dwelling_CheckBox2.Checked == true || (Convert.ToString(Factions.SelectedItem) == "Dungeon" && Dwelling_CheckBox1.Checked == true))) lblPopulation_Tier[Dwelling_Tier_B].Text = Convert.ToString(Convert.ToInt32(lblPopulation_Tier[Dwelling_Tier_B].Text) + (Dwelling[Faction, 1] * (int)udCreatures_WeekLimit.Value));
            
            //The Total Gold text label is updated with the appropriate value and the check for the gold constraint is performed
            lblGoldTotal.Text = Convert.ToString(Gold_Total);
			Gold_Maximum();
            BonusDwellingTier1 = 0;
            BonusDwellingTier2 = 0;

            SetUpDownMaximums();
        }

        /// <summary>
        /// Updates the visibility of lblLimitGoldExceeded.
        /// </summary>
        public void Gold_Maximum()
        {
            lblLimitGoldExceeded.Visible = (chkLimitGold.Checked && Convert.ToInt32(lblGoldTotal.Text) > udLimitGold.Value);
        }

        #region Current_Methods
        //These methods change the creature pictures and bonus dwelling text for each faction
        public void Current_Academy()
        {
            pbCreature_Tier1_01.Image = Properties.Resources.Academy_Tier1_01;
            pbCreature_Tier1_02.Image = Properties.Resources.Academy_Tier1_02;
            pbCreature_Tier1_03.Image = Properties.Resources.Academy_Tier1_03;
            pbCreature_Tier2_01.Image = Properties.Resources.Academy_Tier2_01;
            pbCreature_Tier2_02.Image = Properties.Resources.Academy_Tier2_02;
            pbCreature_Tier2_03.Image = Properties.Resources.Academy_Tier2_03;
            pbCreature_Tier3_01.Image = Properties.Resources.Academy_Tier3_01;
            pbCreature_Tier3_02.Image = Properties.Resources.Academy_Tier3_02;
            pbCreature_Tier3_03.Image = Properties.Resources.Academy_Tier3_03;
            pbCreature_Tier4_01.Image = Properties.Resources.Academy_Tier4_01;
            pbCreature_Tier4_02.Image = Properties.Resources.Academy_Tier4_02;
            pbCreature_Tier4_03.Image = Properties.Resources.Academy_Tier4_03;
            pbCreature_Tier5_01.Image = Properties.Resources.Academy_Tier5_01;
            pbCreature_Tier5_02.Image = Properties.Resources.Academy_Tier5_02;
            pbCreature_Tier5_03.Image = Properties.Resources.Academy_Tier5_03;
            pbCreature_Tier6_01.Image = Properties.Resources.Academy_Tier6_01;
            pbCreature_Tier6_02.Image = Properties.Resources.Academy_Tier6_02;
            pbCreature_Tier6_03.Image = Properties.Resources.Academy_Tier6_03;
            pbCreature_Tier7_01.Image = Properties.Resources.Academy_Tier7_01;
            pbCreature_Tier7_02.Image = Properties.Resources.Academy_Tier7_02;
            pbCreature_Tier7_03.Image = Properties.Resources.Academy_Tier7_03;
            chkDwelling1.Text = "Treasure Cave";
            chkDwelling1.Visible = true;
            chkDwelling2.Text = "";
            chkDwelling2.Visible = false;
        }
        public void Current_Dungeon()
        {
            pbCreature_Tier1_01.Image = Properties.Resources.Dungeon_Tier1_01;
            pbCreature_Tier1_02.Image = Properties.Resources.Dungeon_Tier1_02;
            pbCreature_Tier1_03.Image = Properties.Resources.Dungeon_Tier1_03;
            pbCreature_Tier2_01.Image = Properties.Resources.Dungeon_Tier2_01;
            pbCreature_Tier2_02.Image = Properties.Resources.Dungeon_Tier2_02;
            pbCreature_Tier2_03.Image = Properties.Resources.Dungeon_Tier2_03;
            pbCreature_Tier3_01.Image = Properties.Resources.Dungeon_Tier3_01;
            pbCreature_Tier3_02.Image = Properties.Resources.Dungeon_Tier3_02;
            pbCreature_Tier3_03.Image = Properties.Resources.Dungeon_Tier3_03;
            pbCreature_Tier4_01.Image = Properties.Resources.Dungeon_Tier4_01;
            pbCreature_Tier4_02.Image = Properties.Resources.Dungeon_Tier4_02;
            pbCreature_Tier4_03.Image = Properties.Resources.Dungeon_Tier4_03;
            pbCreature_Tier5_01.Image = Properties.Resources.Dungeon_Tier5_01;
            pbCreature_Tier5_02.Image = Properties.Resources.Dungeon_Tier5_02;
            pbCreature_Tier5_03.Image = Properties.Resources.Dungeon_Tier5_03;
            pbCreature_Tier6_01.Image = Properties.Resources.Dungeon_Tier6_01;
            pbCreature_Tier6_02.Image = Properties.Resources.Dungeon_Tier6_02;
            pbCreature_Tier6_03.Image = Properties.Resources.Dungeon_Tier6_03;
            pbCreature_Tier7_01.Image = Properties.Resources.Dungeon_Tier7_01;
            pbCreature_Tier7_02.Image = Properties.Resources.Dungeon_Tier7_02;
            pbCreature_Tier7_03.Image = Properties.Resources.Dungeon_Tier7_03;
            chkDwelling1.Text = "Ritual Pit";
            chkDwelling1.Visible = true;
            chkDwelling2.Text = "";
            chkDwelling2.Visible = false;
        }
        public void Current_Fortress()
        {
            pbCreature_Tier1_01.Image = Properties.Resources.Fortress_Tier1_01;
            pbCreature_Tier1_02.Image = Properties.Resources.Fortress_Tier1_02;
            pbCreature_Tier1_03.Image = Properties.Resources.Fortress_Tier1_03;
            pbCreature_Tier2_01.Image = Properties.Resources.Fortress_Tier2_01;
            pbCreature_Tier2_02.Image = Properties.Resources.Fortress_Tier2_02;
            pbCreature_Tier2_03.Image = Properties.Resources.Fortress_Tier2_03;
            pbCreature_Tier3_01.Image = Properties.Resources.Fortress_Tier3_01;
            pbCreature_Tier3_02.Image = Properties.Resources.Fortress_Tier3_02;
            pbCreature_Tier3_03.Image = Properties.Resources.Fortress_Tier3_03;
            pbCreature_Tier4_01.Image = Properties.Resources.Fortress_Tier4_01;
            pbCreature_Tier4_02.Image = Properties.Resources.Fortress_Tier4_02;
            pbCreature_Tier4_03.Image = Properties.Resources.Fortress_Tier4_03;
            pbCreature_Tier5_01.Image = Properties.Resources.Fortress_Tier5_01;
            pbCreature_Tier5_02.Image = Properties.Resources.Fortress_Tier5_02;
            pbCreature_Tier5_03.Image = Properties.Resources.Fortress_Tier5_03;
            pbCreature_Tier6_01.Image = Properties.Resources.Fortress_Tier6_01;
            pbCreature_Tier6_02.Image = Properties.Resources.Fortress_Tier6_02;
            pbCreature_Tier6_03.Image = Properties.Resources.Fortress_Tier6_03;
            pbCreature_Tier7_01.Image = Properties.Resources.Fortress_Tier7_01;
            pbCreature_Tier7_02.Image = Properties.Resources.Fortress_Tier7_02;
            pbCreature_Tier7_03.Image = Properties.Resources.Fortress_Tier7_03;
            chkDwelling1.Text = "Wrestler's Arena";
            chkDwelling1.Visible = true;
            chkDwelling2.Text = "Runic Sanctuary";
            chkDwelling2.Visible = true;
        }
        public void Current_Haven()
        {
            pbCreature_Tier1_01.Image = Properties.Resources.Haven_Tier1_01;
            pbCreature_Tier1_02.Image = Properties.Resources.Haven_Tier1_02;
            pbCreature_Tier1_03.Image = Properties.Resources.Haven_Tier1_03;
            pbCreature_Tier2_01.Image = Properties.Resources.Haven_Tier2_01;
            pbCreature_Tier2_02.Image = Properties.Resources.Haven_Tier2_02;
            pbCreature_Tier2_03.Image = Properties.Resources.Haven_Tier2_03;
            pbCreature_Tier3_01.Image = Properties.Resources.Haven_Tier3_01;
            pbCreature_Tier3_02.Image = Properties.Resources.Haven_Tier3_02;
            pbCreature_Tier3_03.Image = Properties.Resources.Haven_Tier3_03;
            pbCreature_Tier4_01.Image = Properties.Resources.Haven_Tier4_01;
            pbCreature_Tier4_02.Image = Properties.Resources.Haven_Tier4_02;
            pbCreature_Tier4_03.Image = Properties.Resources.Haven_Tier4_03;
            pbCreature_Tier5_01.Image = Properties.Resources.Haven_Tier5_01;
            pbCreature_Tier5_02.Image = Properties.Resources.Haven_Tier5_02;
            pbCreature_Tier5_03.Image = Properties.Resources.Haven_Tier5_03;
            pbCreature_Tier6_01.Image = Properties.Resources.Haven_Tier6_01;
            pbCreature_Tier6_02.Image = Properties.Resources.Haven_Tier6_02;
            pbCreature_Tier6_03.Image = Properties.Resources.Haven_Tier6_03;
            pbCreature_Tier7_01.Image = Properties.Resources.Haven_Tier7_01;
            pbCreature_Tier7_02.Image = Properties.Resources.Haven_Tier7_02;
            pbCreature_Tier7_03.Image = Properties.Resources.Haven_Tier7_03;
            chkDwelling1.Text = "Farms";
            chkDwelling1.Visible = true;
            chkDwelling2.Text = "";
            chkDwelling2.Visible = false;
        }
        public void Current_Inferno()
        {
            pbCreature_Tier1_01.Image = Properties.Resources.Inferno_Tier1_01;
            pbCreature_Tier1_02.Image = Properties.Resources.Inferno_Tier1_02;
            pbCreature_Tier1_03.Image = Properties.Resources.Inferno_Tier1_03;
            pbCreature_Tier2_01.Image = Properties.Resources.Inferno_Tier2_01;
            pbCreature_Tier2_02.Image = Properties.Resources.Inferno_Tier2_02;
            pbCreature_Tier2_03.Image = Properties.Resources.Inferno_Tier2_03;
            pbCreature_Tier3_01.Image = Properties.Resources.Inferno_Tier3_01;
            pbCreature_Tier3_02.Image = Properties.Resources.Inferno_Tier3_02;
            pbCreature_Tier3_03.Image = Properties.Resources.Inferno_Tier3_03;
            pbCreature_Tier4_01.Image = Properties.Resources.Inferno_Tier4_01;
            pbCreature_Tier4_02.Image = Properties.Resources.Inferno_Tier4_02;
            pbCreature_Tier4_03.Image = Properties.Resources.Inferno_Tier4_03;
            pbCreature_Tier5_01.Image = Properties.Resources.Inferno_Tier5_01;
            pbCreature_Tier5_02.Image = Properties.Resources.Inferno_Tier5_02;
            pbCreature_Tier5_03.Image = Properties.Resources.Inferno_Tier5_03;
            pbCreature_Tier6_01.Image = Properties.Resources.Inferno_Tier6_01;
            pbCreature_Tier6_02.Image = Properties.Resources.Inferno_Tier6_02;
            pbCreature_Tier6_03.Image = Properties.Resources.Inferno_Tier6_03;
            pbCreature_Tier7_01.Image = Properties.Resources.Inferno_Tier7_01;
            pbCreature_Tier7_02.Image = Properties.Resources.Inferno_Tier7_02;
            pbCreature_Tier7_03.Image = Properties.Resources.Inferno_Tier7_03;
            chkDwelling1.Text = "Spawn of Chaos";
            chkDwelling1.Visible = true;
            chkDwelling2.Text = "Halls of Horror";
            chkDwelling2.Visible = true;
        }
        public void Current_Necropolis()
        {
            pbCreature_Tier1_01.Image = Properties.Resources.Necropolis_Tier1_01;
            pbCreature_Tier1_02.Image = Properties.Resources.Necropolis_Tier1_02;
            pbCreature_Tier1_03.Image = Properties.Resources.Necropolis_Tier1_03;
            pbCreature_Tier2_01.Image = Properties.Resources.Necropolis_Tier2_01;
            pbCreature_Tier2_02.Image = Properties.Resources.Necropolis_Tier2_02;
            pbCreature_Tier2_03.Image = Properties.Resources.Necropolis_Tier2_03;
            pbCreature_Tier3_01.Image = Properties.Resources.Necropolis_Tier3_01;
            pbCreature_Tier3_02.Image = Properties.Resources.Necropolis_Tier3_02;
            pbCreature_Tier3_03.Image = Properties.Resources.Necropolis_Tier3_03;
            pbCreature_Tier4_01.Image = Properties.Resources.Necropolis_Tier4_01;
            pbCreature_Tier4_02.Image = Properties.Resources.Necropolis_Tier4_02;
            pbCreature_Tier4_03.Image = Properties.Resources.Necropolis_Tier4_03;
            pbCreature_Tier5_01.Image = Properties.Resources.Necropolis_Tier5_01;
            pbCreature_Tier5_02.Image = Properties.Resources.Necropolis_Tier5_02;
            pbCreature_Tier5_03.Image = Properties.Resources.Necropolis_Tier5_03;
            pbCreature_Tier6_01.Image = Properties.Resources.Necropolis_Tier6_01;
            pbCreature_Tier6_02.Image = Properties.Resources.Necropolis_Tier6_02;
            pbCreature_Tier6_03.Image = Properties.Resources.Necropolis_Tier6_03;
            pbCreature_Tier7_01.Image = Properties.Resources.Necropolis_Tier7_01;
            pbCreature_Tier7_02.Image = Properties.Resources.Necropolis_Tier7_02;
            pbCreature_Tier7_03.Image = Properties.Resources.Necropolis_Tier7_03;
            chkDwelling1.Text = "Unearthed Graves";
            chkDwelling1.Visible = true;
            chkDwelling2.Text = "Dragon Tombstone";
            chkDwelling2.Visible = true;
        }
        public void Current_Stronghold()
        {
            pbCreature_Tier1_01.Image = Properties.Resources.Stronghold_Tier1_01;
            pbCreature_Tier1_02.Image = Properties.Resources.Stronghold_Tier1_02;
            pbCreature_Tier1_03.Image = Properties.Resources.Stronghold_Tier1_03;
            pbCreature_Tier2_01.Image = Properties.Resources.Stronghold_Tier2_01;
            pbCreature_Tier2_02.Image = Properties.Resources.Stronghold_Tier2_02;
            pbCreature_Tier2_03.Image = Properties.Resources.Stronghold_Tier2_03;
            pbCreature_Tier3_01.Image = Properties.Resources.Stronghold_Tier3_01;
            pbCreature_Tier3_02.Image = Properties.Resources.Stronghold_Tier3_02;
            pbCreature_Tier3_03.Image = Properties.Resources.Stronghold_Tier3_03;
            pbCreature_Tier4_01.Image = Properties.Resources.Stronghold_Tier4_01;
            pbCreature_Tier4_02.Image = Properties.Resources.Stronghold_Tier4_02;
            pbCreature_Tier4_03.Image = Properties.Resources.Stronghold_Tier4_03;
            pbCreature_Tier5_01.Image = Properties.Resources.Stronghold_Tier5_01;
            pbCreature_Tier5_02.Image = Properties.Resources.Stronghold_Tier5_02;
            pbCreature_Tier5_03.Image = Properties.Resources.Stronghold_Tier5_03;
            pbCreature_Tier6_01.Image = Properties.Resources.Stronghold_Tier6_01;
            pbCreature_Tier6_02.Image = Properties.Resources.Stronghold_Tier6_02;
            pbCreature_Tier6_03.Image = Properties.Resources.Stronghold_Tier6_03;
            pbCreature_Tier7_01.Image = Properties.Resources.Stronghold_Tier7_01;
            pbCreature_Tier7_02.Image = Properties.Resources.Stronghold_Tier7_02;
            pbCreature_Tier7_03.Image = Properties.Resources.Stronghold_Tier7_03;
            chkDwelling1.Text = "Garbage Pile";
            chkDwelling1.Visible = true;
            chkDwelling2.Text = "";
            chkDwelling2.Visible = false;
        }
        public void Current_Sylvan()
        {
            pbCreature_Tier1_01.Image = Properties.Resources.Sylvan_Tier1_01;
            pbCreature_Tier1_02.Image = Properties.Resources.Sylvan_Tier1_02;
            pbCreature_Tier1_03.Image = Properties.Resources.Sylvan_Tier1_03;
            pbCreature_Tier2_01.Image = Properties.Resources.Sylvan_Tier2_01;
            pbCreature_Tier2_02.Image = Properties.Resources.Sylvan_Tier2_02;
            pbCreature_Tier2_03.Image = Properties.Resources.Sylvan_Tier2_03;
            pbCreature_Tier3_01.Image = Properties.Resources.Sylvan_Tier3_01;
            pbCreature_Tier3_02.Image = Properties.Resources.Sylvan_Tier3_02;
            pbCreature_Tier3_03.Image = Properties.Resources.Sylvan_Tier3_03;
            pbCreature_Tier4_01.Image = Properties.Resources.Sylvan_Tier4_01;
            pbCreature_Tier4_02.Image = Properties.Resources.Sylvan_Tier4_02;
            pbCreature_Tier4_03.Image = Properties.Resources.Sylvan_Tier4_03;
            pbCreature_Tier5_01.Image = Properties.Resources.Sylvan_Tier5_01;
            pbCreature_Tier5_02.Image = Properties.Resources.Sylvan_Tier5_02;
            pbCreature_Tier5_03.Image = Properties.Resources.Sylvan_Tier5_03;
            pbCreature_Tier6_01.Image = Properties.Resources.Sylvan_Tier6_01;
            pbCreature_Tier6_02.Image = Properties.Resources.Sylvan_Tier6_02;
            pbCreature_Tier6_03.Image = Properties.Resources.Sylvan_Tier6_03;
            pbCreature_Tier7_01.Image = Properties.Resources.Sylvan_Tier7_01;
            pbCreature_Tier7_02.Image = Properties.Resources.Sylvan_Tier7_02;
            pbCreature_Tier7_03.Image = Properties.Resources.Sylvan_Tier7_03;
            chkDwelling1.Text = "Blooming Grove";
            chkDwelling1.Visible = true;
            chkDwelling2.Text = "Treant Saplings";
            chkDwelling2.Visible = true;
        }
        #endregion

        /// <summary>
        /// Returns a unit in the given faction and tier or
        /// null if none are found.
        /// </summary>
        /// <param name="faction">The faction of the unit.</param>
        /// <param name="tier">The unit's tier.</param>
        /// <returns>A Unit object of the given tier and faction or null.</returns>
        Unit GetUnit(string faction, int tier)
        {
            return Units.Where(u => u.Faction == faction && u.Tier == tier).FirstOrDefault();
        }

        private void UpDown(object sender, EventArgs e)
        {
            LabelsUpdate();
        }
        private void Citadel_Check_Change(object sender, EventArgs e)
        {
            // If the user unchecked the "Citadel" CheckBox
            // also uncheck the "Castle" one.
            if (chkCastle.Checked == true && chkCitadel.Checked == false)
            {
                chkCastle.Checked = false;
            }
            LabelsUpdate();
        }
        private void Castle_Check_Change(object sender, EventArgs e)
        {
            // If the user checked the "Castle" CheckBox but the
            // "Citadel" one isn't checked - check it.
            if (chkCastle.Checked == true && chkCitadel.Checked == false)
            {
                chkCitadel.Checked = true;
            }
            LabelsUpdate();
        }
        private void Factions_SelectedIndexChanged(object sender, EventArgs e) // [???] Refactor
        {
            LabelsUpdate();
            ComboBox _sender = (ComboBox)sender;
            chkDwelling1.Checked = false;
            chkDwelling2.Checked = false;
            switch (Convert.ToString(_sender.SelectedItem))
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
            udWeeksLimit.Enabled = chkLimitPopulation.Checked;
            if (!chkLimitPopulation.Checked) udWeeksLimit.Value = 1;

            LabelsUpdate();
        }
        private void Check_GoldLimit_CheckedChanged(object sender, EventArgs e)
        {
            udLimitGold.Enabled = chkLimitGold.Checked;
            if (chkLimitGold.Checked) Gold_Maximum();
        }
        private void udCreatures_GoldLimit_ValueChanged(object sender, EventArgs e)
        {
            Gold_Maximum();
        }
        private void Picture_Click(object sender, EventArgs e)
        {
            // Get the clicked PictureBox. From its name find which tier it belongs to
            // and whether it is the first, second or third in that tier. With that
            // information get the frame picture for that tier, move it to the clicked
            // PictureBox and set the IsUpgraded variable appropriately.
            Control _sender = (Control)sender;
            string controlName = _sender.Name;
            int tierStringIndex = controlName.IndexOf("Tier") + 4;
            int controlTier = Convert.ToInt32(controlName.Substring(tierStringIndex, 1));
            int pictureIndex = Convert.ToInt32(controlName.Substring(controlName.Length - 1));
            string frameName = "pbFrame_Tier" + controlTier;
            PictureBox frame = (PictureBox)Controls.Find(frameName, true).FirstOrDefault();
            frame.Location = new Point(_sender.Left - 3, _sender.Top - 3);

            IsUpgraded[controlTier] = pictureIndex > 1 ? true : false;

            LabelsUpdate();
        }

        /// <summary>
        /// Sets the maximum value for each tier's
        /// NumericUpDown control.
        /// </summary>
        private void SetUpDownMaximums()
        {
            for (int i = 1; i <= 7; i++)
            {
                int tierPopulation = Convert.ToInt32(LabelsPopulationArray[i].Text);
                NumericUpDownsArray[i].Maximum = tierPopulation + (99999 * Convert.ToInt32(!chkLimitPopulation.Checked));
            }
        }
    }
}