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
        private List<string[]> equipment = new List<string[]>();

        public string EmployerPosition { get => employerPosition; set => employerPosition = value; }
        public List<string[]> Equipment { get => equipment; set => equipment = value; }
    }
}
