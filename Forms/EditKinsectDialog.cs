using System;
using System.Windows.Forms;

using MHXXSaveEditor.Data;

namespace MHXXSaveEditor.Forms
{
    public partial class EditKinsectDialog : Form
    {
        private MainForm mainForm;
        public EditKinsectDialog(MainForm mainForm, string eqpName)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.Text = "Editing Kinsect - " + eqpName;

            comboBoxKinsectType.SelectedIndex = Convert.ToInt32(mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 12]);
            numericUpDownLevel.Value = Convert.ToInt32(mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 13]);
            numericUpDownPowerLv.Value = Convert.ToInt32(mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 14]);
            numericUpDownWeightLv.Value = Convert.ToInt32(mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 15]);
            numericUpDownSpeedLv.Value = Convert.ToInt32(mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 16]);
            numericUpDownFireLv.Value = Convert.ToInt32(mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 17]);
            numericUpDownWaterLv.Value = Convert.ToInt32(mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 18]);
            numericUpDownThunderLv.Value = Convert.ToInt32(mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 19]);
            numericUpDownIceLv.Value = Convert.ToInt32(mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 20]);
            numericUpDownDragonLv.Value = Convert.ToInt32(mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 21]);
            numericUpDownPowerExp.Value = Convert.ToInt32(mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 24]);
            numericUpDownWeightExp.Value = Convert.ToInt32(mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 25]);
            numericUpDownSpeedExp.Value = Convert.ToInt32(mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 26]);
            numericUpDownFireExp.Value = Convert.ToInt32(mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 28]);
            numericUpDownWaterExp.Value = Convert.ToInt32(mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 29]);
            numericUpDownThunderExp.Value = Convert.ToInt32(mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 30]);
            numericUpDownIceExp.Value = Convert.ToInt32(mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 31]);
            numericUpDownDragonExp.Value = Convert.ToInt32(mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 32]);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 12] = Convert.ToByte(comboBoxKinsectType.SelectedIndex);
            mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 13] = Convert.ToByte(numericUpDownLevel.Value);
            mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 14] = Convert.ToByte(numericUpDownPowerLv.Value);
            mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 15] = Convert.ToByte(numericUpDownWeightLv.Value);
            mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 16] = Convert.ToByte(numericUpDownSpeedLv.Value);
            mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 17] = Convert.ToByte(numericUpDownFireLv.Value);
            mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 18] = Convert.ToByte(numericUpDownWaterLv.Value);
            mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 19] = Convert.ToByte(numericUpDownThunderLv.Value);
            mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 20] = Convert.ToByte(numericUpDownIceLv.Value);
            mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 21] = Convert.ToByte(numericUpDownDragonLv.Value);
            mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 24] = Convert.ToByte(numericUpDownPowerExp.Value);
            mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 25] = Convert.ToByte(numericUpDownWeightExp.Value);
            mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 26] = Convert.ToByte(numericUpDownSpeedExp.Value);
            mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 28] = Convert.ToByte(numericUpDownFireExp.Value);
            mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 29] = Convert.ToByte(numericUpDownWaterExp.Value);
            mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 30] = Convert.ToByte(numericUpDownThunderExp.Value);
            mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 31] = Convert.ToByte(numericUpDownIceExp.Value);
            mainForm.player.equipmentInfo[(mainForm.equipSelectedSlot * 36) + 32] = Convert.ToByte(numericUpDownDragonExp.Value);

            this.Close();
        }
    }
}
