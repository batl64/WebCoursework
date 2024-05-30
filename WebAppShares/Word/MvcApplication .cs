using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using WebAppShares.Data.Identity;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using WebAppShares.Data;
using Microsoft.AspNetCore.Mvc;


namespace WebAppShares.Word
{
    public class YourClass : Controller
    {
        private readonly ApplicationDbContext _context;

        public YourClass(ApplicationDbContext context)
        {
            _context = context;

        }

        public void YourMethod()
        {
            Excel.Application xlApp = null;
            Excel.Workbook xlBook = null;
            Excel.Worksheet xlSheet = null;

            try
            {
                xlApp = new Excel.Application();
                xlBook = xlApp.Workbooks.Add();
                xlSheet = (Excel.Worksheet)xlBook.Worksheets[1];

            
                List<ProductsModel> prod =  _context.Products.Include(x => x.Image).OrderByDescending(x => x.Id).ToList();
                xlSheet.Cells[1, 1].Value2 = "Id";
                xlSheet.Cells[1, 2].Value2 = "Name";
                xlSheet.Cells[1, 1].Value2 = "Quantity";
                xlSheet.Cells[1, 2].Value2 = "Description";
                xlSheet.Cells[1, 2].Value2 = "ImageId";
                xlSheet.Cells[1, 1].Value2 = "Price";
                xlSheet.Cells[1, 2].Value2 = "Discount";


                for (int i = 1; i <= prod.Count; i++)
                {
                    xlSheet.Cells[i, 1].Value2 = "Name";
                    xlSheet.Cells[i, 2].Value2 = "Age";
                }
             
                xlBook.Save();


            }
            finally
            {
                
                if (xlSheet != null)
                {
                    Marshal.ReleaseComObject(xlSheet);
                    xlSheet = null;
                }
                if (xlBook != null)
                {
                    xlBook.Close(false);
                    Marshal.ReleaseComObject(xlBook);
                    xlBook = null;
                }
                if (xlApp != null)
                {
                    xlApp.Quit();
                    Marshal.ReleaseComObject(xlApp);
                    xlApp = null;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }


        }
    }
}