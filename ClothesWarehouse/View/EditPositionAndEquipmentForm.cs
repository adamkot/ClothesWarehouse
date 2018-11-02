using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClothesWarehouse
{
    public partial class EditPositionAndEquipmentForm : Form
    {
        public EditPositionAndEquipmentForm()
        {
            InitializeComponent();
            ArrayList item = new ArrayList();
            item.Add("6");
            item.Add("12");
            item.Add("18");
            item.Add("24");
            item.Add("30");
            item.Add("36");
            item.Add("42");
            item.Add("48");
            item.Add("54");
            item.Add("60");
            DataExpComboBox.DataSource = item;
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                
                DialogResult = DialogResult.OK;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void PositionAddButton_Click(object sender, EventArgs e)
        {

        }

        private void EquipmentAddButton_Click(object sender, EventArgs e)
        {

        }
    }
}
