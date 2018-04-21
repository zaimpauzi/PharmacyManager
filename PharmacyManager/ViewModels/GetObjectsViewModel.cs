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
        

        //public List<MedicineObject> getMedicineList()
        //{
        //    object _input;
        //    string[] medparam = new string[5];
        //    int rCnt;
        //    int cCnt;
        //    int rw = 0;
        //    int cl = 0;
        //    string currentDir = Environment.CurrentDirectory;
        //    string filePath = "MedicineList.xlsx";
        //    string fullPath = Path.Combine(currentDir, filePath);
        //    var xlApp = new Excel.Application();
        //    var xlWorkBook = xlApp.Workbooks.Open(fullPath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
        //    var xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
        //    var range = xlWorkSheet.UsedRange;
        //    rw = range.Rows.Count;
        //    cl = range.Columns.Count;
        //    List<MedicineObject> medicineList = new List<MedicineObject>();


        //    for (rCnt = 1; rCnt <= rw; rCnt++)
        //    {
               
        //            for (cCnt = 1; cCnt <= cl; cCnt++)
        //        {
                    
        //            _input = (range.Cells[rCnt, cCnt] as Excel.Range).Value2;
        //            //_input = (range.Cells[rCnt, cCnt] as Excel.Range).Value2;
                   
        //            string input = _input.ToString();
        //            medparam[cCnt] = input;
        //        }

        //        medicineList.Add(new MedicineObject(medparam[1], medparam[2], medparam[3], medparam[4]));

        //    }

        //    xlWorkBook.Close(true, null, null);
        //    xlApp.Quit();

        //    Marshal.ReleaseComObject(xlWorkSheet);
        //    Marshal.ReleaseComObject(xlWorkBook);
        //    Marshal.ReleaseComObject(xlApp);
        //    return medicineList;

        //}



        public PatientObject getPatientObject(string barCode)
        {

            string[] c = new string[2];
            object medicineDetail;
            string PatientDetail;
            string medName;
            //string medMax;
            //string medMin;
            //string medUnit;
            //string patientName;
            int rCnt;
            int cCnt;
            int rw = 0;
            int cl = 0;
            string patientDatabaseDir = Environment.CurrentDirectory + @"\PatientDatabase";
            string fileName = barCode + ".xlsx";
            string fullPath = Path.Combine(patientDatabaseDir, fileName);
            var patientObject = new PatientObject();
            bool isExist = false;
            bool medicineExist = false;
            ObservableCollection<Medicine> _medicineList = new ObservableCollection<Medicine>();
            string[] barCodeList = Directory.GetFiles(patientDatabaseDir, "*.xlsx");


            foreach (var codeList in barCodeList)
            {
                if (fullPath == codeList)
                {
                    var xlApp = new Excel.Application();
                    var xlWorkBook = xlApp.Workbooks.Open(fullPath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                    var xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                    var range = xlWorkSheet.UsedRange;
                    rw = range.Rows.Count;
                    cl = range.Columns.Count;
                    isExist = true;

                    for (rCnt = 1; rCnt <= rw; rCnt++)
                    {

                        object catchDetail = (range.Cells[rCnt, 2] as Excel.Range).Value2;
                        if (rCnt == 1)
                        {
                            patientObject.Name = catchDetail.ToString();
                        }

                        if (rCnt == 2)
                        {
                            patientObject.IC = catchDetail.ToString();
                        }

                        if (rCnt >= 4)
                        {
                            //MessageBox.Show(str);
                            var medicine = new Medicine();
                            for (cCnt = 1; cCnt <= cl; cCnt++)
                            {
                                medicineDetail = (range.Cells[rCnt, cCnt] as Excel.Range).Value2;

                                if (medicineDetail != null)
                                {
                                    if (cCnt == 1)
                                    {
                                        medicine.Name = medicineDetail.ToString();
                                    }

                                    if (cCnt == 2)
                                    {
                                        medicine.Min = medicineDetail.ToString();
                                    }

                                    if (cCnt == 3)
                                    {
                                        medicine.Add = medicineDetail.ToString();
                                    }

                                    if (cCnt == 4)
                                    {
                                        medicine.Max = medicineDetail.ToString();
                                    }

                                    if (cCnt == 5)
                                    {
                                        medicine.Unit = medicineDetail.ToString();
                                    }

                                    medicineExist = true;
                                }
                            }
                            if (medicineExist)
                            {
                                _medicineList.Add(medicine);
                                medicineExist = false;
                            }
                        }
                        
                    }

                    patientObject.medicine = _medicineList;
                    xlWorkBook.Close(true, null, null);
                    xlApp.Quit();

                    Marshal.ReleaseComObject(xlWorkSheet);
                    Marshal.ReleaseComObject(xlWorkBook);
                    Marshal.ReleaseComObject(xlApp);
                }

                else
                {
                    isExist = false;

                }
            }
        

           
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
