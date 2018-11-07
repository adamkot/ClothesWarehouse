using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace ClothesWarehouse
{
    class EmployerXML
    {

        /*
         * Pobiera listę obiektów Employer
         * i zapisuje je do pliku "Employer.xml" w głównym katalogu programu
         */
        public void SaveDataToXml(List<Employer> employer)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Employer>));
            TextWriter writer = new StreamWriter("Employer.xml");
            DataTable tableName = new DataTable("Employer");
            DataColumn nameColumn = new DataColumn("Name");
            DataColumn snameColumn = new DataColumn("Surname");
            DataColumn positionColumn = new DataColumn("Position");
            DataColumn equColumn = new DataColumn("Equipment");
            DataColumn expColumn = new DataColumn("ExpDate");
            tableName.Columns.Add(nameColumn);
            tableName.Columns.Add(snameColumn);
            tableName.Columns.Add(positionColumn);
            tableName.Columns.Add(equColumn);
            tableName.Columns.Add(expColumn);
            for (int i = 0; i < employer.Count; i++)
            {
                DataRow row;
                row = tableName.NewRow();
                row[nameColumn] = employer[i].Name;
                row[snameColumn] = employer[i].Surname;
                row[positionColumn] = employer[i].Position;
                row[equColumn] = employer[i].Equipment;
                row[expColumn] = employer[i].ExpDate;
                tableName.Rows.Add(row);
            }
            ser.Serialize(writer, employer);
            writer.Close();
        }

        /*
         * Wczytuje z pliku "Employer.xml" z głównego katalogu programu
         * Zwraca listę obiektów Employer
         */
        public List<Employer> ReadDataFromXml()
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Employer>));
            ser.UnknownNode += new
            XmlNodeEventHandler(serializer_UnknownNode);
            ser.UnknownAttribute += new
            XmlAttributeEventHandler(serializer_UnknownAttribute);
            FileStream fs = new FileStream("Employer.xml", FileMode.Open);
            List<Employer> eList = (List<Employer>)ser.Deserialize(fs);
            fs.Close();
            return eList;
        }

        protected void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        protected void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }
    }
}
