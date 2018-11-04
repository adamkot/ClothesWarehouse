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
            //wczytuje dane do position list
            try
            {
                pList = pXml.ReadDataFromXml();
                for (int i = 0; i < pList.Count; i++)
                {
                    pListPosition.Add(pList[i].EmployerPosition);
                }
            }
            catch
            {
                MessageBox.Show("Nieudane wczytywanie z pamięci");
            }
            //ustwiam dataSource dla listBox
            var bindingListPosition = new BindingList<String>(pListPosition) { };
            positionSource = new BindingSource(bindingListPosition, null);
            ListPositionListBox.DataSource = positionSource;
            setEquipmentListBox(pListEquipment);
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
            equipmentSource.Add(EquipmentAddTextBox.Text);
            EquipmentAddTextBox.Text = "";
            DataExpComboBox.SelectedIndex = 0;
        }

        private void DataExpComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListPositionListBox.SelectedIndex > 0)
            {
                Position p = pList[ListPositionListBox.SelectedIndex];
                //zapisuje wynik na pozycji = pozycji equipment
                p.Expiry[ListEquipmentListBox.SelectedIndex] = Convert.ToInt32(DataExpComboBox.SelectedValue);
            }
        }

        private void ListPositionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Position p = pList[ListPositionListBox.SelectedIndex];
            pListEquipment = p.Equipment;
            setEquipmentListBox(pListEquipment);
        }

        private void setEquipmentListBox(List<String> eqList)
        {
            var bindingListEquipment = new BindingList<String>(eqList) { };
            equipmentSource = new BindingSource(bindingListEquipment, null);
            ListEquipmentListBox.DataSource = equipmentSource;
        }

        
    }
}
