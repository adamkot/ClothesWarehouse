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
    class PositionXML
    {
        /*
         * Pobiera listę obiektów Position
         * i zapisuje je do pliku "Position.xml" w głównym katalogu programu
         */
        public void SaveDataToXml(List<Position> position)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Position>));
            TextWriter writer = new StreamWriter("Position.xml");
            for (int k = 0; k < position.Count; k++)
            {
                DataTable tableName = new DataTable(position[k].EmployerPosition);
                DataColumn equColumn = new DataColumn("Equipment");
                tableName.Columns.Add(equColumn);
                DataRow row;
                for (int i = 0; i < position[k].Equipment.Count; i++)
                {
                    row = tableName.NewRow();
                    row[equColumn] = position[k].Equipment[i];
                    tableName.Rows.Add(row);
                }
            }
            ser.Serialize(writer, position);
            writer.Close();
        }

        /*
         * Wczytuje z pliku "Position.xml" z głównego katalogu programu
         * Zwraca listę obiektów Position
         */
        public List<Position> ReadDataFromXml() {
            List<Position> pList = null;
            if (File.Exists("Position.xml"))
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<Position>));
                ser.UnknownNode += new
                XmlNodeEventHandler(serializer_UnknownNode);
                ser.UnknownAttribute += new
                XmlAttributeEventHandler(serializer_UnknownAttribute);
                FileStream fs = new FileStream("Position.xml", FileMode.Open);
                pList = (List<Position>)ser.Deserialize(fs);
                fs.Close();
            }
            return pList; 
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
