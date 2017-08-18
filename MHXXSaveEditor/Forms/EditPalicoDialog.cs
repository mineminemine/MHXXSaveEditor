using System;
using System.Text;
using System.Windows.Forms;
using MHXXSaveEditor.Data;

namespace MHXXSaveEditor.Forms
{
    public partial class EditPalicoDialog : Form
    {
        private MainForm mainForm;
        int selectedPalico;

        public EditPalicoDialog(MainForm mainForm, string palicoName, int selectedSlot)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            selectedPalico = selectedSlot;
            this.Text = "Editing Palico - " + palicoName;
            comboBoxForte.Items.AddRange(GameConstants.PalicoForte);
            comboBoxTarget.Items.AddRange(GameConstants.PalicoTarget);
            LoadPalico();
            LoadEquippedActions();
            LoadEquippedSkills();
            LoadLearnedActions();
            LoadLearnedSkills();
        }

        public void LoadPalico()
        {
            // General
            byte[] palicoNameByte, palicoPreviousOwnerByte, palicoNameGiverByte;
            palicoNameByte = palicoPreviousOwnerByte = palicoNameGiverByte = new byte[Constants.SIZEOF_NAME];
            byte[] palicoExpByte, palicoOriginalOwnerIDByte;
            palicoExpByte = palicoOriginalOwnerIDByte = new byte[4];
            byte[] palicoForteSpecificIDByte = new byte[3];
            byte[] palicoGreetingByte = new byte[Constants.TOTAL_PALICO_GREETING];
            string palicoName, palicoNameGiver, palicoPreviousOwner, palicoGreeting, palicoActionRNG, palicoSkillRNG;
            string palicoForteSpecificID = "", palicoOriginalOwnerID = "";
            int palicoForte, palicoExp, palicoLevel, palicoEnthusiasm, palicoTarget, palicoUniqueID;

            Array.Copy(mainForm.player.PalicoData, selectedPalico * Constants.SIZEOF_PALICO, palicoNameByte, 0, Constants.SIZEOF_NAME);
            palicoName = Encoding.UTF8.GetString(palicoNameByte);
            Array.Copy(mainForm.player.PalicoData, (selectedPalico * Constants.SIZEOF_PALICO) + 0x20, palicoExpByte, 0, 4);
            palicoExp = (int)BitConverter.ToUInt32(palicoExpByte, 0);
            palicoLevel = Convert.ToInt32(mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x24]);
            palicoForte = Convert.ToInt32(mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x25]);
            palicoEnthusiasm = Convert.ToInt32(mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x26]);
            palicoTarget = Convert.ToInt32(mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x27]);
            Array.Copy(mainForm.player.PalicoData, (selectedPalico * Constants.SIZEOF_PALICO) + 0x58, palicoOriginalOwnerIDByte, 0, 4);
            //Array.Reverse(palicoOriginalOwnerIDByte);
            foreach (var x in palicoOriginalOwnerIDByte)
            {
                palicoOriginalOwnerID += x.ToString("X2");
            }
            Array.Copy(mainForm.player.PalicoData, (selectedPalico * Constants.SIZEOF_PALICO) + 0x5c, palicoForteSpecificIDByte, 0, 3);
            //Array.Reverse(palicoForteSpecificIDByte);
            foreach (var x in palicoForteSpecificIDByte)
            {
                palicoForteSpecificID += x.ToString("X2");
            }
            palicoUniqueID = Convert.ToInt32(mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x5f]);
            Array.Copy(mainForm.player.PalicoData, (selectedPalico * Constants.SIZEOF_PALICO) + 0x60, palicoGreetingByte, 0, Constants.TOTAL_PALICO_GREETING);
            palicoGreeting = Encoding.UTF8.GetString(palicoGreetingByte);
            Array.Copy(mainForm.player.PalicoData, (selectedPalico * Constants.SIZEOF_PALICO) + 0x9c, palicoNameGiverByte, 0, Constants.SIZEOF_NAME);
            palicoNameGiver = Encoding.UTF8.GetString(palicoNameGiverByte);
            Array.Copy(mainForm.player.PalicoData, (selectedPalico * Constants.SIZEOF_PALICO) + 0xbc, palicoPreviousOwnerByte, 0, Constants.SIZEOF_NAME);
            palicoPreviousOwner = Encoding.UTF8.GetString(palicoPreviousOwnerByte);

            palicoActionRNG = mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x54].ToString("X2") + mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x55].ToString("X2");
            palicoSkillRNG = mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x56].ToString("X2") + mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x57].ToString("X2");
            int pAR, pSR;

            if (palicoForte == 0)
            {
                pAR = Array.IndexOf(GameConstants.PalicoCharismaActionRNG, palicoActionRNG);
                comboBoxActionRNG.Items.AddRange(GameConstants.PalicoCharismaActionRNGAbbv);
            }
            else
            {
                pAR = Array.IndexOf(GameConstants.PalicoActionRNG, palicoActionRNG);
                comboBoxActionRNG.Items.AddRange(GameConstants.PalicoActionRNGAbbv);
            }
            comboBoxSkillRNG.Items.AddRange(GameConstants.PalicoSkillRNGAbbv);
            pSR = Array.IndexOf(GameConstants.PalicoSkillRNG, palicoSkillRNG);
            comboBoxActionRNG.SelectedIndex = pAR;
            comboBoxSkillRNG.SelectedIndex = pSR;

            textBoxName.Text = palicoName;
            numericUpDownExp.Value = palicoExp;
            numericUpDownLevel.Value = palicoLevel;
            comboBoxForte.SelectedIndex = palicoForte;
            numericUpDownEnthusiasm.Value = palicoEnthusiasm;
            comboBoxTarget.SelectedIndex = palicoTarget;
            textBoxOriginalOwnerID.Text = palicoOriginalOwnerID.ToString();
            textBoxForteSpecificID.Text = palicoForteSpecificID.ToString();
            textBoxUniquePalicoID.Text = palicoUniqueID.ToString();
            textBoxNameGiver.Text = palicoNameGiver;
            textBoxPreviousOwner.Text = palicoPreviousOwner;
            textBoxGreeting.Text = palicoGreeting;

            // Design
            byte[] palicoCoatRGBA, palicoRightEyeRGBA, palicoLeftEyeRGBA, palicoVestRGBA;
            palicoCoatRGBA = palicoRightEyeRGBA = palicoLeftEyeRGBA = palicoVestRGBA = new byte[4];

            comboBoxVoice.SelectedIndex = Convert.ToInt32(mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x10f]);
            comboBoxEyes.SelectedIndex = Convert.ToInt32(mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x110]);
            comboBoxClothing.SelectedIndex = Convert.ToInt32(mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x111]);
            comboBoxCoat.SelectedIndex = Convert.ToInt32(mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x114]);
            comboBoxEars.SelectedIndex = Convert.ToInt32(mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x115]);
            comboBoxTail.SelectedIndex = Convert.ToInt32(mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x116]);

            Array.Copy(mainForm.player.PalicoData, (selectedPalico * Constants.SIZEOF_PALICO) + 0x11A, palicoCoatRGBA, 0, 4);
            textBoxCoatRGBA.Text = BitConverter.ToString(palicoCoatRGBA).Replace("-", string.Empty);
            Array.Copy(mainForm.player.PalicoData, (selectedPalico * Constants.SIZEOF_PALICO) + 0x11e, palicoRightEyeRGBA, 0, 4);
            textBoxRightEyeRGBA.Text = BitConverter.ToString(palicoRightEyeRGBA).Replace("-", string.Empty);
            Array.Copy(mainForm.player.PalicoData, (selectedPalico * Constants.SIZEOF_PALICO) + 0x122, palicoLeftEyeRGBA, 0, 4);
            textBoxLeftEyeRGBA.Text = BitConverter.ToString(palicoLeftEyeRGBA).Replace("-", string.Empty);
            Array.Copy(mainForm.player.PalicoData, (selectedPalico * Constants.SIZEOF_PALICO) + 0x126, palicoVestRGBA, 0, 4);
            textBoxVestRGBA.Text = BitConverter.ToString(palicoVestRGBA).Replace("-", string.Empty);

            // Status
            // I have no idea what to name the variables for these tbh
            int palicoStatus = Convert.ToInt32(mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0xe0]);
            int palicoTraining = Convert.ToInt32(mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0xe1]);
            int palicoJob = Convert.ToInt32(mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0xe2]);
            int palicoProwler = Convert.ToInt32(mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0xe3]);

            if (palicoProwler == 1)
                labelStatusDetail.Text = "This palico is selected for Prowler Mode";
            else
            {
                if (palicoJob == 8)
                    labelStatusDetail.Text = "This palico is selected for ordering items";
                else if (palicoJob == 16)
                    labelStatusDetail.Text = "This palico is selected for Special Traning/Catnap/Meditation";
                else if (palicoTraining == 4)
                    labelStatusDetail.Text = "Not sure what this palico is doing..";
                else if (palicoTraining == 16)
                    labelStatusDetail.Text = "This palico is selected for Normal Traning";
                else if (palicoJob == 0 && palicoTraining == 0)
                {
                    switch (palicoStatus)
                    {
                        case 0:
                            labelStatusDetail.Text = "This palico is not hired; check with the Palico Sellers";
                            break;
                        case 32:
                            labelStatusDetail.Text = "This palico is resting; not doing anything";
                            break;
                        case 33:
                            labelStatusDetail.Text = "This palico is selected as the First Palico for hunting";
                            break;
                        case 34:
                            labelStatusDetail.Text = "This palico is selected as the Second Palico for hunting";
                            break;
                        case 40:
                            labelStatusDetail.Text = "This palico is on a Meownster Hunt";
                            break;
                        case 160:
                            labelStatusDetail.Text = "This DLC palico is not hired; check with the Palico Sellers";
                            break;
                        case 161:
                            labelStatusDetail.Text = "This DLC palico is resting; not doing anything";
                            break;
                        case 162:
                            labelStatusDetail.Text = "This DLC palico is selected as the First Palico for hunting";
                            break;
                        case 163:
                            labelStatusDetail.Text = "This DLC palico is selected as the Second Palico for hunting";
                            break;
                        case 168:
                            labelStatusDetail.Text = "This DLC palico is on a Meownster Hunt";
                            break;
                        default:
                            labelStatusDetail.Text = "Not sure what this palico is doing [" + palicoStatus + "]";
                            break;
                    }
                }
                else
                    labelStatusDetail.Text = "How did I get here.. [" + palicoJob + "] [" + palicoTraining + "]";
            }
        }

        public void LoadEquippedActions()
        {
            string hexValue, actionName;
            int intValue;
            for (int a = 0; a < 8; a++)
            {
                hexValue = mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x28 + a].ToString("X2");
                intValue = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
                actionName = GameConstants.PalicoSupportMoves[intValue];

                string[] arr = new string[2];
                arr[0] = (a + 1).ToString();
                arr[1] = actionName;
                ListViewItem act = new ListViewItem(arr);
                listViewEquippedActions.Items.Add(act);
            }
            listViewEquippedActions.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewEquippedActions.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        public void LoadLearnedActions()
        {
            string hexValue, actionName;
            int intValue;
            for (int a = 0; a < 16; a++)
            {
                hexValue = mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x38 + a].ToString("X2");
                intValue = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
                actionName = GameConstants.PalicoSupportMoves[intValue];

                string[] arr = new string[2];
                arr[0] = (a + 1).ToString();
                arr[1] = actionName;
                ListViewItem act = new ListViewItem(arr);
                listViewLearnedActions.Items.Add(act);
            }
            listViewLearnedActions.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewLearnedActions.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        public void LoadEquippedSkills()
        {
            string hexValue, skillName;
            int intValue;
            for (int a = 0; a < 8; a++)
            {
                hexValue = mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x30 + a].ToString("X2");
                intValue = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
                skillName = GameConstants.PalicoSkills[intValue];

                string[] arr = new string[2];
                arr[0] = (a + 1).ToString();
                arr[1] = skillName;
                ListViewItem ski = new ListViewItem(arr);
                listViewEquippedSkills.Items.Add(ski);
            }
            listViewEquippedSkills.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewEquippedSkills.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        public void LoadLearnedSkills()
        {
            string hexValue, skillName;
            int intValue;
            for (int a = 0; a < 12; a++)
            {
                hexValue = mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x48 + a].ToString("X2");
                intValue = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
                skillName = GameConstants.PalicoSkills[intValue];

                string[] arr = new string[2];
                arr[0] = (a + 1).ToString();
                arr[1] = skillName;
                ListViewItem ski = new ListViewItem(arr);
                listViewLearnedSkills.Items.Add(ski);
            }
            listViewLearnedSkills.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewLearnedSkills.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void buttonCancel_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        public void updateListViewEquippedActions()
        {
            int actionSelectedSlot;
            actionSelectedSlot = Convert.ToInt32(listViewEquippedActions.SelectedItems[0].SubItems[0].Text) - 1;

            comboBoxEquippedActions.Items.Clear();

            if (actionSelectedSlot == 0)
                comboBoxEquippedActions.Enabled = false;
            else if (actionSelectedSlot == 1 && Convert.ToInt32(mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x25]) == 0)
            {
                comboBoxEquippedActions.Items.AddRange(GameConstants.PalicoSupportMoves);
                comboBoxEquippedActions.Enabled = true;
            }
            else if (actionSelectedSlot == 1)
                comboBoxEquippedActions.Enabled = false;
            else
            {
                comboBoxEquippedActions.Items.AddRange(GameConstants.PalicoSupportMoves);
                comboBoxEquippedActions.Enabled = true;
            }
            comboBoxEquippedActions.SelectedIndex = comboBoxEquippedActions.FindStringExact(listViewEquippedActions.SelectedItems[0].SubItems[1].Text);
        }

        private void listViewEquippedActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEquippedActions.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ListView ls = (ListView)sender;
                if (!ls.Focused)
                {
                    return;
                }

                updateListViewEquippedActions();
            }
        }

        private void comboBoxEquippedActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEquippedActions.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ComboBox cb = (ComboBox)sender;
                if (!cb.Focused)
                {
                    return;
                }

                int actionSelectedSlot, newActionSelected;
                actionSelectedSlot = Convert.ToInt32(listViewEquippedActions.SelectedItems[0].SubItems[0].Text) - 1;
                newActionSelected = comboBoxEquippedActions.SelectedIndex;
                listViewEquippedActions.SelectedItems[0].SubItems[1].Text = comboBoxEquippedActions.Text;
            }
        }

        public void updateListViewEquippedSkills()
        {
            comboBoxEquippedSkills.Items.Clear();

            comboBoxEquippedSkills.Items.AddRange(GameConstants.PalicoSkills);
            comboBoxEquippedSkills.Enabled = true;
            comboBoxEquippedSkills.SelectedIndex = comboBoxEquippedSkills.FindStringExact(listViewEquippedSkills.SelectedItems[0].SubItems[1].Text);
        }

        private void listViewEquippedSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEquippedSkills.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ListView ls = (ListView)sender;
                if (!ls.Focused)
                {
                    return;
                }

                updateListViewEquippedSkills();
            }
        }

        private void comboBoxEquippedSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEquippedSkills.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ComboBox cb = (ComboBox)sender;
                if (!cb.Focused)
                {
                    return;
                }

                int skillSelectedSlot, newSkillSelected;
                skillSelectedSlot = Convert.ToInt32(listViewEquippedSkills.SelectedItems[0].SubItems[0].Text) - 1;
                newSkillSelected = comboBoxEquippedSkills.SelectedIndex;
                listViewEquippedSkills.SelectedItems[0].SubItems[1].Text = comboBoxEquippedSkills.Text;
            }
        }

        public void updateListViewLearnedActions()
        {
            int actionSelectedSlot;
            actionSelectedSlot = Convert.ToInt32(listViewLearnedActions.SelectedItems[0].SubItems[0].Text) - 1;

            comboBoxLearnedActions.Items.Clear();

            if(Convert.ToInt32(mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x25]) == 0)
            {
                if (actionSelectedSlot == 0 || actionSelectedSlot == 1 || actionSelectedSlot == 2)
                    comboBoxLearnedActions.Enabled = false;
                else
                {
                    comboBoxLearnedActions.Items.AddRange(GameConstants.PalicoSupportMoves);
                    comboBoxLearnedActions.Enabled = true;
                }
                comboBoxLearnedActions.SelectedIndex = comboBoxLearnedActions.FindStringExact(listViewLearnedActions.SelectedItems[0].SubItems[1].Text);
            }
            else
            {
                if (actionSelectedSlot == 0 || actionSelectedSlot == 2 || actionSelectedSlot == 3)
                    comboBoxLearnedActions.Enabled = false;
                else if (actionSelectedSlot == 1)
                {
                    switch (Convert.ToInt32(mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x25]))
                    {
                        case 0:
                            comboBoxLearnedActions.Enabled = false;
                            break;
                        case 1:
                            comboBoxEquippedActions.Items.Add("Demon Horn");
                            comboBoxEquippedActions.Items.Add("Piercing Boomerangs");
                            break;
                        case 2:
                            comboBoxEquippedActions.Items.Add("Armor Horn");
                            comboBoxEquippedActions.Items.Add("Emergency Retreat");
                            break;
                        case 3:
                            comboBoxEquippedActions.Items.Add("Cheer Horn");
                            comboBoxEquippedActions.Items.Add("Emergency Retreat");
                            break;
                        case 4:
                            comboBoxEquippedActions.Items.Add("Armor Horn");
                            comboBoxEquippedActions.Items.Add("Cheer Horn");
                            break;
                        case 5:
                            comboBoxEquippedActions.Items.Add("Demon Horn");
                            comboBoxEquippedActions.Items.Add("Camouflage");
                            break;
                        case 6:
                            comboBoxEquippedActions.Items.Add("Piercing Boomerangs");
                            comboBoxEquippedActions.Items.Add("Camouflage");
                            break;
                        case 7:
                            comboBoxEquippedActions.Items.Add("Power Roar");
                            break;
                        default:
                            comboBoxEquippedActions.Items.Add("Something broke here lol");
                            break;
                    }
                }
                else
                {
                    comboBoxLearnedActions.Items.AddRange(GameConstants.PalicoSupportMoves);
                    comboBoxLearnedActions.Enabled = true;
                }
                comboBoxLearnedActions.SelectedIndex = comboBoxLearnedActions.FindStringExact(listViewLearnedActions.SelectedItems[0].SubItems[1].Text);
            }
        }

        private void listViewLearnedActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewLearnedActions.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ListView ls = (ListView)sender;
                if (!ls.Focused)
                {
                    return;
                }

                updateListViewLearnedActions();
            }
        }

        private void comboBoxLearnedActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewLearnedActions.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ComboBox cb = (ComboBox)sender;
                if (!cb.Focused)
                {
                    return;
                }

                int actionSelectedSlot, newActionSelected;
                actionSelectedSlot = Convert.ToInt32(listViewLearnedActions.SelectedItems[0].SubItems[0].Text) - 1;
                newActionSelected = comboBoxLearnedActions.SelectedIndex;
                listViewLearnedActions.SelectedItems[0].SubItems[1].Text = comboBoxLearnedActions.Text;
            }
        }

        public void updateListViewLearnedSkills()
        {
            comboBoxLearnedSkills.Items.Clear();

            comboBoxLearnedSkills.Items.AddRange(GameConstants.PalicoSkills);
            comboBoxLearnedSkills.Enabled = true;
            comboBoxLearnedSkills.SelectedIndex = comboBoxLearnedSkills.FindStringExact(listViewLearnedSkills.SelectedItems[0].SubItems[1].Text);
        }

        private void listViewLearnedSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewLearnedSkills.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ListView ls = (ListView)sender;
                if (!ls.Focused)
                {
                    return;
                }

                updateListViewLearnedSkills();
            }
        }

        private void comboBoxLearnedSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewLearnedSkills.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ComboBox cb = (ComboBox)sender;
                if (!cb.Focused)
                {
                    return;
                }

                int skillSelectedSlot, newSkillSelected;
                skillSelectedSlot = Convert.ToInt32(listViewLearnedSkills.SelectedItems[0].SubItems[0].Text) - 1;
                newSkillSelected = comboBoxLearnedSkills.SelectedIndex;
                listViewLearnedSkills.SelectedItems[0].SubItems[1].Text = comboBoxLearnedSkills.Text;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Name
            byte[] nameByte = new byte[Constants.SIZEOF_NAME];
            Encoding.UTF8.GetBytes(textBoxName.Text, 0, textBoxName.Text.Length, nameByte, 0);
            Array.Copy(nameByte, 0, mainForm.player.PalicoData, selectedPalico * Constants.SIZEOF_PALICO, Constants.SIZEOF_NAME);

            // EXP
            byte[] expByte = BitConverter.GetBytes((int)numericUpDownExp.Value);
            for (int ex = 0; ex < 4; ex++)
            {
                mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x20 + ex] = expByte[ex];
            }

            mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x24] = (byte)numericUpDownLevel.Value;
            mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x25] = (byte)comboBoxForte.SelectedIndex;
            mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x26] = (byte)numericUpDownEnthusiasm.Value;
            mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x27] = (byte)comboBoxTarget.SelectedIndex;
            mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x10f] = (byte)comboBoxVoice.SelectedIndex;
            mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x110] = (byte)comboBoxEyes.SelectedIndex;
            mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x111] = (byte)comboBoxClothing.SelectedIndex;
            mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x114] = (byte)comboBoxCoat.SelectedIndex;
            mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x115] = (byte)comboBoxEars.SelectedIndex;
            mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x116] = (byte)comboBoxTail.SelectedIndex;

            // Action & Skill 
            int pAR = comboBoxActionRNG.SelectedIndex;
            int pSR = comboBoxSkillRNG.SelectedIndex;
            if (comboBoxForte.SelectedIndex == 0)
            {
                mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x54] = (byte)int.Parse(GameConstants.PalicoCharismaActionRNG[pAR].Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x55] = (byte)int.Parse(GameConstants.PalicoCharismaActionRNG[pAR].Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x56] = (byte)int.Parse(GameConstants.PalicoSkillRNG[pSR].Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x57] = (byte)int.Parse(GameConstants.PalicoSkillRNG[pSR].Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            }
            else
            {
                mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x54] = (byte)int.Parse(GameConstants.PalicoActionRNG[pAR].Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x55] = (byte)int.Parse(GameConstants.PalicoActionRNG[pAR].Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x56] = (byte)int.Parse(GameConstants.PalicoSkillRNG[pSR].Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x57] = (byte)int.Parse(GameConstants.PalicoSkillRNG[pSR].Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            // Greeting
            byte[] greetingByte = new byte[Constants.TOTAL_PALICO_GREETING];
            Encoding.UTF8.GetBytes(textBoxGreeting.Text, 0, textBoxGreeting.Text.Length, greetingByte, 0);
            Array.Copy(greetingByte, 0, mainForm.player.PalicoData, (selectedPalico * Constants.SIZEOF_PALICO) + 0x60, Constants.TOTAL_PALICO_GREETING);
            
            // Name Giver
            Array.Clear(nameByte, 0, nameByte.Length);
            Encoding.UTF8.GetBytes(textBoxNameGiver.Text, 0, textBoxNameGiver.Text.Length, nameByte, 0);
            Array.Copy(nameByte, 0, mainForm.player.PalicoData, (selectedPalico * Constants.SIZEOF_PALICO) + 0x9c, Constants.SIZEOF_NAME);

            // Previous Owner
            Array.Clear(nameByte, 0, nameByte.Length);
            Encoding.UTF8.GetBytes(textBoxPreviousOwner.Text, 0, textBoxPreviousOwner.Text.Length, nameByte, 0);
            Array.Copy(nameByte, 0, mainForm.player.PalicoData, (selectedPalico * Constants.SIZEOF_PALICO) + 0xbc, Constants.SIZEOF_NAME);

            // Colors
            int k = 0;
            for(int a = 0; a < 8; a += 2)
            {
                mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x11a + k] = (byte)int.Parse(textBoxCoatRGBA.Text.Substring(a, 2), System.Globalization.NumberStyles.HexNumber);
                mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x11e + k] = (byte)int.Parse(textBoxCoatRGBA.Text.Substring(a, 2), System.Globalization.NumberStyles.HexNumber);
                mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x122 + k] = (byte)int.Parse(textBoxCoatRGBA.Text.Substring(a, 2), System.Globalization.NumberStyles.HexNumber);
                mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x126 + k] = (byte)int.Parse(textBoxCoatRGBA.Text.Substring(a, 2), System.Globalization.NumberStyles.HexNumber);
                k++;
            }

            // Actions
            int intAction;
            int iter = 0;
            foreach (ListViewItem item in listViewEquippedActions.Items)
            {
                intAction = Array.IndexOf(GameConstants.PalicoSupportMoves, item.SubItems[1].Text);
                mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x28 + iter] = (byte)intAction;
                iter++;
            }
            iter = 0;
            foreach (ListViewItem item in listViewLearnedActions.Items)
            {
                intAction = Array.IndexOf(GameConstants.PalicoSupportMoves, item.SubItems[1].Text);
                mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x38 + iter] = (byte)intAction;
                iter++;
            }

            // Skills
            int intSkills;
            iter = 0;
            foreach (ListViewItem item in listViewEquippedSkills.Items)
            {
                intSkills = Array.IndexOf(GameConstants.PalicoSkills, item.SubItems[1].Text);
                mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x30 + iter] = (byte)intSkills;
                iter++;
            }
            iter = 0;
            foreach (ListViewItem item in listViewLearnedSkills.Items)
            {
                intSkills = Array.IndexOf(GameConstants.PalicoSkills, item.SubItems[1].Text);
                mainForm.player.PalicoData[(selectedPalico * Constants.SIZEOF_PALICO) + 0x48 + iter] = (byte)intSkills;
                iter++;
            }

            this.Close();
        }
    }
}
