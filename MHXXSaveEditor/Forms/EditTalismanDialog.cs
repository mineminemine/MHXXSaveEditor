using System;
using System.Windows.Forms;
using MHXXSaveEditor.Data;

namespace MHXXSaveEditor.Forms
{
    public partial class EditTalismanDialog : Form
    {
        private MainForm mainForm;

        public EditTalismanDialog(MainForm mainForm, string eqpName)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.Text = "Editing Kinsect - " + eqpName;
            comboBoxSkillName1.Items.AddRange(GameConstants.SkillNames);
            comboBoxSkillName2.Items.AddRange(GameConstants.SkillNames);

            comboBoxSkillName1.SelectedIndex = Convert.ToInt32(mainForm.player.EquipmentInfo[(mainForm.equipSelectedSlot * 36) + 12]);
            comboBoxSkillName2.SelectedIndex = Convert.ToInt32(mainForm.player.EquipmentInfo[(mainForm.equipSelectedSlot * 36) + 13]);
            numericUpDownSkillLevel1.Value = Convert.ToInt32(mainForm.player.EquipmentInfo[(mainForm.equipSelectedSlot * 36) + 14]);
            numericUpDownSkillLevel2.Value = Convert.ToInt32(mainForm.player.EquipmentInfo[(mainForm.equipSelectedSlot * 36) + 15]);
            numericUpDownSlots.Value = Convert.ToInt32(mainForm.player.EquipmentInfo[(mainForm.equipSelectedSlot * 36) + 16]);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            mainForm.player.EquipmentInfo[(mainForm.equipSelectedSlot * 36) + 12] = Convert.ToByte(comboBoxSkillName1.SelectedIndex);
            mainForm.player.EquipmentInfo[(mainForm.equipSelectedSlot * 36) + 13] = Convert.ToByte(comboBoxSkillName2.SelectedIndex);
            mainForm.player.EquipmentInfo[(mainForm.equipSelectedSlot * 36) + 14] = Convert.ToByte(numericUpDownSkillLevel1.Value); ;
            mainForm.player.EquipmentInfo[(mainForm.equipSelectedSlot * 36) + 15] = Convert.ToByte(numericUpDownSkillLevel2.Value); ;
            mainForm.player.EquipmentInfo[(mainForm.equipSelectedSlot * 36) + 16] = Convert.ToByte(numericUpDownSlots.Value);
            mainForm.player.EquipmentInfo[(mainForm.equipSelectedSlot * 36) + 17] = 0;
            mainForm.player.EquipmentInfo[(mainForm.equipSelectedSlot * 36) + 18] = Convert.ToByte(GameConstants.TalismanRarity[mainForm.player.EquipmentInfo[(mainForm.equipSelectedSlot * 36) + 2]]);
            mainForm.player.EquipmentInfo[(mainForm.equipSelectedSlot * 36) + 19] = 1;
            for ( int a = 20; a < 36; a++)
            {
                mainForm.player.EquipmentInfo[(mainForm.equipSelectedSlot * 36) + a] = 0;
            }

            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
