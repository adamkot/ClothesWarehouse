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
            empList = eXml.ReadDataFromXml(); // uwaga na błędy!!!
            var bindingList = new BindingList<Employer>(empList) { };
            source = new BindingSource(bindingList, null);
            mainTable.DataSource = source;

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

        private void searchButton_Click(object sender, EventArgs e)
        {
            search();
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

        private void equipmentListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //equipment list form
        }

        private void viewOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //view1
        }

        private void viewInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //view2
        }

        private void viewLastWymianyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //view3
        }

        private void homePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show home page
        }

        #endregion

        private void addRecord()
        {
            AddForm addForm = new AddForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                source.Add(addForm.Emp);
            }
        }

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
                MessageBox.Show("Błąd: nie zaznaczono pola)");
            }
            
        }

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
                MessageBox.Show("Błąd: nie zaznaczono pola)");
            }
        }

        private void loadListFromDB()
        {
            empList = eXml.ReadDataFromXml();
            //load data from database
        }

        private void saveListInDB()
        {

            
            //send data to database

            eXml.SaveDataToXml(empList);
        }

        private void search()
        {
            //search
        }

        private void exitProcedure()
        {
            DialogResult dialogResult = MessageBox.Show("Czy chcesz zakończyć pracę?", "Zamknij", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Dispose();
            }
            else
            {
                //else
            }
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
