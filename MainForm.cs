using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MHXXSaveEditor.Data;
using MHXXSaveEditor.Util;
using System.Linq;

namespace MHXXSaveEditor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        Player player = new Player();
        string filePath;
        byte[] saveFile;
        int currentPlayer;
        int itemSelectedSlot;

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
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

            // To see which character slots are enabled
            if (saveFile[4] == 1) // First slot
            {
                currentPlayer = 1;
                toolStripMenuItem2.Enabled = true;
                toolStripMenuItem2.Checked = true;
            }
            if (saveFile[5] == 1) // Second slot
            {
                toolStripMenuItem3.Enabled = true;
            }
            if (saveFile[6] == 1) // Third slot
            {
                toolStripMenuItem4.Enabled = true;
            }

            saveToolStripMenuItem.Enabled = true; // Enables the save toolstrip once save is loaded

            // Extract data from save file
            var ext = new DataExtractor();
            ext.getInfo(saveFile, currentPlayer, player);

            loadSave(); // Load save file data into editor
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if(currentPlayer == 1)
            {
            }
            else
            {
                currentPlayer = 1;
                toolStripMenuItem3.Checked = false;
                toolStripMenuItem4.Checked = false;
                loadSave();
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (currentPlayer == 2)
            {
            }
            else
            {
                currentPlayer = 2;
                toolStripMenuItem2.Checked = false;
                toolStripMenuItem4.Checked = false;
                loadSave();
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (currentPlayer == 3)
            {
            }
            else
            {
                currentPlayer = 1;
                toolStripMenuItem2.Checked = false;
                toolStripMenuItem3.Checked = false;
                loadSave();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            packSaveFile();
            File.WriteAllBytes(filePath, saveFile);
            MessageBox.Show("File saved", "Saved !");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by Ukee for GBATemp\nBased off APM's MHX/MHGen Save editor\nAlso thanks to Seth VanHeulen", "About");
        }

        public void loadSave()
        {
            // General Info
            charNameBox.Text = player.Name;
            numericUpDownTime.Value = player.PlayTime;
            numericUpDownFunds.Value = player.Funds;
            numericUpDownHR.Value = player.HunterRank;
            numericUpDownHRP.Value = player.HRPoints;
            numericUpDownWyc.Value = player.AcademyPoints;

            TimeSpan time = TimeSpan.FromSeconds(player.PlayTime);
            labelConvTime.Text = "D.HH:MM:SS - " + time.ToString();

            // Player
            numericUpDownVoice.Value = Convert.ToInt32(player.Voice);
            numericUpDownEyeColor.Value = Convert.ToInt32(player.EyeColor);
            numericUpDownClothing.Value = Convert.ToInt32(player.Clothing);
            comboBoxGender.SelectedIndex = Convert.ToInt32(player.Gender);
            numericUpDownHair.Value = Convert.ToInt32(player.HairStyle);
            numericUpDownFace.Value = Convert.ToInt32(player.Face);
            numericUpDownFeatures.Value = Convert.ToInt32(player.Features);

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

            // Item Box
            for (int a = 0; a < 2300; a++)
            {
                string itemName = GameConstants.ItemNameList[Array.IndexOf(GameConstants.ItemIDList , Convert.ToInt32(player.itemId[a]))];
                string[] arr = new string[4];
                ListViewItem itm;
                arr[0] = (a + 1).ToString();
                arr[1] = itemName;
                arr[2] = player.itemCount[a];
                itm = new ListViewItem(arr);
                listViewItem.Items.Add(itm);
            }
            listViewItem.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewItem.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            comboBoxItem.Items.AddRange(GameConstants.ItemNameList);
        }

        public void packSaveFile()
        {
            // Char Name
            byte[] charNameByte = new byte[32];
            byte[] convName = Encoding.UTF8.GetBytes(charNameBox.Text);
            Array.Copy(convName, 0, charNameByte, 0, convName.Length);
            Array.Copy(charNameByte, 0, saveFile, player.SaveOffset, Constants.SIZEOF_NAME);

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

            // Play Time
            byte[] playTime = BitConverter.GetBytes((int)numericUpDownTime.Value);
            Array.Copy(playTime, 0, saveFile, player.SaveOffset + Offsets.PLAY_TIME_OFFSET, 4);
            Array.Copy(playTime, 0, saveFile, player.SaveOffset + Offsets.PLAY_TIME_OFFSET2, 4);

            // Character Faatures
            saveFile[player.SaveOffset + Offsets.CHARACTER_GENDER_OFFSET] = (byte)comboBoxGender.SelectedIndex;
            saveFile[player.SaveOffset + Offsets.CHARACTER_GENDER_OFFSET2] = (byte)comboBoxGender.SelectedIndex;
            saveFile[player.SaveOffset + Offsets.CHARACTER_GENDER_OFFSET3] = (byte)comboBoxGender.SelectedIndex;
            saveFile[player.SaveOffset + Offsets.CHARACTER_VOICE_OFFSET] = (byte)numericUpDownVoice.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_VOICE_OFFSET2] = (byte)numericUpDownVoice.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_VOICE_OFFSET3] = (byte)numericUpDownVoice.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_EYE_COLOR_OFFSET] = (byte)numericUpDownEyeColor.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_EYE_COLOR_OFFSET2] = (byte)numericUpDownEyeColor.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_EYE_COLOR_OFFSET3] = (byte)numericUpDownEyeColor.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_CLOTHING_OFFSET] = (byte)numericUpDownClothing.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_CLOTHING_OFFSET2] = (byte)numericUpDownClothing.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_CLOTHING_OFFSET3] = (byte)numericUpDownClothing.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_HAIRSTYLE_OFFSET] = (byte)numericUpDownHair.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_HAIRSTYLE_OFFSET2] = (byte)numericUpDownHair.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_HAIRSTYLE_OFFSET3] = (byte)numericUpDownHair.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_FACE_OFFSET] = (byte)numericUpDownFace.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_FACE_OFFSET2] = (byte)numericUpDownFace.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_FACE_OFFSET3] = (byte)numericUpDownFace.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_FEATURES_OFFSET] = (byte)numericUpDownFeatures.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_FEATURES_OFFSET2] = (byte)numericUpDownFeatures.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_FEATURES_OFFSET3] = (byte)numericUpDownFeatures.Value;

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
            string itemBinary = "0000";

            foreach (ListViewItem i in listViewItem.Items)
            {
                int iteration = Convert.ToInt32(i.SubItems[0].Text) - 1;
                
                player.itemId[iteration] = (GameConstants.ItemIDList[Array.IndexOf(GameConstants.ItemNameList, i.SubItems[1].Text)]).ToString();
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
        }

        private void listViewItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            // To clear the inputs; reset them
            //numericUpDownItemID.Value = 0;
            //numericUpDownItemAmount.Value = 0;
            //comboBoxItem.SelectedIndex = 0;

            if (listViewItem.SelectedItems.Count == 0) // Check if anything was selected
                return;
            else
            {
                itemSelectedSlot = Convert.ToInt32(listViewItem.SelectedItems[0].SubItems[0].Text) - 1;
                numericUpDownItemID.Value = Convert.ToInt32(player.itemId[itemSelectedSlot]);
                numericUpDownItemAmount.Value = Convert.ToInt32(player.itemCount[itemSelectedSlot]);
                comboBoxItem.SelectedIndex = Array.IndexOf(GameConstants.ItemIDList, Convert.ToInt32(player.itemId[itemSelectedSlot]));
            }
        }

        private void comboBoxItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems.Count == 0) // Check if anything was selected
                return;
            else
            {
                int index = GameConstants.ItemIDList[Array.IndexOf(GameConstants.ItemNameList, comboBoxItem.Text)];
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
                player.itemId[itemSelectedSlot] = GameConstants.ItemIDList[Array.IndexOf(GameConstants.ItemNameList, comboBoxItem.Text)].ToString();
            }
        }

        private void numericUpDownItemAmount_ValueChanged(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems.Count == 0) // Check if anything was selected
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

        //private void numericUpDownItemID_ValueChanged(object sender, EventArgs e)
        //{
        //    if (listViewItem.SelectedItems.Count == 1)
        //    {
        //        if (numericUpDownItemID.Value == 0)
        //        {
        //            listViewItem.SelectedItems[0].SubItems[1].Text = "-----";
        //            listViewItem.SelectedItems[0].SubItems[2].Text = "0";
        //            player.itemCount[itemSelectedSlot] = listViewItem.SelectedItems[0].SubItems[2].Text;
        //            player.itemId[itemSelectedSlot] = itemSelectedSlot.ToString();
        //        }
        //        else
        //        {
        //            comboBoxItem.SelectedIndex = (int)numericUpDownItemID.Value;
        //            if (Convert.ToInt32(player.itemCount[itemSelectedSlot]) == 0)
        //                numericUpDownItemAmount.Value = 1;
        //            else
        //                numericUpDownItemAmount.Value = Convert.ToInt32(player.itemCount[itemSelectedSlot]);
        //            listViewItem.SelectedItems[0].SubItems[1].Text = comboBoxItem.Text;
        //            listViewItem.SelectedItems[0].SubItems[2].Text = numericUpDownItemAmount.Value.ToString();
        //            player.itemCount[itemSelectedSlot] = numericUpDownItemAmount.Value.ToString();
        //            player.itemId[itemSelectedSlot] = numericUpDownItemID.Value.ToString();
        //        }
        //    }
        //}

    }
}
