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
using ZXing;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using System.Threading;

namespace PharmacyManager.ViewModels
{
    public class GetObjectsViewModel
    {
        

        public List<MedicineObject> getMedicineList()
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



        public PatientObject getPatientObject(string ic, List<MedicineObject> medicineList)
        {

            string[] c = new string[2];
            object patientDetail;
            string PatientDetail;
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
                        patientDetail = (range.Cells[rCnt, cCnt] as Excel.Range).Value2;
                        PatientDetail = patientDetail.ToString();

                        if (cCnt ==2)
                        {
                            patientObject.Name = PatientDetail;
                        }

                        if (cCnt==3)
                        {
                            patientObject.IC = PatientDetail;
                        }

                        if (cCnt > 2)
                        {
                            medName = PatientDetail;
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

        public string getBarCode()
        {
            VideoCapture capture = new VideoCapture();
            string Result=null;
            
            while (Result == null)
            {

                var image = capture.QueryFrame();
                try
                {
                    Bitmap barcodeBitmap = image.ToImage<Bgr, Byte>().Bitmap; //Convert the emgu Image to BitmapImage 
                    //barcodeBitmap.Save("test.bmp");
                    //Bitmap barcodeBitmap = new Bitmap("C:\\test.bmp");

                    // create a barcode reader instance
                    IBarcodeReader reader = new BarcodeReader();
                    var result = reader.Decode(barcodeBitmap);

                    if (result != null)
                    {
                        Result = result.Text.ToString();
                    }

                }
                catch (Exception)
                {

                    MessageBox.Show("Camera Not Working!");
                    Environment.Exit(0);
                }

            }
            capture.Dispose();
            return Result;
        }

    }
}
