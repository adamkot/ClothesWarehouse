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
    public partial class EditForm : Form
    {
        private Employer emp;
        internal Employer Emp { get => emp; set => emp = value; }

        public EditForm(object o)
        {
            emp = (Employer)o;
            InitializeComponent();
            cancelButton.CausesValidation = false; //bez validacji jeśli anulujemy
            nameTextBox.Text = emp.Name;
            sNameTextBox.Text = emp.Surname;
            positionTextBox.Text = emp.Position;
            equipmentTextBox.Text = emp.Equipment;
            dateTimePicker.Value = emp.ExpDate;
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                emp = new Employer();
                emp.Name = nameTextBox.Text;
                emp.Surname = sNameTextBox.Text;
                emp.Position = positionTextBox.Text;
                emp.Equipment = equipmentTextBox.Text;
                emp.ExpDate = dateTimePicker.Value;
                DialogResult = DialogResult.OK;
            }
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
