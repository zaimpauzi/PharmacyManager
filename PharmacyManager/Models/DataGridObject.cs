using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManager.Models
{
    class DataGridObject
    {
        public string DGMedName { get; set; }

        public string DGUnit { get; set; }
        
        public ObservableCollection<int> DGQuantity { get; set; }
    }
}
