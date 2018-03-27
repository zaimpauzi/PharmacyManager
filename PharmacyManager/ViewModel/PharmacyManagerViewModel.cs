using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace PharmacyManager
{
    class PharmacyManagerViewModel
    {
       
        string str;
        string[] med = new string[13];
        int rCnt;
        int cCnt;
        int rw = 0;
        int cl = 0;

        public PatientObject GetPatientObject(string ic)
        {
            var patientObject = new PatientObject();
            string currentDir = Environment.CurrentDirectory;
            string filePath = "Database.xlsx";
            string fullPath = Path.Combine(currentDir, filePath);
            var xlApp = new Excel.Application();
            var xlWorkBook = xlApp.Workbooks.Open(fullPath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            var xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            var range = xlWorkSheet.UsedRange;
            rw = range.Rows.Count;
            cl = range.Columns.Count;


            for (rCnt = 1; rCnt <= rw; rCnt++)
            {
                str = (string)(range.Cells[rCnt, 1] as Excel.Range).Value2;

                if (str == ic)
                {
                    //MessageBox.Show(str);
                    for (cCnt = 2; cCnt <= cl; cCnt++)
                    {
                        med[cCnt] = (string)(range.Cells[rCnt, cCnt] as Excel.Range).Value2;
                        //MessageBox.Show(med[cCnt]);

                    }
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
            return patientObject;
        }

    }
}
