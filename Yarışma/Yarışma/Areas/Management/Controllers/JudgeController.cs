using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Yarışma.Models;
using ClosedXML.Excel;
using Yarışma.Areas.Management.Models;
using System.IO;
using System.Linq;

namespace Yarışma.Areas.Management.Controllers
{
    [Area("Management")]
    public class JudgeController : Controller
    {
        CompetitionDbContext db = new CompetitionDbContext();

        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var judges = db.Judges
                           .Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .ToList();

            var totalJudge = db.Judges.Count();

            var vm = new JudgeViewModel
            {
                Judges = judges,
                TotalCount = totalJudge,
                PageSize = pageSize,
                CurrentPage = page,
                JudgeCategories = db.JudgeCategories.ToList(),
            };
            return View(vm);
        }

        public IActionResult GetContestants(int page = 1, int pageSize = 10)
        {
            var judges = db.Judges
                           .Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .ToList();

            var totalJudge = db.Judges.Count();

            var vm = new JudgeViewModel
            {
                Judges = judges,
                TotalCount = totalJudge,
                PageSize = pageSize,
                CurrentPage = page,
                JudgeCategories = db.JudgeCategories.ToList(),
            };

            return PartialView(vm);
        }

        public IActionResult ExportToExcel()
        {
            // Veritabanından hakemleri alıyoruz
            var judges = db.Judges.ToList();

            // Excel dosyasını oluşturma
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Hakemler");

                // Başlıklar
                worksheet.Cell(1, 1).Value = "Ad Soyad";
                worksheet.Cell(1, 2).Value = "Kategori";
         
                worksheet.Cell(1, 3).Value = "Hakem Türü";

                // Verileri doldurma
                int row = 2;
                foreach (var judge in judges)
                {
                    worksheet.Cell(row, 1).Value = judge.JudgeProfil?.FullName ?? "Ad Soyad Yok";
                    worksheet.Cell(row, 2).Value = judge.ProjectCategory?.Name ?? "Kategori Yok";
           
                    worksheet.Cell(row, 3).Value = judge.JudgeCategory.Name ?? "Hakem Türü Yok";

                    row++;
                }

                // Excel dosyasını hafızaya kaydetme
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    // Excel dosyasını indirme olarak gönderme
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Hakemler.xlsx");
                }
            }
        }
        public IActionResult PendingApproval(int page = 1, int pageSize = 10)
        {
            // Onaylanmamış hakemleri alıyoruz
            var pendingJudges = db.Judges
                                  .Where(j => !j.IsApproved)
                                  .Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();

            var totalPendingCount = db.Judges.Count(j => !j.IsApproved);

            var vm = new JudgeViewModel
            {
                Judges = pendingJudges,
                TotalCount = totalPendingCount,
                PageSize = pageSize,
                CurrentPage = page,
                JudgeCategories = db.JudgeCategories.ToList()
            };

            return View(vm);
        }
        [HttpPost]
        public IActionResult ApproveJudge(int judgeId)
        {
            // Onaylanacak hakemi buluyoruz
            var judge = db.Judges.FirstOrDefault(j => j.Id == judgeId);

            if (judge != null)
            {
                judge.IsApproved = true; // Hakemi onaylıyoruz
                db.SaveChanges();
            }

            return RedirectToAction("PendingApproval"); // Onay bekleyen sayfaya geri dönüyoruz
        }

    }
}
