using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PharmacyManager.Commands;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace PharmacyManager.Models
{
    public class PatientObject

    {
        public string Name { get; set; }

        public string IC { get; set; }

        public ObservableCollection<Medicine> medicine { get; set; }

        
    }

    public class Medicine
    {

        public string Name { get; set; }

        public string Min { get; set; }

        public string Max { get; set; }

        public string Unit { get; set; }



    }
}
