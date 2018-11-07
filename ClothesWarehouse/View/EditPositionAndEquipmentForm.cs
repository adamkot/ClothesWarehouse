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
        private string setPosition;
        public string SetPosition { get => setPosition; set => setPosition = value; }
        PositionXML pXml = new PositionXML();
        List<Position> pList = new List<Position>();
        List<String> pListPosition = new List<string>();
        List<String> pListEquipment = new List<string>();
        private BindingSource positionSource;
        private BindingSource equipmentSource;

        public EditPositionAndEquipmentForm()
        {
            InitializeComponent();
            //na potrzeby sprawdzania warunku czy wybieramy z listy podczas wczytywania
            setPosition = "";
            //przyciski bez validacji
            CancelButton.CausesValidation = false;
            //wczytuje dane do positionList
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
                    MessageBox.Show("Plik nie istnieje", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    pList = new List<Position>();
                }
            }
            catch
            {
                MessageBox.Show("Nieudane wczytanie z pamięci", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void EditPositionAndEquipmentForm_Load(object sender, EventArgs e)
        {
            //jeśli setPosition istnieje to ustaw PositionList
            if (setPosition.Length > 0)
            {
                ListPositionListBox.SelectedIndex = pListPosition.IndexOf(setPosition);
            }
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
            if (EquipmentAddTextBox.Text.Length != 0 && PositionAddTextBox.Text.Length != 0)
            {
                DialogResult dialogResult = MessageBox.Show("Pola tekstowe nie są puste! "
                    + "\nCzy nie powinny zostać zapisane?",
                    "Pytanie", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {

                }
                if (dialogResult == DialogResult.No)
                {
                    saveDataExpComboBox();
                }
            }
            else
            {
                saveDataExpComboBox();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ListPositionListBox.SelectedIndex != -1)
            {
                Position p = pList[ListPositionListBox.SelectedIndex];
                pList.Remove(p);
                positionSource.Remove(p.EmployerPosition);
            }
            else
            {
                MessageBox.Show("Nie zaznaczono pozycji do usunięcia", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void deleteEqToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ListPositionListBox.SelectedIndex != -1 && ListEquipmentListBox.SelectedIndex != -1)
            {
                Position p = pList[ListPositionListBox.SelectedIndex];
                p.Equipment.Remove(p.Equipment[ListPositionListBox.SelectedIndex]);
                setNewEquipmentList(ListPositionListBox.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Nie zaznaczono pozycji do usunięcia", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /*
         * Ustawianie nowej listy wyposażenia na podstawie danych
         * pobranych z obiektu Position
         * W przypadku gdy w obiekcie nie ma wyposażenia
         * lista będzie pusta
         */
        private void setNewEquipmentList(int selectedIndex) {
            if (selectedIndex != -1)
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
                    MessageBox.Show("Brak obiektów na liście", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                pListEquipment = tmpList;
                setDataSourceEquipmentListBox(pListEquipment);
                setDataExpComboBox();
            }
        }

        /*
         * Ustawianie datasource dla equipmentList
         */
        private void setDataSourceEquipmentListBox(List<String> eqList)
        {
            var bindingListEquipment = new BindingList<String>(eqList) { };
            equipmentSource = new BindingSource(bindingListEquipment, null);
            ListEquipmentListBox.DataSource = equipmentSource;
        }

        /*
         * Ustawianie datasource dla positionList
         */
        private void setDataSourcePositionListBox(List<String> poList)
        {
            var bindingListPosition = new BindingList<String>(poList) { };
            positionSource = new BindingSource(bindingListPosition, null);
            ListPositionListBox.DataSource = positionSource;
        }

        /*
         * Ustawianie liczby miesięcy odczytanych z obiektu Position
         * Jeśli nie istnieje to ustaw 0
         */
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

        /*
         * Zapisuje liczbe miesięcy do obiektu Position
         */
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
