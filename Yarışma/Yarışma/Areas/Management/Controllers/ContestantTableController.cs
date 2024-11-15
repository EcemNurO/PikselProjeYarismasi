using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using System.Reflection.Metadata;

namespace Yarışma.Areas.Management.Controllers
{
    [Area("Management")]
    public class ContestantTableController : Controller
    {
        private readonly CompetitionDbContext db = new CompetitionDbContext();

        public IActionResult Index(string searchTerm, int page = 1, int pageSize = 10)
        {
            var contestantsQuery = db.Contestants
       .Include(c => c.contestantProfil)
       .Include(c => c.ContestantCategory)
       .Include(c => c.Projects)
       .ThenInclude(p => p.ProjectCategory)
       .AsQueryable();

            // Eğer arama terimi varsa, yarışmacıları filtrele
            if (!string.IsNullOrEmpty(searchTerm))
            {
                contestantsQuery = contestantsQuery
                    .Where(c => c.contestantProfil.FullName.Contains(searchTerm) ||
                                c.ContestantCategory.Name.Contains(searchTerm));
            }

            // Sayfalama ile birlikte verileri çek
            var contestants = contestantsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Toplam yarışmacı sayısını hesapla
            var totalContestants = contestantsQuery.Count();

            ViewBag.TotalContestants = totalContestants;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTerm = searchTerm;

            return View(contestants);
        }
        public IActionResult Details(int id)
        {
             var contestant = db.Contestants
                        .Include(c => c.Projects)
                            .ThenInclude(p => p.ProjectCategory) // Proje kategorisini dahil et
                        .Include(c => c.Projects)
                            .ThenInclude(p => p.ProjectQuestions) // Projeye ait soruları dahil et
                       /* .ThenInclude(q => q.ProjectAnswers) */// Sorunun yanıtlarını dahil et
                        .FirstOrDefault(c => c.Id == id);

            if (contestant == null)
            {
                return NotFound();
            }

            return View(contestant);
        }
        public IActionResult ExportToExcel()
        {
            var contestants =db.Contestants
                                .Include(c => c.contestantProfil)
                                .Include(c => c.ContestantCategory)
                                .ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Yarışmacılar");

                // Başlıkları yazıyoruz
                worksheet.Cells[1, 1].Value = "Yarışmacı Adı";
                worksheet.Cells[1, 2].Value = "Kategori";
                worksheet.Cells[1, 3].Value = "Proje Sayısı";

                // Verileri yazıyoruz
                int row = 2;
                foreach (var contestant in contestants)
                {
                    worksheet.Cells[row, 1].Value = contestant.contestantProfil.FullName;
                    worksheet.Cells[row, 2].Value = contestant.ContestantCategory.Name;
                    worksheet.Cells[row, 3].Value = contestant.Projects.Count;
                    row++;
                }

                // Stil ayarlamaları
                worksheet.Cells[1, 1, row - 1, 3].AutoFitColumns();
                worksheet.Cells[1, 1, 1, 3].Style.Font.Bold = true;

                // Excel dosyasını byte dizisi olarak alıyoruz
                var fileContents = package.GetAsByteArray();

                // Excel dosyasını indirme olarak sunuyoruz
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Yarismacilar.xlsx");
            }






        }
    }
}