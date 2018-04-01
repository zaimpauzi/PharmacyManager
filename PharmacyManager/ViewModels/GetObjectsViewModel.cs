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
            return medicineList;

        }



        public PatientObject GetPatientObject(string ic, List<MedicineObject> medicineList)
        {
            
            string[] med = new string[13];
            string[] medName = new string[10];
            string[] medMax = new string[10];
            string[] medMin = new string[10];
            string[] medUnit = new string[10];
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
                        
                        if (cCnt > 2)
                        { 
                            
                            foreach (var medicine in medicineList)
                            {
                                if (med[cCnt] == medicine.Name)
                                {
                                    medMin[cCnt - 3] = medicine.MinQuantity;
                                    medMax[cCnt - 3] = medicine.MaxQuantity;
                                    medUnit[cCnt - 3] = medicine.Unit;
                                }
                            }
                        }
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

            patientObject.MedMin1 = medMin[0];
            patientObject.MedMin2 = medMin[1];
            patientObject.MedMin3 = medMin[2];
            patientObject.MedMin4 = medMin[3];
            patientObject.MedMin5 = medMin[4];
            patientObject.MedMin6 = medMin[5];
            patientObject.MedMin7 = medMin[6];
            patientObject.MedMin8 = medMin[7];
            patientObject.MedMin9 = medMin[8];
            patientObject.MedMin10 = medMin[9];

            patientObject.MedMax1 = medMax[0];
            patientObject.MedMax2 = medMax[1];
            patientObject.MedMax3 = medMax[2];
            patientObject.MedMax4 = medMax[3];
            patientObject.MedMax5 = medMax[4];
            patientObject.MedMax6 = medMax[5];
            patientObject.MedMax7 = medMax[6];
            patientObject.MedMax8 = medMax[7];
            patientObject.MedMax9 = medMax[8];
            patientObject.MedMax10 = medMax[9];

            patientObject.MedUnit1 = medUnit[0];
            patientObject.MedUnit2 = medUnit[1];
            patientObject.MedUnit3 = medUnit[2];
            patientObject.MedUnit4 = medUnit[3];
            patientObject.MedUnit5 = medUnit[4];
            patientObject.MedUnit6 = medUnit[5];
            patientObject.MedUnit7 = medUnit[6];
            patientObject.MedUnit8 = medUnit[7];
            patientObject.MedUnit9 = medUnit[8];
            patientObject.MedUnit10 = medUnit[9];

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
