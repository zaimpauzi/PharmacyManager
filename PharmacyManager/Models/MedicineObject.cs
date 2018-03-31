using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManager.Models
{
    public class MedicineObject
    {

        public Count Count  { get; set; }
    
    }

    public class Count
    {
        public string Name { get; set; }
        public string Unit { get; set; }
        public string MinQuantity { get; set; }
        public string MaxQuantity { get; set; }
    }
}
