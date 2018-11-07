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
        private PositionXML pXml = new PositionXML();
        private List<Position> pList = new List<Position>();
        private List<String> pListPosition = new List<string>();
        private List<String> pListEquipment = new List<string>();
        private EditPositionAndEquipmentForm editForm = new EditPositionAndEquipmentForm();

        public AddForm()
        {
            InitializeComponent();
            //przyciski bez validacji
            cancelButton.CausesValidation = false;
            addEquipmentButton.CausesValidation = false;
            addPositionButton.CausesValidation = false;
            loadData();
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

        private void addPositionButton_Click(object sender, EventArgs e)
        {
            //otwórz okno stanowisk
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                refresh();
            }
        }

        private void addEquipmentButton_Click(object sender, EventArgs e)
        {
            //otwórz okno stanowisk i wskaż na wybrane stanowisko
            editForm.SetPosition = positionListBox.SelectedValue.ToString();
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                refresh();
            }
        }

        private void refresh()
        {
            pListPosition.Clear();
            loadData();
        }

        private void positionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //odśwież liste equipment dla wybranego stanowiska
            setNewEquipmentList(positionListBox.SelectedIndex);
        }

        private void equipmentListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ustaw termin wymiany dla wybranego wyposażenia
            setDataTime();
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

        /*
         * ładuje dane z pliku przez obiekt PositionXML
         * w przypadku jeśli wczytanie pliku się nie powiedzie
         * zostanie utworzona pusta lista
         */
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
                    MessageBox.Show("Plik nie istnieje", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    pList = new List<Position>();
                }
            }
            catch
            {
                MessageBox.Show("Nieudane wczytywanie z pamięci", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var bindingListPosition = new BindingList<String>(pListPosition) { };
            positionSource = new BindingSource(bindingListPosition, null);
            positionListBox.DataSource = positionSource;
            setDataTime();
        }

        /*
         * odświeża liste Equipment
         * korzysta z parametru do wybrania obiektu Position
         * jeśli nie uda się wczytać danych to wyświetlony będzie komunikat
         * i załadowana pusta lista
         */
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
                MessageBox.Show("Brak obiektów na liście", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            pListEquipment = tmpList;
            setDataSourceEquipmentListBox(pListEquipment);
        }

        /*
         * ustawia DataSource dla EquipmentListBox
         */
        private void setDataSourceEquipmentListBox(List<String> eqList)
        {
            var bindingListEquipment = new BindingList<String>(eqList) { };
            equipmentSource = new BindingSource(bindingListEquipment, null);
            equipmentListBox.DataSource = equipmentSource;
        }

        /*
         * konweryuje i dodaje miesiące do daty
         */
        private void setDataTime()
        {
            dateTimePicker.Value = DateTime.Now.AddMonths(Convert.ToInt32(getExpData()));
        }

        /*
         * Ustawia datę na podstawie danych z pamięci
         * pobiera liczbe miesięcy z obiektu position
         */
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
    }
}
