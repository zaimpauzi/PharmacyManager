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
        private ObservableCollection<DataGridObject> dataGridList = new ObservableCollection<DataGridObject>();
        private int _sQuantity;
        //private ObservableCollection<int> quantityy = new ObservableCollection<int>();


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
            //ObservableCollection<Medicine> _listOfMedicineName = new ObservableCollection<Medicine>();
            var Patient = GetObject.GetPatientObject(icInput, MedicineList);
            if (Patient != null)
            {
               
                ObservableCollection<int> Quantity = new ObservableCollection<int>();
                PrintName = Patient.Name;
                //_listOfMedicineName = Patient.medicine;
                //foreach (Medicine medicineName in Patient.medicine)
                //{
                //    _listOfMedicineName.Add(medicineName.medicine);
                //}


                foreach (Medicine _medicine in Patient.medicine)
                {
                    var DataGrid = new DataGridObject();
                    DataGrid.DGMedName = _medicine.Name;
                    DataGrid.DGUnit = _medicine.Unit;
                    Quantity = QuantityLister(Int32.Parse(_medicine.Min), Int32.Parse(_medicine.Max));
                    DataGrid.DGQuantity = Quantity;
                    DataGridList.Add(DataGrid);

                }

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
                OnPropertyChanged("PrintName");
            }
        }

        public ObservableCollection<DataGridObject> DataGridList
        {
            get { return dataGridList; }
            set
            {
                dataGridList = value;
                OnPropertyChanged("DataGridList");
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
                //Quantity.Clear();
        }

     
        public int SQuantity
        {
            get { return _sQuantity;}
            set { _sQuantity = value; }
        }

        //public ObservableCollection<int> Quantityy
        //{
        //    get { return quantityy; }
        //    set
        //    {
        //        if (quantityy == value)
        //        {
        //            return;
        //        }
        //        quantityy = value;
        //        OnPropertyChanged("Quantity");
        //    }
        //}




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
