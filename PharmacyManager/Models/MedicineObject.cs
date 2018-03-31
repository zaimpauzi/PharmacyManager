using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManager.Models
{
    public class MedicineObject
    {

        public string Name { get; set; }
        public string Unit { get; set; }
        public string MinQuantity { get; set; }
        public string MaxQuantity { get; set; }

        public MedicineObject (string _name, string _unit, string _min, string _max)
        {
            Name = _name;
            Unit = _unit;
            MinQuantity = _min;
            MaxQuantity = _max;
        }
    }

}
