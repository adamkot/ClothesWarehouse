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
        private string[] equipment;
        private int[] expiry;

        public string EmployerPosition { get => employerPosition; set => employerPosition = value; }
        public string[] Equipment { get => equipment; set => equipment = value; }
        public int[] Expiry { get => expiry; set => expiry = value; }
    }
}
