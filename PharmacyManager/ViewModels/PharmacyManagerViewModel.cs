using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using PharmacyManager.Commands;
using PharmacyManager.Models;
using System.Windows.Input;

namespace PharmacyManager.ViewModels
{
    class PharmacyManagerViewModel : ViewModelBase
    {

        private string str;
        private string[] med = new string[13];
        private int rCnt;
        private int cCnt;
        private int rw = 0;
        private int cl = 0;
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
            var Patient = GetPatientObject(icInput);
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



        }

        public PatientObject GetPatientObject(string ic)
        {
            string currentDir = Environment.CurrentDirectory;
            string filePath = "Database.xlsx";
            string fullPath = Path.Combine(currentDir, filePath);
            var patientObject = new PatientObject();
            var xlApp = new Excel.Application();
            var xlWorkBook = xlApp.Workbooks.Open(fullPath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            var xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            var range = xlWorkSheet.UsedRange;
            rw = range.Rows.Count;
            cl = range.Columns.Count;
            bool isExist = false;


            for (rCnt = 1; rCnt <= rw; rCnt++)
            {
               double catchIC = (double)(range.Cells[rCnt, 1] as Excel.Range).Value2;
                str = catchIC.ToString();
                if (str == ic)
                {
                    //MessageBox.Show(str);
                    for (cCnt = 2; cCnt <= cl; cCnt++)
                    {
                        med[cCnt] = (string)(range.Cells[rCnt, cCnt] as Excel.Range).Value2;
                        //MessageBox.Show(med[cCnt]);
                    }
                    isExist = true;
                }
            }

            patientObject.Name = med[2];
            patientObject.Medicine1 = med[3];
            patientObject.Medicine2 = med[4];
            patientObject.Medicine3 = med[5];
            patientObject.Medicine4 = med[6];
            patientObject.Medicine5 = med[7];
            patientObject.Medicine6 = med[8];
            patientObject.Medicine7 = med[9];
            patientObject.Medicine8 = med[10];
            patientObject.Medicine9 = med[11];
            patientObject.Medicine10 = med[12];

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
            if (isExist)
            {
                return patientObject;
            }
            else
            {
                return null;
            }
        }

    }
}
