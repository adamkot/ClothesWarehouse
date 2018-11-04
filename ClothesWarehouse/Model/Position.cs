using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesWarehouse
{
    public class Position
    {
        private string employerPosition;
        private List<String> equipment = new List<string>();
        private List<int> expiry = new List<int>();

        public string EmployerPosition { get => employerPosition; set => employerPosition = value; }
        public List<String> Equipment { get => equipment; set => equipment = value; }
        public List<int> Expiry { get => expiry; set => expiry = value; }
    }
}
