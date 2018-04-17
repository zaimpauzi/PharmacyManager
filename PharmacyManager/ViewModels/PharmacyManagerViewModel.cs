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
using System.Threading;
using System.Windows.Threading;
using System.Windows.Documents;
using System.Windows.Controls;

namespace PharmacyManager.ViewModels
{
    class PharmacyManagerViewModel : ViewModelBase
    {
        private List<MedicineObject> medicineList;
        private DelegateCommand clearCommand;
        private bool canClear;
        private string printName;
        private string printIC;
        private ObservableCollection<DataGridObject> dataGridList = new ObservableCollection<DataGridObject>();
        private int _sQuantity;
        private Thread thread;
      

        //Contructor
        public PharmacyManagerViewModel()
        {
            var GetObject = new GetObjectsViewModel();  //Initialize GetObject class
            medicineList = GetObject.getMedicineList(); //Get list of medicine available in excel. It will store in List variable for entire application running.
            thread = new Thread(GetAllObjects);
            thread.Start();
            //MessageBox.Show("test");
            //GetAllObjects();
        }

        private void isSearch()
        {
            bool stillSearching = true;
            while (stillSearching == true)
            {
            
                  var GetObject = new GetObjectsViewModel();
                  string barCode = GetObject.getBarCode();
                  var Patient = GetObject.getPatientObject(barCode, medicineList);
                  if (Patient != null)
                     {
               
                         ObservableCollection<int> Quantity = new ObservableCollection<int>();
                         PrintName = Patient.Name;
                         PrintIC = Patient.IC;
                         foreach (Medicine _medicine in Patient.medicine)
                             {
                                  var DataGrid = new DataGridObject();
                                  DataGrid.DGMedName = _medicine.Name;
                                  DataGrid.DGUnit = _medicine.Unit;
                                  DataGrid.SelectedQuantity = 0;
                                  Quantity = QuantityLister(Int32.Parse(_medicine.Min), Int32.Parse(_medicine.Max));
                                  DataGrid.DGQuantity = Quantity;
                                  DispatchService.Invoke(() =>
                                       {
                                           this.DataGridList.Add(DataGrid);
                                       });

                              }
                         stillSearching = false;
                         canClear = true;
                      }

                  else
                     {
                        MessageBox.Show("TIADA DALAM REKOD", "Amaran", MessageBoxButton.OK, MessageBoxImage.Information);
                
                     }
            }

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

        public string PrintIC
        {
            get { return printIC; }
            set
            {
                printIC = value;
                OnPropertyChanged("PrintIC");
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
                    clearCommand = new DelegateCommand(isClear, CanClear);
                }
                return clearCommand;
            }
        }

        private void isClear()
        {
            //ObservableCollection<DataGridObject> test = new ObservableCollection<DataGridObject>();
            //test = DataGridList;
            //PrintData();
            PrintName = string.Empty;
            PrintIC = string.Empty;
            DataGridList.Clear();
            canClear = false;

            //Restart thread            
            thread.Abort();
            //Thread.Sleep(2000);
            thread = new Thread(GetAllObjects);
            thread.Start();

            //restart app
            //System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            //Application.Current.Shutdown();
        }

        private bool CanClear()
        {
            return canClear;
        }

        private void GetAllObjects()
        {
            
            isSearch();

        }

        public int SQuantity
        {
            get { return _sQuantity;}
            set { _sQuantity = value; }
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

        public void PrintData()
        {
            // Create a PrintDialog
            PrintDialog printDlg = new PrintDialog();

            // Create a FlowDocument dynamically.
            FlowDocument doc = CreateFlowDocument();
            doc.Name = "FlowDoc";

            // Create IDocumentPaginatorSource from FlowDocument
            IDocumentPaginatorSource idpSource = doc;
            printDlg.ShowDialog();
            
            // Call PrintDocument method to send document to printer
            printDlg.PrintDocument(idpSource.DocumentPaginator, "Hello WPF Printing.");
        }

        public FlowDocument CreateFlowDocument()
        {
            // Create a FlowDocument
            FlowDocument doc = new FlowDocument();

            // Create a Section
            Section sec = new Section();

            // Create first Paragraph
            Paragraph p1 = new Paragraph();

            // Create and add a new Bold, Italic and Underline
            Bold bld = new Bold();
            bld.Inlines.Add(new Run("First Paragraph"));
            Italic italicBld = new Italic();
            italicBld.Inlines.Add(bld);
            Underline underlineItalicBld = new Underline();
            underlineItalicBld.Inlines.Add(italicBld);

            // Add Bold, Italic, Underline to Paragraph
            p1.Inlines.Add(underlineItalicBld);

            // Add Paragraph to Section
            sec.Blocks.Add(p1);

            // Add Section to FlowDocument
            doc.Blocks.Add(sec);

            return doc;
        }


    }
    public static class DispatchService
    {
        public static void Invoke(Action action)
        {
            Dispatcher dispatchObject = Application.Current.Dispatcher;
            if (dispatchObject == null || dispatchObject.CheckAccess())
            {
                action();
            }
            else
            {
                dispatchObject.Invoke(action);
            }
        }
    }
}
