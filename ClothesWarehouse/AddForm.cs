using System;
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
    public partial class AddForm : Form
    {
        private Employer emp;
        internal Employer Emp { get => emp; set => emp = value; }

        public AddForm()
        {
            InitializeComponent();
            cancelButton.CausesValidation = false; //bez validacji jeśli anulujemy
            List<String> list = new List<string> {"test1", "test2", "test3" };
            equipmentListBox.DataSource = list;
            positionListBox.DataSource = list;
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                emp = new Employer();
                emp.Name = nameTextBox.Text;
                emp.Surname = sNameTextBox.Text;
                emp.Position = positionListBox.Text;
                emp.Equipment = equipmentListBox.Text;
                emp.ExpDate = dateTimePicker.Value;
                DialogResult = DialogResult.OK;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void nameTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (nameTextBox.Text.Length < 3)
            {
                e.Cancel = true;
                errorProvider.SetError(nameTextBox, "wprowadź imię");
            }
        }

        private void sNameTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (sNameTextBox.Text.Length < 3)
            {
                e.Cancel = true;
                errorProvider.SetError(sNameTextBox, "wprowadź nazwisko");
            }
        }
    }
}
