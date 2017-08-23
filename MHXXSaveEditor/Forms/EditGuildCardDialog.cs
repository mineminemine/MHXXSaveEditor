using System;
using System.Text;
using System.Windows.Forms;
using MHXXSaveEditor.Data;

namespace MHXXSaveEditor.Forms
{
    public partial class EditGuildCardDialog : Form
    {
        private MainForm MainForm;

        public EditGuildCardDialog(MainForm mainForm)
        {
            InitializeComponent();
            MainForm = mainForm;
            LoadGeneral();
            LoadQuests();
            LoadWeapons();
            LoadMonsters();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOkay_Click(object sender, EventArgs e)
        {
            byte[] timeByte = BitConverter.GetBytes((int)numericUpDownGCPlayTime.Value);
            for (int a = 0; a < 4; a++)
            {
                MainForm.player.GuildCardData[0x914 + a] = timeByte[a];
            }

            byte[] gcBytes = new byte[textBoxGCID.Text.Length / 2];
            for (int a = 0; a < textBoxGCID.Text.Length; a += 2)
            {
                gcBytes[a / 2] = Convert.ToByte(textBoxGCID.Text.Substring(a, 2), 16);
            }

            for (int a = 0; a < 8; a++)
            {
                MainForm.player.GuildCardData[0x8b0 + a] = gcBytes[a];
            }

            Close();
        }

        public void LoadGeneral()
        {
            byte[] nameByte = new byte[12];
            byte[] timeByte = new byte[4];
            string gcID = "";
            for(int a = 0; a < 12; a++)
            {
                nameByte[a] = MainForm.player.GuildCardData[a];
            }
            for(int a = 0x8b0; a < 0x8b8; a++)
            {
                gcID += MainForm.player.GuildCardData[a].ToString("X2");
            }
            Array.Copy(MainForm.player.GuildCardData, 0x914, timeByte, 0, 4);

            textBoxGCName.Text = Encoding.Unicode.GetString(nameByte);
            textBoxGCID.Text = gcID;
            numericUpDownGCPlayTime.Value = BitConverter.ToInt32(timeByte, 0);
        }

        public void LoadQuests()
        {
            numericUpDownVillageLow.Value = BitConverter.ToInt16(MainForm.player.GuildCardData, 0x85e);
            numericUpDownVillageHigh.Value = BitConverter.ToInt16(MainForm.player.GuildCardData, 0x860);
            numericUpDownHHLow.Value = BitConverter.ToInt16(MainForm.player.GuildCardData, 0x862);
            numericUpDownHHHigh.Value = BitConverter.ToInt16(MainForm.player.GuildCardData, 0x864);
            numericUpDownGRank.Value = BitConverter.ToInt16(MainForm.player.GuildCardData, 0x866);
            numericUpDownSpecialPermit.Value = BitConverter.ToInt16(MainForm.player.GuildCardData, 0x868);
            numericUpDownArena.Value = BitConverter.ToInt16(MainForm.player.GuildCardData, 0x86A);
        }

        public void LoadWeapons()
        {
            comboBoxWeaponType.SelectedIndex = 0;
            numericUpDownVillageCount.Value = BitConverter.ToInt16(MainForm.player.GuildCardData, 0x8BA);
            numericUpDownHubCount.Value = BitConverter.ToInt16(MainForm.player.GuildCardData, 0x8D8);
            numericUpDownArenaCount.Value = BitConverter.ToInt16(MainForm.player.GuildCardData, 0x8F6);
        }

        private void numericUpDownGCPlayTime_ValueChanged(object sender, EventArgs e)
        {
            TimeSpan time = TimeSpan.FromSeconds((double)numericUpDownGCPlayTime.Value);
            labelConvTime.Text = "D.HH:MM:SS - " + time.ToString();
        }

        private void comboBoxWeaponType_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDownVillageCount.Value = BitConverter.ToInt16(MainForm.player.GuildCardData, 0x8BA + (comboBoxWeaponType.SelectedIndex * 2));
            numericUpDownHubCount.Value = BitConverter.ToInt16(MainForm.player.GuildCardData, 0x8D8 + (comboBoxWeaponType.SelectedIndex * 2));
            numericUpDownArenaCount.Value = BitConverter.ToInt16(MainForm.player.GuildCardData, 0x8F6 + (comboBoxWeaponType.SelectedIndex * 2));
        }

        private void numericUpDownVillageCount_ValueChanged(object sender, EventArgs e)
        {
            byte[] numberBytes = BitConverter.GetBytes((int)numericUpDownVillageCount.Value);
            MainForm.player.GuildCardData[0x8BA + (comboBoxWeaponType.SelectedIndex * 2)] = numberBytes[0];
            MainForm.player.GuildCardData[0x8BA + (comboBoxWeaponType.SelectedIndex * 2) + 1] = numberBytes[1];
        }

        private void numericUpDownHubCount_ValueChanged(object sender, EventArgs e)
        {
            byte[] numberBytes = BitConverter.GetBytes((int)numericUpDownHubCount.Value);
            MainForm.player.GuildCardData[0x8D8 + (comboBoxWeaponType.SelectedIndex * 2)] = numberBytes[0];
            MainForm.player.GuildCardData[0x8D8 + (comboBoxWeaponType.SelectedIndex * 2) + 1] = numberBytes[1];
        }

        private void numericUpDownArenaCount_ValueChanged(object sender, EventArgs e)
        {
            byte[] numberBytes = BitConverter.GetBytes((int)numericUpDownArenaCount.Value);
            MainForm.player.GuildCardData[0x8F6 + (comboBoxWeaponType.SelectedIndex * 2)] = numberBytes[0];
            MainForm.player.GuildCardData[0x8F6 + (comboBoxWeaponType.SelectedIndex * 2) + 1] = numberBytes[1];
        }

        public void LoadMonsters()
        {
            comboBoxMonsters.Items.AddRange(GameConstants.MonsterHuntNames);
            comboBoxMonsters.SelectedIndex = 0;
            numericUpDownKillCount.Value = BitConverter.ToInt16(MainForm.player.MonsterKills, 0);
            numericUpDownCaptureCount.Value = BitConverter.ToInt16(MainForm.player.MonsterCaptures, 0);
        }

        private void comboBoxMonsters_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDownKillCount.Value = BitConverter.ToInt16(MainForm.player.MonsterKills, (comboBoxMonsters.SelectedIndex * 2));
            numericUpDownCaptureCount.Value = BitConverter.ToInt16(MainForm.player.MonsterCaptures, (comboBoxMonsters.SelectedIndex * 2));
        }

        private void numericUpDownKillCount_ValueChanged(object sender, EventArgs e)
        {
            byte[] numberBytes = BitConverter.GetBytes((int)numericUpDownKillCount.Value);
            MainForm.player.MonsterKills[(comboBoxMonsters.SelectedIndex * 2)] = numberBytes[0];
            MainForm.player.MonsterKills[(comboBoxMonsters.SelectedIndex * 2) + 1] = numberBytes[1];
        }

        private void numericUpDownCaptureCount_ValueChanged(object sender, EventArgs e)
        {
            byte[] numberBytes = BitConverter.GetBytes((int)numericUpDownCaptureCount.Value);
            MainForm.player.MonsterCaptures[(comboBoxMonsters.SelectedIndex * 2)] = numberBytes[0];
            MainForm.player.MonsterCaptures[(comboBoxMonsters.SelectedIndex * 2) + 1] = numberBytes[1];
        }

        private void numericUpDownVillageLow_ValueChanged(object sender, EventArgs e)
        {
            byte[] numberBytes = BitConverter.GetBytes((int)numericUpDownVillageLow.Value);
            MainForm.player.GuildCardData[0x85e] = numberBytes[0];
            MainForm.player.GuildCardData[0x85f] = numberBytes[1];
        }

        private void numericUpDownVillageHigh_ValueChanged(object sender, EventArgs e)
        {
            byte[] numberBytes = BitConverter.GetBytes((int)numericUpDownVillageHigh.Value);
            MainForm.player.GuildCardData[0x860] = numberBytes[0];
            MainForm.player.GuildCardData[0x861] = numberBytes[1];
        }

        private void numericUpDownHHLow_ValueChanged(object sender, EventArgs e)
        {
            byte[] numberBytes = BitConverter.GetBytes((int)numericUpDownHHLow.Value);
            MainForm.player.GuildCardData[0x862] = numberBytes[0];
            MainForm.player.GuildCardData[0x863] = numberBytes[1];
        }

        private void numericUpDownHHHigh_ValueChanged(object sender, EventArgs e)
        {
            byte[] numberBytes = BitConverter.GetBytes((int)numericUpDownHHHigh.Value);
            MainForm.player.GuildCardData[0x864] = numberBytes[0];
            MainForm.player.GuildCardData[0x865] = numberBytes[1];
        }

        private void numericUpDownGRank_ValueChanged(object sender, EventArgs e)
        {
            byte[] numberBytes = BitConverter.GetBytes((int)numericUpDownGRank.Value);
            MainForm.player.GuildCardData[0x866] = numberBytes[0];
            MainForm.player.GuildCardData[0x867] = numberBytes[1];
        }

        private void numericUpDownSpecialPermit_ValueChanged(object sender, EventArgs e)
        {
            byte[] numberBytes = BitConverter.GetBytes((int)numericUpDownSpecialPermit.Value);
            MainForm.player.GuildCardData[0x868] = numberBytes[0];
            MainForm.player.GuildCardData[0x869] = numberBytes[1];
        }

        private void numericUpDownArena_ValueChanged(object sender, EventArgs e)
        {
            byte[] numberBytes = BitConverter.GetBytes((int)numericUpDownArena.Value);
            MainForm.player.GuildCardData[0x86A] = numberBytes[0];
            MainForm.player.GuildCardData[0x86B] = numberBytes[1];
        }
    }
}
