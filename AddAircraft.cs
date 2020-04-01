using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace windows_a29_acmi
{
    public partial class AddAircraft : DevExpress.XtraEditors.XtraForm
    {

        public Aircraft aircraft;

        public AddAircraft()
        {
            InitializeComponent();

            aircraft = new Aircraft();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (!(callsignEdit.EditValue is null))
                aircraft.callSign = callsignEdit.EditValue.ToString();

            if (!(coalitionEdit.EditValue is null))
                aircraft.coalition = coalitionEdit.EditValue.ToString();

            if (!(groupEdit.EditValue is null))
                aircraft.group = groupEdit.EditValue.ToString();

            if (!(pilotEdit.EditValue is null))
                aircraft.pilot = pilotEdit.EditValue.ToString();

            if (!(tailnumberEdit.EditValue is null))
                aircraft.tailnumber = tailnumberEdit.EditValue.ToString();

            if (!(colorEdit.EditValue is null))
                aircraft.color = colorEdit.EditValue.ToString();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
