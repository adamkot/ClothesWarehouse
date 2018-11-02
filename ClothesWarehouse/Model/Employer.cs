using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesWarehouse
{
    public class Employer
    {
        private string name;
        private string surname;
        private string position;
        private string equipment;
        private DateTime expDate;

        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public string Position { get => position; set => position = value; }
        public string Equipment { get => equipment; set => equipment = value; }
        public DateTime ExpDate { get => expDate; set => expDate = value; }
    }
}
