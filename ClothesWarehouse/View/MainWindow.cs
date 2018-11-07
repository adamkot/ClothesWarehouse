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
    public partial class MainWindow : Form
    {
        private List<Employer> empList;
        private BindingSource source;
        EmployerXML eXml;

        public MainWindow()
        {
            InitializeComponent();

            empList = new List<Employer>();
            eXml = new EmployerXML();
            try
            {
                empList = eXml.ReadDataFromXml();
            }
            catch
            {
                MessageBox.Show("Błąd: nie wczytano danych z pamięci", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            var bindingList = new BindingList<Employer>(empList) { };
            source = new BindingSource(bindingList, null);
            mainTable.DataSource = source;
            //opis tabeli
            mainTable.Columns[0].HeaderText = "Imię";
            mainTable.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            mainTable.Columns[1].HeaderText = "Nazwisko";
            mainTable.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            mainTable.Columns[2].HeaderText = "Stanowisko";
            mainTable.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            mainTable.Columns[3].HeaderText = "Wyposażenie";
            mainTable.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            mainTable.Columns[4].HeaderText = "Data ważności";
            mainTable.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            expireDateListener();
        }

        #region menu

        private void addButton_Click(object sender, EventArgs e)
        {
            addRecord();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            editRecord();
        }

        private void delButton_Click(object sender, EventArgs e)
        {
            deleteRecord();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            loadListFromDB();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            saveListInDB();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadListFromDB();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveListInDB();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exitProcedure();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addRecord();
        }

        private void editDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editRecord();
        }

        private void delToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteRecord();
        }

        private void positionListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditPositionAndEquipmentForm editForm = new EditPositionAndEquipmentForm();
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                
            }
        }

        private void viewOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewExp();
        }

        private void viewInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewInData();
        }

        private void viewLastWymianyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewAll();
        }

        private void homePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/adamkot/ClothesWarehouse");
        }

        private void searchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (searchTextBox.Text.Length > 2)
            {
                search();
            }
            else
            {
                viewAll();
            }

        }

        #endregion

        /*
         * Otwiera okno AddForm.
         * Jesli okno wypełniono poprawnie dodaje rekord do listy
         */
        private void addRecord()
        {
            AddForm addForm = new AddForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                source.Add(addForm.Emp);
            }
        }

        /*
         * Otwiera okno EditForm
         * Jeśli zostało wypełnione poprawnie to zamienia rekord
         */
        private void editRecord()
        {

            if (mainTable.CurrentCell != null)
            {
                EditForm editForm = new EditForm(empList[mainTable.CurrentCell.RowIndex]);
                editForm.Emp = empList[mainTable.CurrentCell.RowIndex];

                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    source.RemoveAt(mainTable.CurrentCell.RowIndex);
                    source.Add(editForm.Emp);
                }
            }
            else
            {
                MessageBox.Show("Błąd: nie zaznaczono pola");
            }
            
        }

        /*
         * Usuwa zaznaczony rekord
         */
        private void deleteRecord()
        {
            if (mainTable.CurrentCell != null)
            {
                DialogResult dialogResult = MessageBox.Show("Czy chcesz usunąć pole \"" 
                    + empList[mainTable.CurrentCell.RowIndex].Name + " " 
                    + empList[mainTable.CurrentCell.RowIndex].Surname + "\"", "Usuń", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    source.RemoveAt(mainTable.CurrentCell.RowIndex);
                }
            }
            else
            {
                MessageBox.Show("Błąd: nie zaznaczono pola", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*
         * Wczytuje dane z pliku przez klase EmployerXml
         */
        private void loadListFromDB()
        {
            empList = eXml.ReadDataFromXml();
        }

        /*
         * Zapisuje dane do pliku przez klase EmployerXml
         */
        private void saveListInDB()
        {
            eXml.SaveDataToXml(empList);
        }

        /*
         * Generuje listę rekordów z datami po terminie
         */
        private void viewExp()
        {
            List<Employer> viewList = new List<Employer>();
            for (int i = 0; i < empList.Count; i++)
            {
                if (DateTime.Compare(empList[i].ExpDate, DateTime.Now) < 0)
                {
                    viewList.Add(empList[i]);
                }
            }
            var viewBindingList = new BindingList<Employer>(viewList) { };
            BindingSource viewSource = new BindingSource(viewBindingList, null);
            mainTable.DataSource = viewSource;
        }

        /*
         * Generuje listę rekordów z datami przed terminem
         */
        private void viewInData()
        {
            List<Employer> viewList = new List<Employer>();
            for (int i = 0; i < empList.Count; i++)
            {
                if (DateTime.Compare(empList[i].ExpDate, DateTime.Now) >= 0)
                {
                    viewList.Add(empList[i]);
                }
            }
            var viewBindingList = new BindingList<Employer>(viewList) { };
            BindingSource viewSource = new BindingSource(viewBindingList, null);
            mainTable.DataSource = viewSource;
        }

        /*
         * Generuje listę wszystkich rekordów
         */
        private void viewAll()
        {
            mainTable.DataSource = source;
            expireDateListener();
        }

        /*
         * Generuje listę rekordów zgodną z treścią wyszukiwania
         */
        private void search()
        {
            int max = empList.Count;
            List<Employer> searchList = new List<Employer>();
            for (int i = 0; i < max; i++) {
                if ((empList[i].Equipment.ToLower().Contains(searchTextBox.Text.ToLower()))
                    || (empList[i].Name.ToLower().Contains(searchTextBox.Text.ToLower()))
                    || (empList[i].Surname.ToLower().Contains(searchTextBox.Text.ToLower()))
                    || (empList[i].Position.ToLower().Contains(searchTextBox.Text.ToLower()))) {
                    searchList.Add(empList[i]);
                }
            }
            var searchBindingList = new BindingList<Employer>(searchList) { };
            BindingSource searchSource = new BindingSource(searchBindingList, null);
            mainTable.DataSource = searchSource;
        }

        /*
         * Sprawdza czy termin jest przekroczony
         * jeśli tak zaznacza wiersz na czerwono
         */
        private void expireDateListener()
        {
            for (int i = 0; i < empList.Count; i++) {
                if (DateTime.Compare(empList[i].ExpDate,DateTime.Now) < 0) {
                    mainTable.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        /*
         * Procedura wyjścia z programu
         * (pytanie czy chcesz wyjść)
         */
        private void exitProcedure()
        {
            DialogResult dialogResult = MessageBox.Show("Czy chcesz zakończyć pracę?", "Zamknij", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Dispose();
            }
            else
            {

            }
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
