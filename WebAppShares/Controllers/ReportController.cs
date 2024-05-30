using Microsoft.AspNetCore.Mvc;
//using WordReportNamespace;

using WebAppShares.Data;
using Microsoft.EntityFrameworkCore;
using WebAppShares.Data.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Identity;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

public class ReportController : Controller
{

   private readonly WordReportGenerator.WordReportGenerator _wordReportGenerator;
   private readonly ExcelReportGenerator.ExcelReportGenerator _excelReportGenerator;
   private readonly ApplicationDbContext _context;

    public ReportController(WordReportGenerator.WordReportGenerator wordReportGenerator,
                           ExcelReportGenerator.ExcelReportGenerator excelReportGenerator, 
                            ApplicationDbContext context)
   {
       _wordReportGenerator = wordReportGenerator;
       _excelReportGenerator = excelReportGenerator;
        _context = context;
   }

    [HttpGet("generate-word-report")]
    public IActionResult GenerateWordReport()
    {
        List<ProductsModel> prod = _context.Products.Include(x => x.Image).OrderByDescending(x => x.Id).ToList();
        List<IdentityUser> user = _context.Users.OrderByDescending(x => x.Id).ToList();

        string filePath = "wwwroot/reports/report.docx";
        var ff = Path.Combine(Environment.CurrentDirectory, filePath);

        var list = new List<Tuple<List<string[]>, int>>();

        var data = new List<string[]>
    {
        new string[] { "Id", "Name", "Description", "Quantity", "Price", "Discont" }
    };

        foreach (var p in prod)
        {
            data.Add(new string[] { p.Id.ToString(), p.Name, p.Description, p.Quantity.ToString(), p.Price.ToString(), p.Discount.ToString() });
        }
        list.Add(Tuple.Create(data, 6));

        data = new List<string[]>
    {
        new string[] { "Id", "Email", "UserName", "PhoneNumber", "EmailConfirmed" }
    };

        foreach (var u in user)
        {
            data.Add(new string[] { u.Id, u.Email, u.UserName, u.PhoneNumber, u.EmailConfirmed.ToString() });
        }
        list.Add(Tuple.Create(data, 5));

        _wordReportGenerator.GenerateReport(ff, list);

        return PhysicalFile(ff, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
    }


    [HttpGet("generate-excel-report")]
    public  IActionResult GenerateExcelReport()
    {
        List<ProductsModel> prod = _context.Products.Include(x => x.Image).OrderByDescending(x => x.Id).ToList();
        List<IdentityUser> user = _context.Users.OrderByDescending(x => x.Id).ToList();

        string filePath = "wwwroot/reports/report.docx";
        var ff = Path.Combine(Environment.CurrentDirectory, filePath);

        var list = new List<Tuple<List<string[]>, int>>();

        var data = new List<string[]>
    {
        new string[] { "Id", "Name", "Description", "Quantity", "Price", "Discont" }
    };

        foreach (var p in prod)
        {
            data.Add(new string[] { p.Id.ToString(), p.Name, p.Description, p.Quantity.ToString(), p.Price.ToString(), p.Discount.ToString() });
        }
        list.Add(Tuple.Create(data, 6));

        data = new List<string[]>
    {
        new string[] { "Id", "Email", "UserName", "PhoneNumber", "EmailConfirmed" }
    };

        foreach (var u in user)
        {
            data.Add(new string[] { u.Id, u.Email, u.UserName, u.PhoneNumber, u.EmailConfirmed.ToString() });
        }
        list.Add(Tuple.Create(data, 5));

        _excelReportGenerator.GenerateRepor(ff, list);

        return PhysicalFile(ff, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }
}
