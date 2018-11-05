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
        private BindingSource positionSource;
        private BindingSource equipmentSource;
        PositionXML pXml = new PositionXML();
        List<Position> pList = new List<Position>();
        List<String> pListPosition = new List<string>();
        List<String> pListEquipment = new List<string>();

        public AddForm()
        {
            InitializeComponent();
            cancelButton.CausesValidation = false; //bez validacji jeśli anulujemy
            loadData();
        }

        private void loadData()
        {
            try
            {
                pList = pXml.ReadDataFromXml();
                if (pList != null)
                {
                    for (int i = 0; i < pList.Count; i++)
                    {
                        pListPosition.Add(pList[i].EmployerPosition);
                    }
                }
                else
                {
                    MessageBox.Show("Plik nie istnieje");
                    pList = new List<Position>();
                }
            }
            catch
            {
                MessageBox.Show("Nieudane wczytywanie z pamięci");
            }

            var bindingListPosition = new BindingList<String>(pListPosition) { };
            positionSource = new BindingSource(bindingListPosition, null);
            positionListBox.DataSource = positionSource;
            setDataTime();
        }

        private void setNewEquipmentList(int selectedIndex)
        {
            Position p = pList[selectedIndex];
            List<string> tmpList = new List<string>();
            try
            {
                for (int i = 0; i < p.Equipment.Count; i++)
                {
                    tmpList.Add(p.Equipment[i][0]);
                }
            }
            catch
            {
                MessageBox.Show("Brak obiektów na liście");
            }
            pListEquipment = tmpList;
            setDataSourceEquipmentListBox(pListEquipment);
        }

        private void setDataSourceEquipmentListBox(List<String> eqList)
        {
            var bindingListEquipment = new BindingList<String>(eqList) { };
            equipmentSource = new BindingSource(bindingListEquipment, null);
            equipmentListBox.DataSource = equipmentSource;
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

        private void setDataTime()
        {
            dateTimePicker.Value = DateTime.Now.AddMonths(Convert.ToInt32(getExpData()));
        }

        private string getExpData()
        {
            if (positionListBox.SelectedIndex != -1 && equipmentListBox.SelectedIndex != -1)
            {
                Position p = pList[positionListBox.SelectedIndex];
                return p.Equipment[equipmentListBox.SelectedIndex][1];
            }
            else
            {
                return "0";
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

        private void addPositionButton_Click(object sender, EventArgs e)
        {
            EditPositionAndEquipmentForm editPaeForm = new EditPositionAndEquipmentForm();
            editPaeForm.Show();
        }

        private void addEquipmentButton_Click(object sender, EventArgs e)
        {
            EditPositionAndEquipmentForm editPaeForm = new EditPositionAndEquipmentForm();
            editPaeForm.Show();
        }

        private void positionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setNewEquipmentList(positionListBox.SelectedIndex);
        }

        private void equipmentListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setDataTime();
        }
    }
}
