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
using System.Drawing;

namespace PharmacyManager.ViewModels
{
    class PharmacyManagerViewModel : ViewModelBase
    {
        private List<MedicineObject> medicineList;
        private DelegateCommand clearCommand;
        private DelegateCommand printCommand;
        private bool canClear;
        private string printName;
        private string printIC;
        private ObservableCollection<DataGridObject> dataGridList = new ObservableCollection<DataGridObject>();
        private int _sQuantity;
        private Thread thread;
        private string barCode;


        //Contructor
        public PharmacyManagerViewModel()
        {
            //var GetObject = new GetObjectsViewModel();  //Initialize GetObject class
            //medicineList = GetObject.getMedicineList(); //Get list of medicine available in excel. It will store in List variable for entire application running.
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
                  barCode = GetObject.getBarCode();
                  var Patient = GetObject.getPatientObject(barCode);
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
                                  Quantity = QuantityLister(Int32.Parse(_medicine.Min), Int32.Parse(_medicine.Max), Int32.Parse(_medicine.Add));
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
            PrintName = string.Empty;
            PrintIC = string.Empty;
            DataGridList.Clear();
            canClear = false;

            //Restart thread            
            thread.Abort();
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

        public ICommand PrintCommand
        {
            get
            {
                if (printCommand == null)
                {
                    printCommand = new DelegateCommand(isPrint, CanClear);
                }
                return printCommand;
            }
        }

        private void isPrint()
        {
            PrintData();
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

        public ObservableCollection<int> QuantityLister(int _min, int _max, int _add)
        {
            //_max = (_max + 1);
            ObservableCollection<int> quantityList = new ObservableCollection<int>();
            for (int i = _min; i <= _max; i += _add)
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
            printDlg.PrintDocument(idpSource.DocumentPaginator, "List");
        }

        public FlowDocument CreateFlowDocument()
        {

            // Create a FlowDocument
            FlowDocument doc = new FlowDocument();
            doc.ColumnWidth = 180;
            // Create first Paragraph
            Paragraph p1 = new Paragraph();
            p1.FontSize = 12;
            p1.Inlines.Add(new Run("Farmasi Hospital Sik, Kedah"));
          
            // Create the Table 1
            var table1 = new Table();

            //Create table with 2 column
            for (int x = 0; x < 3; x++)
            {
                table1.Columns.Add(new TableColumn());
            }

            // Create and add an empty TableRowGroup to hold the table's Rows.
            table1.RowGroups.Add(new TableRowGroup());

            // Add the first row.
            table1.RowGroups[0].Rows.Add(new TableRow());

            // Alias the current working row for easy reference.
            TableRow currentRowT1 = table1.RowGroups[0].Rows[0];
            currentRowT1.FontSize = 12;
            
            // Add cells with content to the second row.
            currentRowT1.Cells.Add(new TableCell(new Paragraph(new Run("Nama"))));
            currentRowT1.Cells.Add(new TableCell(new Paragraph(new Run(PrintName))));
            currentRowT1.Cells[1].ColumnSpan = 2;
            currentRowT1.Cells[1].TextAlignment = TextAlignment.Left;

            table1.RowGroups[0].Rows.Add(new TableRow());
            currentRowT1 = table1.RowGroups[0].Rows[1];
            currentRowT1.FontSize = 12;
            currentRowT1.Cells.Add(new TableCell(new Paragraph(new Run("No. IC"))));
            currentRowT1.Cells.Add(new TableCell(new Paragraph(new Run(PrintIC))));
            currentRowT1.Cells[1].ColumnSpan = 2;
            currentRowT1.Cells[1].TextAlignment = TextAlignment.Left;

            table1.RowGroups[0].Rows.Add(new TableRow());
            currentRowT1 = table1.RowGroups[0].Rows[2];
            currentRowT1.FontSize = 12;
            currentRowT1.Cells.Add(new TableCell(new Paragraph(new Run("ID"))));
            currentRowT1.Cells.Add(new TableCell(new Paragraph(new Run(barCode))));
            currentRowT1.Cells[1].ColumnSpan = 2;
            currentRowT1.Cells[1].TextAlignment = TextAlignment.Left;
            
            // Create the Table 2
            var table2 = new Table();
            //Create table with 3 column
            for (int x = 0; x < 4; x++)
            {
                table2.Columns.Add(new TableColumn());
            }

            // Create and add an empty TableRowGroup to hold the table's Rows.
            table2.RowGroups.Add(new TableRowGroup());

            // Add the first row.
            table2.RowGroups[0].Rows.Add(new TableRow());

            // Alias the current working row for easy reference.
            TableRow currentRowT2 = table2.RowGroups[0].Rows[0];
            currentRowT2.FontSize = 12;
            
            // Add cells with content to the second row.
            currentRowT2.Cells.Add(new TableCell(new Paragraph(new Run("Jenis Ubat"))));
            currentRowT2.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            currentRowT2.Cells.Add(new TableCell(new Paragraph(new Run("Kuantiti"))));
            currentRowT2.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            currentRowT2.Cells[0].ColumnSpan = 2;
            currentRowT2.Cells[0].TextAlignment = TextAlignment.Center;
            currentRowT2.Cells[2].ColumnSpan = 2;
            currentRowT2.Cells[2].TextAlignment = TextAlignment.Center;
            
            int _row = 0;

            foreach (var medListPrint in dataGridList)
            {
                _row = (_row + 1);
                
                table2.RowGroups[0].Rows.Add(new TableRow());
                currentRowT2 = table2.RowGroups[0].Rows[_row];
                currentRowT2.FontSize = 12;
                currentRowT2.Cells.Add(new TableCell(new Paragraph(new Run(medListPrint.DGMedName))));
                currentRowT2.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                currentRowT2.Cells.Add(new TableCell(new Paragraph(new Run(medListPrint.SelectedQuantity.ToString()))));
                currentRowT2.Cells.Add(new TableCell(new Paragraph(new Run(medListPrint.DGUnit))));
                currentRowT2.Cells[0].ColumnSpan = 2;
                currentRowT2.Cells[0].TextAlignment = TextAlignment.Left;
                currentRowT2.Cells[2].TextAlignment = TextAlignment.Right;
            }
            // Add it to the FlowDocument Blocks collection.
            doc.Blocks.Add(p1);
            doc.Blocks.Add(table1);
            doc.Blocks.Add(table2);

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
