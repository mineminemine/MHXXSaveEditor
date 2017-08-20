using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using MHXXSaveEditor.Data;
using MHXXSaveEditor.Util;
using MHXXSaveEditor.Forms;

namespace MHXXSaveEditor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.Text = Constants.EDITOR_VERSION; // Changes app title
        }

        public Player player = new Player();
        string filePath;
        byte[] saveFile;
        int currentPlayer, itemSelectedSlot;
        public int equipSelectedSlot, palicoEqpSelectedSlot;

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripMenuItemSaveSlot1.Enabled = false;
            toolStripMenuItemSaveSlot2.Enabled = false;
            toolStripMenuItemSaveSlot3.Enabled = false;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MHXX System File|system|All files (*.*)|*.*";
            ofd.FilterIndex = 1;

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                ofd.Dispose();
                return;
            }

            filePath = ofd.FileName;
            this.Text = string.Format("{0} [{1}]", Constants.EDITOR_VERSION, ofd.SafeFileName); // Changes app title
            saveFile = File.ReadAllBytes(ofd.FileName); // Read all bytes from file into memory buffer
            ofd.Dispose();

            if (saveFile.Length != 4726152) // Check if save file is correct or not
            {
                MessageBox.Show("This is not a MHXX save file, program will now exit.", "Error !");
                if (Application.MessageLoop)
                {
                    Application.Exit();
                }
                else
                {
                    Environment.Exit(1);
                }
            }

            // To see which character slots are enabled
            if (saveFile[4] == 1) // First slot
            {
                currentPlayer = 1;
                toolStripMenuItemSaveSlot1.Enabled = true;
                toolStripMenuItemSaveSlot1.Checked = true;
            }
            if (saveFile[5] == 1 ) // Second slot
            {
                if (currentPlayer != 1)
                {
                    currentPlayer = 2;
                }
                toolStripMenuItemSaveSlot2.Enabled = true;
            }
            if (saveFile[6] == 1) // Third slot
            {
                if (currentPlayer != 1 || currentPlayer != 2)
                {
                    currentPlayer = 3;
                }
                toolStripMenuItemSaveSlot3.Enabled = true;
            }

            saveToolStripMenuItemSave.Enabled = true; // Enables the save toolstrip once save is loaded
            editToolStripMenuItem.Enabled = true;
            tabControlMain.Enabled = true;

            // Extract data from save file
            var ext = new DataExtractor();
            ext.getInfo(saveFile, currentPlayer, player);

            loadSave(); // Load save file data into editor
        }

        private void toolStripMenuItemSaveSlot1_Click(object sender, EventArgs e)
        {
            if(currentPlayer != 1)
            {
                currentPlayer = 1;
                toolStripMenuItemSaveSlot2.Checked = false;
                toolStripMenuItemSaveSlot3.Checked = false;

                var ext = new DataExtractor();
                ext.getInfo(saveFile, currentPlayer, player);
                loadSave();
            }
        }

        private void toolStripMenuItemSaveSlot2_Click(object sender, EventArgs e)
        {
            if (currentPlayer != 2)
            {
                currentPlayer = 2;
                toolStripMenuItemSaveSlot1.Checked = false;
                toolStripMenuItemSaveSlot3.Checked = false;

                var ext = new DataExtractor();
                ext.getInfo(saveFile, currentPlayer, player);
                loadSave();
            }
        }

        private void toolStripMenuItemSaveSlot3_Click(object sender, EventArgs e)
        {
            if (currentPlayer != 3)
            {
                currentPlayer = 1;
                toolStripMenuItemSaveSlot1.Checked = false;
                toolStripMenuItemSaveSlot2.Checked = false;

                var ext = new DataExtractor();
                ext.getInfo(saveFile, currentPlayer, player);
                loadSave();
            }
        }

        private void saveToolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            packSaveFile();
            File.WriteAllBytes(filePath, saveFile);
            MessageBox.Show("File saved", "Saved !");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by Ukee for GBATemp\nBased off APM's MHX/MHGen Save Editor\nAlso thanks to Seth VanHeulen for the Save File structure\nAnd some threads in GBATemp", "About");
        }

        public void loadSave()
        {
            // Item Box // Equipment Box // Palico Equipment Box
            LoadItemBox();
            LoadEquipmentBox();
            LoadPalicoEquipmentBox();

            // General Info
            charNameBox.Text = player.Name;
            numericUpDownTime.Value = player.PlayTime;
            numericUpDownFunds.Value = player.Funds;
            numericUpDownHR.Value = player.HunterRank;
            numericUpDownHRP.Value = player.HRPoints;
            numericUpDownWyc.Value = player.AcademyPoints;
            numericUpDownBhe.Value = player.BhernaPoints;
            numericUpDownKok.Value = player.KokotoPoints;
            numericUpDownPok.Value = player.PokkePoints;
            numericUpDownYuk.Value = player.YukumoPoints;

            TimeSpan time = TimeSpan.FromSeconds(player.PlayTime);
            labelConvTime.Text = "D.HH:MM:SS - " + time.ToString();

            // Player
            numericUpDownVoice.Value = Convert.ToInt32(player.Voice) + 1;
            numericUpDownEyeColor.Value = Convert.ToInt32(player.EyeColor) + 1;
            numericUpDownClothing.Value = Convert.ToInt32(player.Clothing) + 1;
            comboBoxGender.SelectedIndex = Convert.ToInt32(player.Gender);
            numericUpDownHair.Value = Convert.ToInt32(player.HairStyle) + 1;
            numericUpDownFace.Value = Convert.ToInt32(player.Face) + 1;
            numericUpDownFeatures.Value = Convert.ToInt32(player.Features) + 1;

            // Colors
            numericUpDownSkinR.Value = player.SkinColorRGBA[0];
            numericUpDownSkinG.Value = player.SkinColorRGBA[1];
            numericUpDownSkinB.Value = player.SkinColorRGBA[2];
            numericUpDownSkinA.Value = player.SkinColorRGBA[3];
            numericUpDownHairR.Value = player.HairColorRGBA[0];
            numericUpDownHairG.Value = player.HairColorRGBA[1];
            numericUpDownHairB.Value = player.HairColorRGBA[2];
            numericUpDownHairA.Value = player.HairColorRGBA[3];
            numericUpDownFeaturesR.Value = player.FeaturesColorRGBA[0];
            numericUpDownFeaturesG.Value = player.FeaturesColorRGBA[1];
            numericUpDownFeaturesB.Value = player.FeaturesColorRGBA[2];
            numericUpDownFeaturesA.Value = player.FeaturesColorRGBA[3];
            numericUpDownClothesR.Value = player.ClothingColorRGBA[0];
            numericUpDownClothesG.Value = player.ClothingColorRGBA[1];
            numericUpDownClothesB.Value = player.ClothingColorRGBA[2];
            numericUpDownClothesA.Value = player.ClothingColorRGBA[3];

            // Palico
            listViewPalico.Items.Clear();
            for (int a = 0; a < Constants.TOTAL_PALICO_SLOTS; a++)
            {
                if (Convert.ToInt32(player.PalicoData[a * Constants.SIZEOF_PALICO]) != 0) // Check if first character in name != 0, if != 0 means a palico exist in that block (or at least in my opinion)
                {
                    byte[] palicoNameByte = new byte[32];
                    string palicoName, palicoType;

                    Array.Copy(player.PalicoData, a * Constants.SIZEOF_PALICO, palicoNameByte, 0, Constants.SIZEOF_NAME);
                    palicoName = Encoding.UTF8.GetString(palicoNameByte);
                    palicoType = GameConstants.PalicoForte[Convert.ToInt32(player.PalicoData[(a * Constants.SIZEOF_PALICO) + 37])];

                    string[] arr = new string[3];
                    arr[0] = (a + 1).ToString();
                    arr[1] = palicoName;
                    arr[2] = palicoType;
                    ListViewItem plc = new ListViewItem(arr);
                    listViewPalico.Items.Add(plc);
                }
            }
            listViewPalico.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewPalico.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        public void LoadItemBox()
        {
            listViewItem.Items.Clear();
            string itemName;
            for (int a = 0; a < Constants.TOTAL_ITEM_SLOTS; a++) // 2300 slots for 2300 items
            {
                try
                {
                    itemName = GameConstants.ItemNameList[Convert.ToInt32(player.itemId[a])];
                }
                catch
                {
                    MessageBox.Show("An unknown item was found at slot: " + (a + 1).ToString() + "\nYou may have an invalid item in your item box\nIf you proceed to try and edit it, you may get a crash", "Item Error");
                    if (MessageBox.Show("Item ID: " + player.itemId[a], "Click OK to copy this message", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        Clipboard.SetText("Item ID: " + player.itemId[a]);
                    itemName = "Unknown [" + player.itemId[a].ToString() + "]";
                }

                string[] arr = new string[3];
                arr[0] = (a + 1).ToString();
                arr[1] = itemName;
                arr[2] = player.itemCount[a];
                ListViewItem itm = new ListViewItem(arr);
                listViewItem.Items.Add(itm);
            }
            listViewItem.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewItem.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            comboBoxItem.Items.AddRange(GameConstants.ItemNameList);
        }

        public void LoadEquipmentBox()
        {
            listViewEquipment.Items.Clear();
            for (int a = 0; a < Constants.TOTAL_EQUIP_SLOTS; a++) // 2000 slots for 2000 equips
            {
                int eqID = Convert.ToInt32(player.equipmentInfo[(a * 36) + 3].ToString("X2") + player.equipmentInfo[(a * 36) + 2].ToString("X2"), 16);
                string typeLevelBits = Convert.ToString(player.equipmentInfo[(a * 36) + 1], 2).PadLeft(4, '0') + Convert.ToString(player.equipmentInfo[a * 36], 2).PadLeft(8, '0'); // One byte == the eqp type and level; 7 bits level on left hand side, then right hand side 5 bits eq type
                string equipType = GameConstants.EquipmentTypes[Convert.ToInt32(typeLevelBits.Substring(7, 5), 2)];

                string eqName = "(None)";
                try
                {
                    switch (Convert.ToInt32(typeLevelBits.Substring(7, 5), 2))
                    {
                        case 0:
                            break;
                        case 1:
                            eqName = GameConstants.EquipHeadNames[eqID];
                            break;
                        case 2:
                            eqName = GameConstants.EquipChestNames[eqID];
                            break;
                        case 3:
                            eqName = GameConstants.EquipArmsNames[eqID];
                            break;
                        case 4:
                            eqName = GameConstants.EquipWaistNames[eqID];
                            break;
                        case 5:
                            eqName = GameConstants.EquipLegsNames[eqID];
                            break;
                        case 6:
                            eqName = GameConstants.EquipTalismanNames[eqID];
                            break;
                        case 7:
                            eqName = GameConstants.EquipGreatSwordNames[eqID];
                            break;
                        case 8:
                            eqName = GameConstants.EquipSwordnShieldNames[eqID];
                            break;
                        case 9:
                            eqName = GameConstants.EquipHammerNames[eqID];
                            break;
                        case 10:
                            eqName = GameConstants.EquipLanceNames[eqID];
                            break;
                        case 11:
                            eqName = GameConstants.EquipHeavyBowgunNames[eqID];
                            break;
                        case 12:
                            break;
                        case 13:
                            eqName = GameConstants.EquipLightBowgunNames[eqID];
                            break;
                        case 14:
                            eqName = GameConstants.EquipLongswordNames[eqID];
                            break;
                        case 15:
                            eqName = GameConstants.EquipSwitchAxeNames[eqID];
                            break;
                        case 16:
                            eqName = GameConstants.EquipGunlanceNames[eqID];
                            break;
                        case 17:
                            eqName = GameConstants.EquipBowNames[eqID];
                            break;
                        case 18:
                            eqName = GameConstants.EquipDualBladesNames[eqID];
                            break;
                        case 19:
                            eqName = GameConstants.EquipHuntingHornNames[eqID];
                            break;
                        case 20:
                            eqName = GameConstants.EquipInsectGlaiveNames[eqID];
                            break;
                        case 21:
                            eqName = GameConstants.EquipChargeBladeNames[eqID];
                            break;
                    }
                }
                catch
                {
                    string hexes = "";
                    MessageBox.Show("An unknown equipment was found at slot: " + (a + 1).ToString() + "\nYou may have an invalid equipment in your equipment box\nIf you proceed to try and edit it, you may get a crash", "Equipment Error");
                    for(int b = 0; b <36; b++)
                    {
                        hexes += player.equipmentInfo[(a * 36) + b].ToString("X2") + " ";
                    }
                    if (MessageBox.Show(hexes, "Click OK to copy this message", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        Clipboard.SetText(hexes);
                }

                string[] arr = new string[3];
                arr[0] = (a + 1).ToString();
                arr[1] = equipType;
                arr[2] = eqName;
                ListViewItem itm = new ListViewItem(arr);
                listViewEquipment.Items.Add(itm);
            }
            listViewEquipment.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewEquipment.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            comboBoxEquipType.Items.AddRange(GameConstants.EquipmentTypes);
            comboBoxEquipDeco1.Items.AddRange(GameConstants.JwlNames);
            comboBoxEquipDeco2.Items.AddRange(GameConstants.JwlNames);
            comboBoxEquipDeco3.Items.AddRange(GameConstants.JwlNames);
        }

        public void LoadPalicoEquipmentBox()
        {
            listViewPalicoEquipment.Items.Clear();
            for (int a = 0; a < Constants.TOTAL_PALICO_EQUIP; a++) // 1000 slots
            {
                int eqID = Convert.ToInt32(player.equipmentPalico[(a * 36) + 3].ToString("X2") + player.equipmentPalico[(a * 36) + 2].ToString("X2"), 16);
                int equipType = Convert.ToInt32(player.equipmentPalico[(a * 36)]);
                string typeName = "(None)";
                string eqName = "(None)";

                switch (equipType)
                {
                    case 22:
                        eqName = GameConstants.PalicoWeaponNames[eqID];
                        typeName = GameConstants.PalicoEquip[1];
                        break;
                    case 23:
                        eqName = GameConstants.PalicoHeadNames[eqID];
                        typeName = GameConstants.PalicoEquip[2];
                        break;
                    case 24:
                        eqName = GameConstants.PalicoArmorNames[eqID];
                        typeName = GameConstants.PalicoEquip[3];
                        break;
                    default:
                        typeName = GameConstants.PalicoEquip[0];
                        break;
                }

                string[] arr = new string[3];
                arr[0] = (a + 1).ToString();
                arr[1] = typeName;
                arr[2] = eqName;
                ListViewItem eqp = new ListViewItem(arr);
                listViewPalicoEquipment.Items.Add(eqp);
            }
            listViewPalicoEquipment.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewPalicoEquipment.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        public void packSaveFile()
        {
            // Char Name
            byte[] charNameByte = new byte[Constants.SIZEOF_NAME]; // create byte array with size of 32
            byte[] convName = Encoding.UTF8.GetBytes(charNameBox.Text); // get bytes from text box
            Array.Copy(convName, 0, charNameByte, 0, convName.Length); // copy from convname into the charnamebyte (which also leaves the other empty spaces as 00)
            Array.Copy(charNameByte, 0, saveFile, player.SaveOffset, Constants.SIZEOF_NAME); // copy into save file

            // HR Points
            byte[] hrPoints = BitConverter.GetBytes((int)numericUpDownHRP.Value);
            Array.Copy(hrPoints, 0, saveFile, player.SaveOffset + Offsets.HR_POINTS_OFFSET, 4);

            // Funds
            byte[] funds = BitConverter.GetBytes((int)numericUpDownFunds.Value);
            Array.Copy(funds, 0, saveFile, player.SaveOffset + Offsets.FUNDS_OFFSET, 4);
            Array.Copy(funds, 0, saveFile, player.SaveOffset + Offsets.FUNDS_OFFSET2, 4);

            // Academy Points
            byte[] academyPoints = BitConverter.GetBytes((int)numericUpDownWyc.Value);
            Array.Copy(academyPoints, 0, saveFile, player.SaveOffset + Offsets.ACADEMY_POINTS_OFFSET, 4);

            // Village Points
            byte[] villagePoints = BitConverter.GetBytes((int)numericUpDownBhe.Value);
            Array.Copy(villagePoints, 0, saveFile, player.SaveOffset + Offsets.BHERNA_POINTS_OFFSET, 4);
            villagePoints = BitConverter.GetBytes((int)numericUpDownPok.Value);
            Array.Copy(villagePoints, 0, saveFile, player.SaveOffset + Offsets.POKKE_POINTS_OFFSET, 4);
            villagePoints = BitConverter.GetBytes((int)numericUpDownYuk.Value);
            Array.Copy(villagePoints, 0, saveFile, player.SaveOffset + Offsets.YUKUMO_POINTS_OFFSET, 4);
            villagePoints = BitConverter.GetBytes((int)numericUpDownKok.Value);
            Array.Copy(villagePoints, 0, saveFile, player.SaveOffset + Offsets.KOKOTO_POINTS_OFFSET, 4);

            // Play Time
            byte[] playTime = BitConverter.GetBytes((int)numericUpDownTime.Value);
            Array.Copy(playTime, 0, saveFile, player.SaveOffset + Offsets.PLAY_TIME_OFFSET, 4);
            Array.Copy(playTime, 0, saveFile, player.SaveOffset + Offsets.PLAY_TIME_OFFSET2, 4);

            // Character Faatures
            saveFile[player.SaveOffset + Offsets.CHARACTER_GENDER_OFFSET] = (byte)comboBoxGender.SelectedIndex;
            saveFile[player.SaveOffset + Offsets.CHARACTER_GENDER_OFFSET2] = (byte)comboBoxGender.SelectedIndex;
            saveFile[player.SaveOffset + Offsets.CHARACTER_GENDER_OFFSET3] = (byte)comboBoxGender.SelectedIndex;
            saveFile[player.SaveOffset + Offsets.CHARACTER_VOICE_OFFSET] = (byte)(numericUpDownVoice.Value - 1);
            saveFile[player.SaveOffset + Offsets.CHARACTER_VOICE_OFFSET2] = (byte)(numericUpDownVoice.Value - 1);
            saveFile[player.SaveOffset + Offsets.CHARACTER_VOICE_OFFSET3] = (byte)(numericUpDownVoice.Value - 1);
            saveFile[player.SaveOffset + Offsets.CHARACTER_EYE_COLOR_OFFSET] = (byte)(numericUpDownEyeColor.Value - 1);
            saveFile[player.SaveOffset + Offsets.CHARACTER_EYE_COLOR_OFFSET2] = (byte)(numericUpDownEyeColor.Value - 1);
            saveFile[player.SaveOffset + Offsets.CHARACTER_EYE_COLOR_OFFSET3] = (byte)(numericUpDownEyeColor.Value -1);
            saveFile[player.SaveOffset + Offsets.CHARACTER_CLOTHING_OFFSET] = (byte)(numericUpDownClothing.Value - 1);
            saveFile[player.SaveOffset + Offsets.CHARACTER_CLOTHING_OFFSET2] = (byte)(numericUpDownClothing.Value - 1);
            saveFile[player.SaveOffset + Offsets.CHARACTER_CLOTHING_OFFSET3] = (byte)(numericUpDownClothing.Value -1);
            saveFile[player.SaveOffset + Offsets.CHARACTER_HAIRSTYLE_OFFSET] = (byte)(numericUpDownHair.Value - 1);
            saveFile[player.SaveOffset + Offsets.CHARACTER_HAIRSTYLE_OFFSET2] = (byte)(numericUpDownHair.Value - 1);
            saveFile[player.SaveOffset + Offsets.CHARACTER_HAIRSTYLE_OFFSET3] = (byte)(numericUpDownHair.Value - 1);
            saveFile[player.SaveOffset + Offsets.CHARACTER_FACE_OFFSET] = (byte)(numericUpDownFace.Value - 1);
            saveFile[player.SaveOffset + Offsets.CHARACTER_FACE_OFFSET2] = (byte)(numericUpDownFace.Value - 1);
            saveFile[player.SaveOffset + Offsets.CHARACTER_FACE_OFFSET3] = (byte) (numericUpDownFace.Value - 1);
            saveFile[player.SaveOffset + Offsets.CHARACTER_FEATURES_OFFSET] = (byte)(numericUpDownFeatures.Value - 1);
            saveFile[player.SaveOffset + Offsets.CHARACTER_FEATURES_OFFSET2] = (byte)(numericUpDownFeatures.Value - 1);
            saveFile[player.SaveOffset + Offsets.CHARACTER_FEATURES_OFFSET3] = (byte)(numericUpDownFeatures.Value - 1);

            // Colors
            player.SkinColorRGBA[0] = (byte)numericUpDownSkinR.Value;
            player.SkinColorRGBA[1] = (byte)numericUpDownSkinG.Value;
            player.SkinColorRGBA[2] = (byte)numericUpDownSkinB.Value;
            player.SkinColorRGBA[3] = (byte)numericUpDownSkinA.Value;
            Array.Copy(player.SkinColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_SKIN_COLOR_OFFSET, 4);
            Array.Copy(player.SkinColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_SKIN_COLOR_OFFSET2, 4);
            Array.Copy(player.SkinColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_SKIN_COLOR_OFFSET3, 4);
            player.HairColorRGBA[0] = (byte)numericUpDownHairR.Value;
            player.HairColorRGBA[1] = (byte)numericUpDownHairG.Value;
            player.HairColorRGBA[2] = (byte)numericUpDownHairB.Value;
            player.HairColorRGBA[3] = (byte)numericUpDownHairA.Value;
            Array.Copy(player.HairColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_HAIR_COLOR_OFFSET, 4);
            Array.Copy(player.HairColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_HAIR_COLOR_OFFSET2, 4);
            Array.Copy(player.HairColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_HAIR_COLOR_OFFSET3, 4);
            player.FeaturesColorRGBA[0] = (byte)numericUpDownFeaturesR.Value;
            player.FeaturesColorRGBA[1] = (byte)numericUpDownFeaturesG.Value;
            player.FeaturesColorRGBA[2] = (byte)numericUpDownFeaturesB.Value;
            player.FeaturesColorRGBA[3] = (byte)numericUpDownFeaturesA.Value;
            Array.Copy(player.FeaturesColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_FEATURES_COLOR_OFFSET, 4);
            Array.Copy(player.FeaturesColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_FEATURES_COLOR_OFFSET2, 4);
            Array.Copy(player.FeaturesColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_FEATURES_COLOR_OFFSET3, 4);
            player.ClothingColorRGBA[0] = (byte)numericUpDownClothesR.Value;
            player.ClothingColorRGBA[1] = (byte)numericUpDownClothesG.Value;
            player.ClothingColorRGBA[2] = (byte)numericUpDownClothesB.Value;
            player.ClothingColorRGBA[3] = (byte)numericUpDownClothesA.Value;
            Array.Copy(player.ClothingColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_CLOTHING_COLOR_OFFSET, 4);
            Array.Copy(player.ClothingColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_CLOTHING_COLOR_OFFSET2, 4);
            Array.Copy(player.ClothingColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_CLOTHING_COLOR_OFFSET3, 4);

            // Item Box
            string itemBinary = "0000"; // Add back the '0000' that was removed in data extraction

            foreach (ListViewItem i in listViewItem.Items)
            {
                int iteration = Convert.ToInt32(i.SubItems[0].Text) - 1;
                
                player.itemId[iteration] = Array.IndexOf(GameConstants.ItemNameList, i.SubItems[1].Text).ToString();
                player.itemCount[iteration] = i.SubItems[2].Text;
            }
            for (int a = 2299; a >= 0; a--)
            {
                itemBinary += Convert.ToString(Convert.ToInt32(player.itemCount[a]), 2).PadLeft(7, '0');
                itemBinary += Convert.ToString(Convert.ToInt32(player.itemId[a]), 2).PadLeft(12, '0');
            }
            var byteArray = Enumerable.Range(0, int.MaxValue / 8)
                          .Select(i => i * 8)    // get the starting index of which char segment
                          .TakeWhile(i => i < itemBinary.Length)
                          .Select(i => itemBinary.Substring(i, 8)) // get the binary string segments
                          .Select(s => Convert.ToByte(s, 2)) // convert to byte
                          .ToArray();

            Array.Reverse(byteArray);
            Array.Copy(byteArray, 0, saveFile, player.SaveOffset + Offsets.ITEM_BOX_OFFSET, byteArray.Length);

            // Equipment Box
            Array.Copy(player.equipmentInfo, 0, saveFile, player.SaveOffset + Offsets.EQUIPMENT_BOX_OFFSET, Constants.SIZEOF_EQUIPBOX);

            // Palico Equipment Box
            Array.Copy(player.equipmentPalico, 0, saveFile, player.SaveOffset + Offsets.PALICO_EQUIPMENT_OFFSET, Constants.SIZEOF_PALICOEQUIPBOX);

            // Palico
            Array.Copy(player.PalicoData, 0, saveFile, player.SaveOffset + Offsets.PALICO_OFFSET, Constants.SIZEOF_PALICOES);

            // Shoutouts
            Array.Copy(player.ManualShoutouts, 0, saveFile, player.SaveOffset + Offsets.MANUAL_SHOUTOUT_OFFSETS, Constants.SIZEOF_MANUAL_SHOUTOUTS);
            Array.Copy(player.AutomaticShoutouts, 0, saveFile, player.SaveOffset + Offsets.AUTOMATIC_SHOUTOUT_OFFSETS, Constants.SIZEOF_AUTOMATIC_SHOUTOUTS);
        }

        private void listViewItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                itemSelectedSlot = Convert.ToInt32(listViewItem.SelectedItems[0].SubItems[0].Text) - 1;
                numericUpDownItemID.Value = Convert.ToInt32(player.itemId[itemSelectedSlot]);
                numericUpDownItemAmount.Value = Convert.ToInt32(player.itemCount[itemSelectedSlot]);
                comboBoxItem.SelectedIndex = Convert.ToInt32(player.itemId[itemSelectedSlot]);
            }
        }

        private void comboBoxItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                int index = Array.IndexOf(GameConstants.ItemNameList, comboBoxItem.Text);
                numericUpDownItemID.Value = index;
                listViewItem.SelectedItems[0].SubItems[1].Text = GameConstants.ItemNameList[Array.IndexOf(GameConstants.ItemNameList, comboBoxItem.Text)];

                if (listViewItem.SelectedItems[0].SubItems[2].Text == "0" && listViewItem.SelectedItems[0].SubItems[1].Text != "-----")
                {
                    numericUpDownItemAmount.Value = 1;
                    listViewItem.SelectedItems[0].SubItems[2].Text = "1";
                }
                if (comboBoxItem.Text == "-----")
                {
                    numericUpDownItemAmount.Value = 0;
                    numericUpDownItemID.Value = 0;
                    listViewItem.SelectedItems[0].SubItems[2].Text = "0";

                }

                player.itemCount[itemSelectedSlot] = listViewItem.SelectedItems[0].SubItems[2].Text;
                player.itemId[itemSelectedSlot] = Array.IndexOf(GameConstants.ItemNameList, comboBoxItem.Text).ToString();
            }
        }

        private void numericUpDownItemAmount_ValueChanged(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                if (numericUpDownItemAmount.Value == 0)
                {
                    listViewItem.SelectedItems[0].SubItems[1].Text = "-----";
                    listViewItem.SelectedItems[0].SubItems[2].Text = "0";
                    numericUpDownItemID.Value = 0;
                    comboBoxItem.SelectedIndex = 0;
                    player.itemCount[itemSelectedSlot] = "0";
                    player.itemId[itemSelectedSlot] = "0";
                }
                else if (listViewItem.SelectedItems[0].SubItems[1].Text == "-----")
                {
                    listViewItem.SelectedItems[0].SubItems[2].Text = "0";
                    player.itemCount[itemSelectedSlot] = "0";
                    numericUpDownItemAmount.Value = 0;
                }
                else
                {
                    listViewItem.SelectedItems[0].SubItems[2].Text = numericUpDownItemAmount.Value.ToString();
                    player.itemCount[itemSelectedSlot] = numericUpDownItemAmount.Value.ToString();
                }
            }
        }

        private void numericUpDownTime_ValueChanged(object sender, EventArgs e)
        {
            TimeSpan time = TimeSpan.FromSeconds((double)numericUpDownTime.Value);
            labelConvTime.Text = "D.HH:MM:SS - " + time.ToString();
        }

        private void comboBoxEquipType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEquipment.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ComboBox cb = (ComboBox)sender;
                if (!cb.Focused)
                {
                    return;
                }

                comboBoxEquipName.Items.Clear();
                numericUpDownEquipLevel.Value = 1;
                comboBoxEquipDeco1.SelectedIndex = 0;
                comboBoxEquipDeco2.SelectedIndex = 0;
                comboBoxEquipDeco3.SelectedIndex = 0;

                if (comboBoxEquipType.SelectedIndex == 0 || comboBoxEquipType.SelectedIndex == 12)
                {
                    comboBoxEquipName.Items.Clear();
                    comboBoxEquipName.Items.Add("(None)");
                    comboBoxEquipName.Enabled = false;
                    comboBoxEquipDeco1.Enabled = false;
                    comboBoxEquipDeco2.Enabled = false;
                    comboBoxEquipDeco3.Enabled = false;
                    buttonEditKinsect.Enabled = false;
                    buttonEditTalisman.Enabled = false;
                }
                else if (comboBoxEquipType.SelectedIndex == 20)
                {
                    comboBoxEquipName.Enabled = true;
                    comboBoxEquipDeco1.Enabled = true;
                    comboBoxEquipDeco2.Enabled = true;
                    comboBoxEquipDeco3.Enabled = true;
                    buttonEditKinsect.Enabled = true;
                }
                else if (comboBoxEquipType.SelectedIndex == 6)
                {
                    comboBoxEquipName.Enabled = true;
                    comboBoxEquipDeco1.Enabled = true;
                    comboBoxEquipDeco2.Enabled = true;
                    comboBoxEquipDeco3.Enabled = true;
                    buttonEditTalisman.Enabled = true;
                    buttonEditKinsect.Enabled = false;
                }
                else
                {
                    comboBoxEquipName.Enabled = true;
                    comboBoxEquipDeco1.Enabled = true;
                    comboBoxEquipDeco2.Enabled = true;
                    comboBoxEquipDeco3.Enabled = true;
                    buttonEditKinsect.Enabled = false;
                    buttonEditTalisman.Enabled = false;
                }

                switch (comboBoxEquipType.SelectedIndex)
                {
                    case 1:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipHeadNames);
                        break;
                    case 2:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipChestNames);
                        break;
                    case 3:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipArmsNames);
                        break;
                    case 4:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipWaistNames);
                        break;
                    case 5:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipLegsNames);
                        break;
                    case 6:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipTalismanNames);
                        break;
                    case 7:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipGreatSwordNames);
                        break;
                    case 8:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipSwordnShieldNames);
                        break;
                    case 9:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipHammerNames);
                        break;
                    case 10:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipLanceNames);
                        break;
                    case 11:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipHeavyBowgunNames);
                        break;
                    case 13:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipLightBowgunNames);
                        break;
                    case 14:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipLongswordNames);
                        break;
                    case 15:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipSwitchAxeNames);
                        break;
                    case 16:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipGunlanceNames);
                        break;
                    case 17:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipBowNames);
                        break;
                    case 18:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipDualBladesNames);
                        break;
                    case 19:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipHuntingHornNames);
                        break;
                    case 20:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipInsectGlaiveNames);
                        break;
                    case 21:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipChargeBladeNames);
                        break;
                }
                if (comboBoxEquipType.SelectedIndex == 0 || comboBoxEquipType.SelectedIndex == 12)
                    comboBoxEquipName.SelectedIndex = 0;
                else
                    comboBoxEquipName.SelectedIndex = 1;

                listViewEquipment.SelectedItems[0].SubItems[1].Text = comboBoxEquipType.Text;
                listViewEquipment.SelectedItems[0].SubItems[2].Text = comboBoxEquipName.Text;

                // Change the equipment type to selected equip in combobox
                string typeLevelBits = Convert.ToString(player.equipmentInfo[(equipSelectedSlot * 36) + 1], 2).PadLeft(4, '0') + Convert.ToString(player.equipmentInfo[equipSelectedSlot * 36], 2).PadLeft(8, '0'); // One byte == the eqp type and level; 7 bits level on left hand side, then right hand side 5 bits eq type
                string eqType = Convert.ToString(comboBoxEquipType.SelectedIndex, 2).PadLeft(5, '0');
                string newByte = "00000000000" + eqType; // the zeroes are used to empty out the level and reset back to lv 1
                int nBytes = newByte.Length / 8;
                byte[] bytes = new byte[nBytes];
                for (int i = 0; i < nBytes; ++i)
                {
                    bytes[i] = Convert.ToByte(newByte.Substring(8 * i, 8), 2);
                }

                player.equipmentInfo[equipSelectedSlot * 36] = bytes[1];
                player.equipmentInfo[(equipSelectedSlot * 36) + 1] = bytes[0];

                // Resets everything to the first of whatever equip of the selected equipment type
                for (int a = 1; a < 36; a++)
                {
                    player.equipmentInfo[(equipSelectedSlot * 36) + a] = 0; // Clears everything to 0
                }
                player.equipmentInfo[(equipSelectedSlot * 36) + 2] = 1; // Changes Eqp ID to 1, being the first selected eqp in that eqp type
            }
        }

        private void comboBoxEquipDeco1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEquipment.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ComboBox cb = (ComboBox)sender;
                if (!cb.Focused)
                {
                    return;
                }

                byte[] idBytes = BitConverter.GetBytes(GameConstants.JwlIDs[comboBoxEquipDeco1.SelectedIndex]);
                player.equipmentInfo[(equipSelectedSlot * 36) + 7] = idBytes[1];
                player.equipmentInfo[(equipSelectedSlot * 36) + 6] = idBytes[0];
            }
        }

        private void comboBoxEquipDeco2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEquipment.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ComboBox cb = (ComboBox)sender;
                if (!cb.Focused)
                {
                    return;
                }

                byte[] idBytes = BitConverter.GetBytes(GameConstants.JwlIDs[comboBoxEquipDeco2.SelectedIndex]);
                player.equipmentInfo[(equipSelectedSlot * 36) + 9] = idBytes[1];
                player.equipmentInfo[(equipSelectedSlot * 36) + 8] = idBytes[0];
            }
        }

        private void comboBoxEquipDeco3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEquipment.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ComboBox cb = (ComboBox)sender;
                if (!cb.Focused)
                {
                    return;
                }

                byte[] idBytes = BitConverter.GetBytes(GameConstants.JwlIDs[comboBoxEquipDeco3.SelectedIndex]);
                player.equipmentInfo[(equipSelectedSlot * 36) + 11] = idBytes[1];
                player.equipmentInfo[(equipSelectedSlot * 36) + 10] = idBytes[0];
            }
        }

        private void buttonEditKinsect_Click(object sender, EventArgs e)
        {
            EditKinsectDialog editKinsect = new EditKinsectDialog(this, listViewEquipment.SelectedItems[0].SubItems[2].Text);
            MessageBox.Show("Edit at your own risk", "WARNING");
            editKinsect.ShowDialog();
            editKinsect.Dispose();
        }

        private void maxAmountItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in listViewItem.Items)
            {
                if (!i.SubItems[1].Text.Contains("-----"))
                {
                    i.SubItems[2].Text = "99";
                }
            }
        }

        private void buttonEditTalisman_Click(object sender, EventArgs e)
        {
            EditTalismanDialog editKinsect = new EditTalismanDialog(this, listViewEquipment.SelectedItems[0].SubItems[2].Text);
            MessageBox.Show("Edit at your own risk", "WARNING");
            editKinsect.ShowDialog();
            editKinsect.Dispose();
        }

        private void numericUpDownEquipLevel_ValueChanged(object sender, EventArgs e)
        {
            if (listViewEquipment.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                NumericUpDown nmu = (NumericUpDown)sender;
                if (!nmu.Focused)
                {
                    return;
                }

                string typeLevelBits = Convert.ToString(player.equipmentInfo[(equipSelectedSlot * 36) + 1], 2).PadLeft(4, '0') + Convert.ToString(player.equipmentInfo[equipSelectedSlot * 36], 2).PadLeft(8, '0'); // One byte == the eqp type and level; 7 bits level on left hand side, then right hand side 5 bits eq type
                int toChange = (int)numericUpDownEquipLevel.Value - 1;
                string newBinary = "0000" + Convert.ToString(toChange, 2).PadLeft(7, '0') + typeLevelBits.Substring(7, 5);

                int nBytes = newBinary.Length / 8;
                byte[] bytes = new byte[nBytes];
                for (int i = 0; i < nBytes; ++i)
                {
                    bytes[i] = Convert.ToByte(newBinary.Substring(8 * i, 8), 2);
                }

                player.equipmentInfo[equipSelectedSlot * 36] = bytes[1];
                player.equipmentInfo[(equipSelectedSlot * 36) + 1] = bytes[0];
            }
        }

        private void buttonEditPalico_Click(object sender, EventArgs e)
        {
            if (listViewPalico.SelectedItems.Count > 0)
            {
                int selectedSlot = listViewPalico.Items.IndexOf(listViewPalico.SelectedItems[0]);
                EditPalicoDialog editPalico = new EditPalicoDialog(this, listViewPalico.SelectedItems[0].SubItems[1].Text, selectedSlot);
                MessageBox.Show("Edit at your own risk", "WARNING");
                editPalico.ShowDialog();
                editPalico.Dispose();

                byte[] palicoNameByte = new byte[32];
                Array.Copy(player.PalicoData, selectedSlot * Constants.SIZEOF_PALICO, palicoNameByte, 0, Constants.SIZEOF_NAME);
                listViewPalico.SelectedItems[0].SubItems[1].Text = Encoding.UTF8.GetString(palicoNameByte);
                listViewPalico.SelectedItems[0].SubItems[2].Text = GameConstants.PalicoForte[Convert.ToInt32(player.PalicoData[(selectedSlot * Constants.SIZEOF_PALICO) + 37])];
            }
        }

        private void listViewPalico_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewPalico.SelectedItems.Count > 0)
            {
                int selectedSlot = listViewPalico.Items.IndexOf(listViewPalico.SelectedItems[0]);
                EditPalicoDialog editPalico = new EditPalicoDialog(this, listViewPalico.SelectedItems[0].SubItems[1].Text, selectedSlot);
                MessageBox.Show("Edit at your own risk", "WARNING");
                editPalico.ShowDialog();
                editPalico.Dispose();

                byte[] palicoNameByte = new byte[32];
                Array.Copy(player.PalicoData, selectedSlot * Constants.SIZEOF_PALICO, palicoNameByte, 0, Constants.SIZEOF_NAME);
                listViewPalico.SelectedItems[0].SubItems[1].Text = Encoding.UTF8.GetString(palicoNameByte);
                listViewPalico.SelectedItems[0].SubItems[2].Text = GameConstants.PalicoForte[Convert.ToInt32(player.PalicoData[(selectedSlot * Constants.SIZEOF_PALICO) + 37])];
            }
        }

        private void comboBoxPalicoEqpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewPalicoEquipment.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ComboBox cb = (ComboBox)sender;
                if (!cb.Focused)
                {
                    return;
                }

                comboBoxPalicoEquip.Items.Clear();
                listViewPalicoEquipment.SelectedItems[0].SubItems[1].Text = comboBoxPalicoEqpType.Text;
                switch(comboBoxPalicoEqpType.SelectedIndex)
                {
                    case 1:
                        player.equipmentPalico[palicoEqpSelectedSlot * 36] = 22;
                        player.equipmentPalico[(palicoEqpSelectedSlot * 36) + 1] = 0;
                        player.equipmentPalico[(palicoEqpSelectedSlot * 36) + 2] = 0;
                        player.equipmentPalico[(palicoEqpSelectedSlot * 36) + 3] = 0;
                        listViewPalicoEquipment.SelectedItems[0].SubItems[2].Text = "(None)";
                        comboBoxPalicoEquip.Items.AddRange(GameConstants.PalicoWeaponNames);
                        comboBoxPalicoEquip.SelectedIndex = 0;
                        comboBoxPalicoEquip.Enabled = true;
                        break;
                    case 2:
                        player.equipmentPalico[palicoEqpSelectedSlot * 36] = 23;
                        player.equipmentPalico[(palicoEqpSelectedSlot * 36) + 1] = 0;
                        player.equipmentPalico[(palicoEqpSelectedSlot * 36) + 2] = 0;
                        player.equipmentPalico[(palicoEqpSelectedSlot * 36) + 3] = 0;
                        listViewPalicoEquipment.SelectedItems[0].SubItems[2].Text = "(None)";
                        comboBoxPalicoEquip.Items.AddRange(GameConstants.PalicoHeadNames);
                        comboBoxPalicoEquip.SelectedIndex = 0;
                        comboBoxPalicoEquip.Enabled = true;
                        break;
                    case 3:
                        player.equipmentPalico[palicoEqpSelectedSlot * 36] = 24;
                        player.equipmentPalico[(palicoEqpSelectedSlot * 36) + 1] = 0;
                        player.equipmentPalico[(palicoEqpSelectedSlot * 36) + 2] = 0;
                        player.equipmentPalico[(palicoEqpSelectedSlot * 36) + 3] = 0;
                        listViewPalicoEquipment.SelectedItems[0].SubItems[2].Text = "(None)";
                        comboBoxPalicoEquip.Items.AddRange(GameConstants.PalicoArmorNames);
                        comboBoxPalicoEquip.SelectedIndex = 0;
                        comboBoxPalicoEquip.Enabled = true;
                        break;
                    default:
                        player.equipmentPalico[palicoEqpSelectedSlot * 36] = 0;
                        player.equipmentPalico[(palicoEqpSelectedSlot * 36) + 1] = 0;
                        player.equipmentPalico[(palicoEqpSelectedSlot * 36) + 2] = 0;
                        player.equipmentPalico[(palicoEqpSelectedSlot * 36) + 3] = 0;
                        listViewPalicoEquipment.SelectedItems[0].SubItems[2].Text = "(None)";
                        comboBoxPalicoEquip.Items.Add("(None)");
                        comboBoxPalicoEquip.SelectedIndex = 0;
                        comboBoxPalicoEquip.Enabled = false;
                        break;
                }
            }
        }

        private void buttonEditShoutouts_Click(object sender, EventArgs e)
        {
            EditShoutoutsDialog editShoutouts = new EditShoutoutsDialog(this);
            editShoutouts.ShowDialog();
            editShoutouts.Dispose();
        }

        private void charNameBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (!tb.Focused)
            {
                return;
            }

            var mlc = new MaxLengthChecker();
            if (mlc.getMaxLength(charNameBox.Text, 32))
                charNameBox.MaxLength = charNameBox.Text.Length;
        }

        private void comboBoxEquipName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEquipment.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ComboBox cb = (ComboBox)sender;
                if (!cb.Focused)
                {
                    return;
                }

                listViewEquipment.SelectedItems[0].SubItems[2].Text = comboBoxEquipName.Text;
                byte[] idBytes = BitConverter.GetBytes(comboBoxEquipName.SelectedIndex);
                player.equipmentInfo[(equipSelectedSlot * 36) + 2] = idBytes[0];
                player.equipmentInfo[(equipSelectedSlot * 36) + 3] = idBytes[1];
            }
        }

        private void comboBoxPalicoEquip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewPalicoEquipment.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ComboBox cb = (ComboBox)sender;
                if (!cb.Focused)
                {
                    return;
                }

                listViewPalicoEquipment.SelectedItems[0].SubItems[2].Text = comboBoxPalicoEquip.Text;
                byte[] idBytes = BitConverter.GetBytes(comboBoxPalicoEquip.SelectedIndex);
                player.equipmentPalico[(palicoEqpSelectedSlot * 36) + 2] = idBytes[0];
                player.equipmentPalico[(palicoEqpSelectedSlot * 36) + 3] = idBytes[1];
            }
        }

        private void listViewPalicoEquipment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewPalicoEquipment.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ListView ls = (ListView)sender;
                if (!ls.Focused)
                {
                    return;
                }

                comboBoxPalicoEquip.Items.Clear();
                comboBoxPalicoEqpType.Items.Clear();
                comboBoxPalicoEqpType.Items.AddRange(GameConstants.PalicoEquip);
                comboBoxPalicoEqpType.Enabled = true;

                palicoEqpSelectedSlot = Convert.ToInt32(listViewPalicoEquipment.SelectedItems[0].SubItems[0].Text) - 1;
                comboBoxPalicoEqpType.SelectedIndex = comboBoxPalicoEqpType.FindStringExact(listViewPalicoEquipment.SelectedItems[0].SubItems[1].Text);
                if(comboBoxPalicoEqpType.SelectedIndex == 1)
                {
                    comboBoxPalicoEquip.Items.AddRange(GameConstants.PalicoWeaponNames);
                    comboBoxPalicoEquip.Enabled = true;
                }
                else if(comboBoxPalicoEqpType.SelectedIndex == 2)
                {
                    comboBoxPalicoEquip.Items.AddRange(GameConstants.PalicoHeadNames);
                    comboBoxPalicoEquip.Enabled = true;
                }
                else if(comboBoxPalicoEqpType.SelectedIndex == 3)
                {
                    comboBoxPalicoEquip.Items.AddRange(GameConstants.PalicoArmorNames);
                    comboBoxPalicoEquip.Enabled = true;
                }
                else
                {
                    comboBoxPalicoEquip.Items.Clear();
                    comboBoxPalicoEquip.Items.Add("(None)");
                    comboBoxPalicoEquip.Enabled = false;
                }

                int eqID = Convert.ToInt32(player.equipmentPalico[(palicoEqpSelectedSlot * 36) + 3].ToString("X2") + player.equipmentPalico[(palicoEqpSelectedSlot * 36) + 2].ToString("X2"), 16);
                comboBoxPalicoEquip.SelectedIndex = eqID;
            }
        }

        private void listViewEquipment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEquipment.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ListView ls = (ListView)sender;
                if (!ls.Focused)
                {
                    return;
                }

                comboBoxEquipName.Items.Clear();
                comboBoxEquipType.Enabled = true;

                equipSelectedSlot = Convert.ToInt32(listViewEquipment.SelectedItems[0].SubItems[0].Text) - 1;
                string typeLevelBits = Convert.ToString(player.equipmentInfo[(equipSelectedSlot * 36) + 1], 2).PadLeft(4, '0') + Convert.ToString(player.equipmentInfo[equipSelectedSlot * 36], 2).PadLeft(8, '0'); // One byte == the eqp type and level; 7 bits level on left hand side, then right hand side 5 bits eq type
                comboBoxEquipType.SelectedIndex = Convert.ToInt32(typeLevelBits.Substring(7, 5), 2);

                int eqID = Convert.ToInt32(player.equipmentInfo[(equipSelectedSlot * 36) + 3].ToString("X2") + player.equipmentInfo[(equipSelectedSlot * 36) + 2].ToString("X2"), 16);
                int eqLevel = Convert.ToInt32(typeLevelBits.Substring(0, 7), 2);
                int deco1 = Convert.ToInt32(player.equipmentInfo[(equipSelectedSlot * 36) + 7].ToString("X2") + player.equipmentInfo[(equipSelectedSlot * 36) + 6].ToString("X2"), 16);
                int deco2 = Convert.ToInt32(player.equipmentInfo[(equipSelectedSlot * 36) + 9].ToString("X2") + player.equipmentInfo[(equipSelectedSlot * 36) + 8].ToString("X2"), 16);
                int deco3 = Convert.ToInt32(player.equipmentInfo[(equipSelectedSlot * 36) + 11].ToString("X2") + player.equipmentInfo[(equipSelectedSlot * 36) + 10].ToString("X2"), 16);

                if (comboBoxEquipType.SelectedIndex == 0 || comboBoxEquipType.SelectedIndex == 12)
                {
                    comboBoxEquipName.Items.Clear();
                    comboBoxEquipName.Text = "(None)";
                    comboBoxEquipName.Enabled = false;
                    comboBoxEquipDeco1.Enabled = false;
                    comboBoxEquipDeco2.Enabled = false;
                    comboBoxEquipDeco3.Enabled = false;
                    buttonEditKinsect.Enabled = false;
                    buttonEditTalisman.Enabled = false;
                    numericUpDownEquipLevel.Enabled = false;
                }
                else if (comboBoxEquipType.SelectedIndex == 20)
                {
                    comboBoxEquipName.Enabled = true;
                    comboBoxEquipDeco1.Enabled = true;
                    comboBoxEquipDeco2.Enabled = true;
                    comboBoxEquipDeco3.Enabled = true;
                    buttonEditKinsect.Enabled = true;
                    buttonEditTalisman.Enabled = false;
                    numericUpDownEquipLevel.Enabled = true;
                }
                else if (comboBoxEquipType.SelectedIndex == 6)
                {
                    comboBoxEquipName.Enabled = true;
                    comboBoxEquipDeco1.Enabled = true;
                    comboBoxEquipDeco2.Enabled = true;
                    comboBoxEquipDeco3.Enabled = true;
                    buttonEditTalisman.Enabled = true;
                    buttonEditKinsect.Enabled = false;
                    numericUpDownEquipLevel.Enabled = false;
                }
                else
                {
                    comboBoxEquipName.Enabled = true;
                    comboBoxEquipDeco1.Enabled = true;
                    comboBoxEquipDeco2.Enabled = true;
                    comboBoxEquipDeco3.Enabled = true;
                    buttonEditKinsect.Enabled = false;
                    buttonEditTalisman.Enabled = false;
                    numericUpDownEquipLevel.Enabled = true;
                }

                try
                {
                    switch (Convert.ToInt32(typeLevelBits.Substring(7, 5), 2))
                    {
                        case 1:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipHeadNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 2:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipChestNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 3:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipArmsNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 4:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipWaistNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 5:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipLegsNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 6:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipTalismanNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 7:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipGreatSwordNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 8:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipSwordnShieldNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 9:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipHammerNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 10:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipLanceNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 11:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipHeavyBowgunNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 13:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipLightBowgunNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 14:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipLongswordNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 15:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipSwitchAxeNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 16:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipGunlanceNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 17:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipBowNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 18:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipDualBladesNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 19:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipHuntingHornNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 20:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipInsectGlaiveNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                        case 21:
                            comboBoxEquipName.Items.AddRange(GameConstants.EquipChargeBladeNames);
                            comboBoxEquipName.SelectedIndex = eqID;
                            break;
                    }
                }
                catch
                {
                    comboBoxEquipName.Items.Add("Unknown [" + eqID + "]");
                    comboBoxEquipName.SelectedIndex = 0;
                }

                numericUpDownEquipLevel.Value = eqLevel + 1;
                comboBoxEquipDeco1.SelectedIndex = Array.IndexOf(GameConstants.JwlIDs ,deco1);
                comboBoxEquipDeco2.SelectedIndex = Array.IndexOf(GameConstants.JwlIDs, deco2);
                comboBoxEquipDeco3.SelectedIndex = Array.IndexOf(GameConstants.JwlIDs, deco3);
            }
        }
    }
}
