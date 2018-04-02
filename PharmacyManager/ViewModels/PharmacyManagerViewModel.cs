using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PharmacyManager.Commands;
using System.Windows.Input;
using PharmacyManager.Models;
using System.Collections.ObjectModel;

namespace PharmacyManager.ViewModels
{
    class PharmacyManagerViewModel : ViewModelBase
    {
        private List<MedicineObject> MedicineList;
        private string icInput;
        private DelegateCommand searchCommand;
        private DelegateCommand clearCommand;
        private string printName;
        private string printMed1;
        private string printMed2;
        private string printMed3;
        private string printMed4;
        private string printMed5;
        private string printMed6;
        private string printMed7;
        private string printMed8;
        private string printMed9;
        private string printMed10;

        private int _sQuantityList1;
        private ObservableCollection<int> quantityList1;


        //Contructor
        public PharmacyManagerViewModel()
        {
            var GetMedicineList = new GetObjectsViewModel();  //Initialize GetObject class
            MedicineList = GetMedicineList.GetMedicineList(); //Get list of medicine available in excel. It will store in List variable for entire application running.
        }

        public string ICInput
        {
            get { return icInput; }
            set { icInput = value; }
        }

        public ICommand SearchCommand
        {
            get
            {
                if (searchCommand == null)
                {
                    searchCommand = new DelegateCommand(isSearch, CanSearch);
                }
                return searchCommand;
            }
        }

        private void isSearch()
        {
          
            var GetObject = new GetObjectsViewModel();
            
            var Patient = GetObject.GetPatientObject(icInput, MedicineList);
            if (Patient != null)
            {
                PrintName = Patient.Name;
                PrintMed1 = Patient.Medicine1;
                PrintMed2 = Patient.Medicine2;
                PrintMed3 = Patient.Medicine3;
                PrintMed4 = Patient.Medicine4;
                PrintMed5 = Patient.Medicine5;
                PrintMed6 = Patient.Medicine6;
                PrintMed7 = Patient.Medicine7;
                PrintMed8 = Patient.Medicine8;
                PrintMed9 = Patient.Medicine9;
                PrintMed10 = Patient.Medicine10;

                QuantityList1 = QuantityLister(Convert.ToInt32(Patient.MedMin1), Convert.ToInt32(Patient.MedMax1));
            }

            else
            {
                MessageBox.Show("TIADA DALAM REKOD", "Amaran", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool CanSearch()
        {
            if (!string.IsNullOrEmpty(this.icInput) )
                return true;
            else
                return false;
        }

        public string PrintName
        {
            get { return printName; }
            set
            {
                printName = value;
                OnPropertyChanged("printName");
            }
        }

        public string PrintMed1
        {
            get { return printMed1; }
            set
            {
                printMed1 = value;
                OnPropertyChanged("printMed1");
            }
        }

        public string PrintMed2
        {
            get { return printMed2; }
            set
            {
                printMed2 = value;
                OnPropertyChanged("printMed2");
            }
        }

        public string PrintMed3
        {
            get { return printMed3; }
            set
            {
                printMed3 = value;
                OnPropertyChanged("printMed3");
            }
        }

        public string PrintMed4
        {
            get { return printMed4; }
            set
            {
                printMed4 = value;
                OnPropertyChanged("printMed4");
            }
        }

        public string PrintMed5
        {
            get { return printMed5; }
            set
            {
                printMed5 = value;
                OnPropertyChanged("printMed5");
            }
        }

        public string PrintMed6
        {
            get { return printMed6; }
            set
            {
                printMed6 = value;
                OnPropertyChanged("printMed6");
            }
        }

        public string PrintMed7
        {
            get { return printMed7; }
            set
            {
                printMed7 = value;
                OnPropertyChanged("printMed7");
            }
        }

        public string PrintMed8
        {
            get { return printMed8; }
            set
            {
                printMed8 = value;
                OnPropertyChanged("printMed8");
            }
        }

        public string PrintMed9
        {
            get { return printMed9; }
            set
            {
                printMed9 = value;
                OnPropertyChanged("printMed9");
            }
        }

        public string PrintMed10
        {
            get { return printMed10; }
            set
            {
                printMed10 = value;
                OnPropertyChanged("printMed10");
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                if (clearCommand == null)
                {
                    clearCommand = new DelegateCommand(isClear);
                }
                return clearCommand;
            }
        }

        private void isClear()
        {
                ICInput = string.Empty;
                PrintName = string.Empty;
                PrintMed1 = string.Empty;
                PrintMed2 = string.Empty;
                PrintMed3 = string.Empty;
                PrintMed4 = string.Empty;
                PrintMed5 = string.Empty;
                PrintMed6 = string.Empty;
                PrintMed7 = string.Empty;
                PrintMed8 = string.Empty;
                PrintMed9 = string.Empty;
                PrintMed10 = string.Empty;
                QuantityList1.Clear();
        }

     
        public int SQuantityList1
        {
            get { return _sQuantityList1;}
            set { _sQuantityList1 = value; }
        }

        public ObservableCollection<int> QuantityList1
        {
            get { return quantityList1; }
            set
            {
                if (quantityList1 == value)
                {
                    return;
                }
                quantityList1 = value;
                OnPropertyChanged("quantityList1");
            }
        }




        public ObservableCollection<int> QuantityLister(int _min, int _max)
        {
            _max = (_max + 1);
            ObservableCollection<int> quantityList = new ObservableCollection<int>();
            for (int i = _min; i < _max; i++)
            {
                quantityList.Add(i);
            }
            return quantityList;
        }

    }
}
