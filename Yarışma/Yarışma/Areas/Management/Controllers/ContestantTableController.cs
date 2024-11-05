using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;
using System.IO;
using System.Linq;

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
                .Include(c => c.ContestantJudges)
                    .ThenInclude(cj => cj.Judge)
                        .ThenInclude(j => j.JudgeCategory)
                .AsQueryable();

            // Arama filtresi
            if (!string.IsNullOrEmpty(searchTerm))
            {
                contestantsQuery = contestantsQuery.Where(c =>
                    (c.contestantProfil.FullName != null && c.contestantProfil.FullName.Contains(searchTerm)) ||
                    (c.ContestantCategory != null && c.ContestantCategory.Name.Contains(searchTerm)) ||
                    c.Projects.Any(p => (p.Name != null && p.Name.Contains(searchTerm)) ||
                                        (p.ProjectCategory != null && p.ProjectCategory.Name.Contains(searchTerm)))
                );
            }

            var totalContestants = contestantsQuery.Count();
            var contestants = contestantsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var vm = new ContestantTableVM
            {
                Contestant = contestants,
                TotalCount = totalContestants,
                PageSize = pageSize,
                CurrentPage = page,
                contestantCategories = db.ContestantCategories.ToList(),
                projects = db.Projects.Include(p => p.ProjectCategory).ToList(),
                JudgeCategories = db.JudgeCategories.ToList(),
                Judge = db.Judges.Include(j => j.JudgeCategory).ToList(),
                ContestantJudges = db.ContestantJudges.ToList()
            };
            ViewBag.SearchTerm = searchTerm;
            return View(vm);
        }

        public IActionResult GetContestants(string searchTerm, int page = 1, int pageSize = 10)
        {
            var contestantsQuery = db.Contestants
                .Include(c => c.contestantProfil)
                .Include(c => c.ContestantCategory)
                .Include(c => c.Projects)
                    .ThenInclude(p => p.ProjectCategory)
                .Include(c => c.ContestantJudges)
                    .ThenInclude(cj => cj.Judge)
                        .ThenInclude(j => j.JudgeCategory)
                .AsQueryable();

            // Arama filtresi
            if (!string.IsNullOrEmpty(searchTerm))
            {
                contestantsQuery = contestantsQuery.Where(c =>
                    (c.contestantProfil.FullName != null && c.contestantProfil.FullName.Contains(searchTerm)) ||
                    (c.ContestantCategory != null && c.ContestantCategory.Name.Contains(searchTerm)) ||
                    c.Projects.Any(p => (p.Name != null && p.Name.Contains(searchTerm)) ||
                                        (p.ProjectCategory != null && p.ProjectCategory.Name.Contains(searchTerm)))
                );
            }

            var totalContestants = contestantsQuery.Count();
            var contestants = contestantsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var vm = new ContestantTableVM
            {
                Contestant = contestants,
                TotalCount = totalContestants,
                PageSize = pageSize,
                CurrentPage = page,
                contestantCategories = db.ContestantCategories.ToList(),
                projects = db.Projects.Include(p => p.ProjectCategory).ToList(),
                JudgeCategories = db.JudgeCategories.ToList(),
                Judge = db.Judges.Include(j => j.JudgeCategory).ToList(),
                ContestantJudges = db.ContestantJudges.ToList()
            };

            ViewBag.SearchTerm = searchTerm;
            return PartialView("_AppreciationPartial", vm);
        }

        public IActionResult ExportToExcel(string searchTerm)
        {
            var contestantsQuery = db.Contestants
                .Include(c => c.contestantProfil)
                .Include(c => c.ContestantCategory)
                .Include(c => c.Projects)
                    .ThenInclude(p => p.ProjectCategory)
                .Include(c => c.ContestantJudges)
                    .ThenInclude(cj => cj.Judge)
                        .ThenInclude(j => j.JudgeCategory)
                .AsQueryable();

            // Arama filtresi
            if (!string.IsNullOrEmpty(searchTerm))
            {
                contestantsQuery = contestantsQuery.Where(c =>
                    (c.contestantProfil.FullName != null && c.contestantProfil.FullName.Contains(searchTerm)) ||
                    (c.ContestantCategory != null && c.ContestantCategory.Name.Contains(searchTerm)) ||
                    c.Projects.Any(p => (p.Name != null && p.Name.Contains(searchTerm)) ||
                                        (p.ProjectCategory != null && p.ProjectCategory.Name.Contains(searchTerm)))
                );
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Yarışmacılar");

                worksheet.Cell(1, 1).Value = "Ad Soyad";
                worksheet.Cell(1, 2).Value = "Kategori";
                worksheet.Cell(1, 3).Value = "Tematik Alan";
                worksheet.Cell(1, 4).Value = "Proje Adı";
                worksheet.Cell(1, 5).Value = "Proje Durumu";
                worksheet.Cell(1, 6).Value = "Sanayici Hakemi";
                worksheet.Cell(1, 7).Value = "Akademisyen Hakemi";
                worksheet.Cell(1, 8).Value = "Ortalama Puan";

                int row = 2;
                foreach (var contestant in contestantsQuery.ToList())
                {
                    var project = contestant.Projects.FirstOrDefault();
                    var sanayiciHakemi = contestant.ContestantJudges
                        .Where(cj => cj.Judge.JudgeCategory.Name == "Sanayici")
                        .Select(cj => cj.Judge)
                        .FirstOrDefault();

                    var akademisyenHakemi = contestant.ContestantJudges
                        .Where(cj => cj.Judge.JudgeCategory.Name == "Akademisyen")
                        .Select(cj => cj.Judge)
                        .FirstOrDefault();

                    var sanayiciPuan = sanayiciHakemi?.ProjectEvaluation?.Score ?? 0;
                    var akademisyenPuan = akademisyenHakemi?.ProjectEvaluation?.Score ?? 0;
                    var ortalamaPuan = (sanayiciPuan + akademisyenPuan) / 2;

                    worksheet.Cell(row, 1).Value = contestant.contestantProfil?.FullName ?? "Ad Soyad Yok";
                    worksheet.Cell(row, 2).Value = contestant.ContestantCategory?.Name ?? "Kategori Yok";
                    worksheet.Cell(row, 3).Value = project?.ProjectCategory?.Name ?? "Tematik Alan Yok";
                    worksheet.Cell(row, 4).Value = project?.Name ?? "Proje Adı Yok";
                    worksheet.Cell(row, 5).Value = project?.Status == true ? "Tamamlandı" : "Devam Ediyor";
                    worksheet.Cell(row, 6).Value = sanayiciHakemi?.JudgeProfil?.FullName ?? "Sanayici Hakemi Yok";
                    worksheet.Cell(row, 7).Value = akademisyenHakemi?.JudgeProfil?.FullName ?? "Akademisyen Hakemi Yok";
                    worksheet.Cell(row, 8).Value = ortalamaPuan > 0 ? ortalamaPuan.ToString("F2") : "Puan Yok";

                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Yarismacilar.xlsx");
                }
            }
        }
    }
}
