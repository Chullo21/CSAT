using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PIMES_DMS.Data;
using PIMES_DMS.Models;

namespace PIMES_DMS.Controllers
{
    public class RMAController : Controller
    {
        private readonly AppDbContext _Db;

        public RMAController(AppDbContext db)
        {
            _Db = db;
        }

        public IActionResult ShowFA(int id)
        {
            RMAModel rma = _Db.RMADb.FirstOrDefault(j => j.RMAID == id);

            byte[]? faByte = rma.FA;

            if (faByte == null)
            {
                
                return NoContent();
            }
            else
            {
                return File(faByte, "application/pdf");
            }

        }

        public void UpdateNotif(string message, string t)
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            NotifModel nm = new NotifModel();
            {
                nm.Message = EN + message;
                nm.DateCreated = DateTime.Now;
                nm.Type = t;
            }

            if (ModelState.IsValid)
            {
                _Db.NotifDb.Add(nm);
            }
        }

        public IActionResult UploadFA(DateTime date, IFormFile attachment, int id)
        {
            var rma = _Db.RMADb.FirstOrDefault(j => j.RMAID == id);

            RMAModel model = new RMAModel();
            {
                model = rma;
                model.DateReceived = date;
                
                using(MemoryStream ms = new MemoryStream())
                {
                    attachment.CopyTo(ms);
                    model.FA = ms.ToArray();
                }
            }

            if (ModelState.IsValid)
            {
                UpdateNotif(", uploaded an F.A attachment with RMA# of " + model.RMANo, "All");

                _Db.RMADb.Update(model);
                _Db.SaveChanges();
            }

            return RedirectToAction("GetRMA", "Issue");
        }

        public IActionResult GenerateExcelFile(int ID)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                var item = _Db.RMADb.FirstOrDefault(j => j.RMAID == ID);

                worksheet.Cells[1, 1].Value = "Date Created";
                worksheet.Cells[1, 2].Value = "RMA #";
                worksheet.Cells[1, 3].Value = "Issue #";
                worksheet.Cells[1, 4].Value = "Product";
                worksheet.Cells[1, 5].Value = "Affected PN";
                worksheet.Cells[1, 6].Value = "Description";
                worksheet.Cells[1, 7].Value = "Problem Statement";
                worksheet.Cells[1, 8].Value = "Quantity";

                worksheet.Cells[2, 1].Value = item!.DateCreated.ToShortDateString();
                worksheet.Cells[2, 2].Value = item.RMANo;
                worksheet.Cells[2, 3].Value = item.IssueNo;
                worksheet.Cells[2, 4].Value = item.Product;
                worksheet.Cells[2, 5].Value = item.AffectedPN;
                worksheet.Cells[2, 6].Value = item.Description;
                worksheet.Cells[2, 7].Value = item.ProblemDesc;
                worksheet.Cells[2, 8].Value = item.QTY;

                worksheet.Column(1).Width = 12;
                worksheet.Column(2).Width = 20;
                worksheet.Column(3).Width = 12;
                worksheet.Column(4).Width = 12;
                worksheet.Column(5).Width = 12;
                worksheet.Column(6).Width = 40;
                worksheet.Column(7).Width = 80;
                worksheet.Column(8).Width = 12;

                // Set row height
                worksheet.Row(1).Height = 25;

                byte[] excelBytes = package.GetAsByteArray();

                string fileName = "data.xlsx";
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Headers["Content-Disposition"] = "inline; filename=" + fileName;
                return File(excelBytes, contentType);
            }

        }

        private List<RMAModel> rmas(int year, int month)
        {
            if (year == 0 && month == 0)
            {
                return _Db.RMADb.ToList();
            }
            else if (year > 0 && month == 0)
            {
                return _Db.RMADb.Where(j => j.DateCreated.Year == year).ToList();
            }
            else 
            {
                return _Db.RMADb.Where(j => j.DateCreated.Year == year && j.DateCreated.Month == month).ToList();
            }
        }

        public IActionResult SelectedDate(int year, int month)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("RMA Monitoring");
                int row = 2;
                foreach (var item in rmas(year, month))
                {
                    worksheet.Cells[1, 1].Value = "Date Created";
                    worksheet.Cells[1, 2].Value = "RMA #";
                    worksheet.Cells[1, 3].Value = "Issue #";
                    worksheet.Cells[1, 4].Value = "Product";
                    worksheet.Cells[1, 5].Value = "Affected PN";
                    worksheet.Cells[1, 6].Value = "Description";
                    worksheet.Cells[1, 7].Value = "Problem Statement";
                    worksheet.Cells[1, 8].Value = "Quantity";

                    worksheet.Cells[row, 1].Value = item.DateCreated.ToShortDateString();
                    worksheet.Cells[row, 2].Value = item.RMANo;
                    worksheet.Cells[row, 3].Value = item.IssueNo;
                    worksheet.Cells[row, 4].Value = item.Product;
                    worksheet.Cells[row, 5].Value = item.AffectedPN;
                    worksheet.Cells[row, 6].Value = item.Description;
                    worksheet.Cells[row, 7].Value = item.ProblemDesc;
                    worksheet.Cells[row, 8].Value = item.QTY;

                    row++;
                }

                // Set column widths
                worksheet.Column(1).Width = 12;
                worksheet.Column(2).Width = 20;
                worksheet.Column(3).Width = 12;
                worksheet.Column(4).Width = 12;
                worksheet.Column(5).Width = 12;
                worksheet.Column(6).Width = 40;
                worksheet.Column(7).Width = 80;
                worksheet.Column(8).Width = 12;

                // Set row height
                worksheet.Row(1).Height = 25;

                byte[] excelBytes = package.GetAsByteArray();

                string fileName = "RMA Monitoring" + year + ".xlsx";
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Headers["Content-Disposition"] = "inline; filename=" + fileName;
                return File(excelBytes, contentType);
            }

        }

    }
}
