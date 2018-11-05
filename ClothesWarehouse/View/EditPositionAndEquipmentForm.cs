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
        PositionXML pXml = new PositionXML();
        List<Position> pList = new List<Position>();
        List<String> pListPosition = new List<string>();
        List<String> pListEquipment = new List<string>();
        private BindingSource positionSource;
        private BindingSource equipmentSource;

        public EditPositionAndEquipmentForm()
        {
            InitializeComponent();
            CancelButton.CausesValidation = false; //bez validacji jeśli anulujemy
            //wczytuje dane do position list
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
                else {
                    MessageBox.Show("Plik nie istnieje");
                    pList = new List<Position>();
                }
            }
            catch
            {
                MessageBox.Show("Nieudane wczytywanie z pamięci");
            }
            //ustwiam dataSource dla listBox
            setDataSourcePositionListBox(pListPosition);
            setDataSourceEquipmentListBox(pListEquipment);
            //ustawiam datasource dla combobox
            ArrayList item = new ArrayList();
            for(int i = 0; i <= 60; i = i + 6)
            {
                item.Add(i);
            }
            DataExpComboBox.DataSource = item;
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                pXml.SaveDataToXml(pList);
                DialogResult = DialogResult.OK;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void PositionAddButton_Click(object sender, EventArgs e)
        {
            Position p = new Position();
            p.EmployerPosition = PositionAddTextBox.Text;
            pList.Add(p);
            positionSource.Add(p.EmployerPosition);
            PositionAddTextBox.Text = "";
        }

        private void EquipmentAddButton_Click(object sender, EventArgs e)
        {
            Position p = pList[ListPositionListBox.SelectedIndex];
            p.Equipment.Add(new string[] { EquipmentAddTextBox.Text, "0"});
            setNewEquipmentList(ListPositionListBox.SelectedIndex);
            EquipmentAddTextBox.Text = "";
        }

        private void DataExpComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //setDataExpComboBox();
        }

        private void ListPositionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setNewEquipmentList(ListPositionListBox.SelectedIndex);
        }

        private void ListEquipmentListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setDataExpComboBox();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            saveDataExpComboBox();
        }

        private void setNewEquipmentList(int selectedIndex) {
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
            setDataExpComboBox();
        }

        private void setDataSourceEquipmentListBox(List<String> eqList)
        {
            var bindingListEquipment = new BindingList<String>(eqList) { };
            equipmentSource = new BindingSource(bindingListEquipment, null);
            ListEquipmentListBox.DataSource = equipmentSource;
        }

        private void setDataSourcePositionListBox(List<String> poList)
        {
            var bindingListPosition = new BindingList<String>(poList) { };
            positionSource = new BindingSource(bindingListPosition, null);
            ListPositionListBox.DataSource = positionSource;
        }

        private void setDataExpComboBox()
        {
            if (ListPositionListBox.SelectedIndex != -1 && ListEquipmentListBox.SelectedIndex != -1)
            {
                Position p = pList[ListPositionListBox.SelectedIndex];
                try
                {
                    DataExpComboBox.SelectedIndex = DataExpComboBox.FindStringExact(p.Equipment[ListEquipmentListBox.SelectedIndex][1]);
                }
                catch
                {
                    DataExpComboBox.SelectedIndex = 0;
                }
            }
        }

        private void saveDataExpComboBox()
        {
            if (ListPositionListBox.SelectedIndex != -1 && ListEquipmentListBox.SelectedIndex != -1)
            {
                Position p = pList[ListPositionListBox.SelectedIndex];
                p.Equipment[ListEquipmentListBox.SelectedIndex][1] = DataExpComboBox.SelectedValue.ToString();
            }  
        }

        
    }
}
