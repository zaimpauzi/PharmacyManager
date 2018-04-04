using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyManager.Models;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Collections.ObjectModel;

namespace PharmacyManager.ViewModels
{
    public class GetObjectsViewModel
    {
        

        public List<MedicineObject> GetMedicineList()
        {
            object _input;
            string[] medparam = new string[5];
            int rCnt;
            int cCnt;
            int rw = 0;
            int cl = 0;
            string currentDir = Environment.CurrentDirectory;
            string filePath = "MedicineList.xlsx";
            string fullPath = Path.Combine(currentDir, filePath);
            var xlApp = new Excel.Application();
            var xlWorkBook = xlApp.Workbooks.Open(fullPath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            var xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            var range = xlWorkSheet.UsedRange;
            rw = range.Rows.Count;
            cl = range.Columns.Count;
            List<MedicineObject> medicineList = new List<MedicineObject>();


            for (rCnt = 1; rCnt <= rw; rCnt++)
            {
               
                    for (cCnt = 1; cCnt <= cl; cCnt++)
                {
                    
                    _input = (range.Cells[rCnt, cCnt] as Excel.Range).Value2;
                    //_input = (range.Cells[rCnt, cCnt] as Excel.Range).Value2;
                   
                    string input = _input.ToString();
                    medparam[cCnt] = input;
                }

                medicineList.Add(new MedicineObject(medparam[1], medparam[2], medparam[3], medparam[4]));

            }

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
            return medicineList;

        }



        public PatientObject GetPatientObject(string ic, List<MedicineObject> medicineList)
        {

            string[] c = new string[2];
            string patientName;
            string medName;
            string medMax;
            string medMin;
            string medUnit;
            string str;
            int rCnt;
            int cCnt;
            int rw = 0;
            int cl = 0;
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

            ObservableCollection<Medicine> _medicineList = new ObservableCollection<Medicine>();

            for (rCnt = 1; rCnt <= rw; rCnt++)
            {
                
                double catchIC = (double)(range.Cells[rCnt, 1] as Excel.Range).Value2;
                str = catchIC.ToString();
                
                if (str == ic)
                {
                    //MessageBox.Show(str);
                    for (cCnt = 2; cCnt <= cl; cCnt++)
                    {
                        patientName = (string)(range.Cells[rCnt, cCnt] as Excel.Range).Value2;

                        if (cCnt ==2)
                        {
                            patientObject.Name = patientName;
                        }

                        if (cCnt > 2)
                        {
                            medName = patientName;
                            foreach (var medicine in medicineList)
                            {
                                if (medName == medicine.Name)
                                {
                                    var _medicine = new Medicine();
                                    medMin = medicine.MinQuantity;
                                    medMax = medicine.MaxQuantity;
                                    medUnit = medicine.Unit;
                                    _medicine.Name = medName;
                                    _medicine.Min = medMin;
                                    _medicine.Max = medMax;
                                    _medicine.Unit = medUnit;
                                    _medicineList.Add(_medicine);
                                    patientObject.medicine = _medicineList;
                                    
                                }
                            }
                        }
                    }
                    isExist = true;
                }
            }
       

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
