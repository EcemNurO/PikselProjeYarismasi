using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
    [Area("Management")]
    public class UnAppreciationController : Controller
    {
       
            CompetitionDbContext db = new CompetitionDbContext();

        public IActionResult Index(string searchTerm, int page = 1, int pageSize = 10)
        {
            // Yarışmacı sorgusunu al
            var contestantsQuery = GetContestantsQuery(searchTerm);
            var contestantsList = contestantsQuery.ToList();

            // Puanı olan yarışmacıları al
            var contestantsWithScore = GetContestantsWithScore(contestantsList, page, pageSize);

            // Toplam yarışmacı sayısını hesapla (sadece puanlı olanlar)
            var totalContestantsWithScore = contestantsList.Count(c => contestantsWithScore.Contains(c));

            // ViewModel oluştur
            var vm = new ContestantTableVM
            {
                Contestant = contestantsWithScore,
                TotalCount = totalContestantsWithScore,
                PageSize = pageSize,
                CurrentPage = page,
                contestantCategories = db.ContestantCategories.ToList(),
                projects = db.Projects.ToList(),
                JudgeCategories = db.JudgeCategories.ToList(),
                Judge = db.Judges.ToList(),
                ContestantJudges = db.ContestantJudges.ToList()
            };

            ViewBag.SearchTerm = searchTerm;
            return View(vm);
        }


        public IActionResult GetContestants(string searchTerm, int page = 1, int pageSize = 10)
        {
            var contestantsQuery = GetContestantsQuery(searchTerm);
            var contestantsList = contestantsQuery.ToList();

            var contestantsWithScore = GetContestantsWithScore(contestantsList, page, pageSize);

            var vm = new ContestantTableVM
            {
                Contestant = contestantsWithScore,
                TotalCount = contestantsList.Count(c => contestantsWithScore.Contains(c)),
                PageSize = pageSize,
                CurrentPage = page,
                contestantCategories = db.ContestantCategories.ToList(),
                projects = db.Projects.ToList(),
                JudgeCategories = db.JudgeCategories.ToList(),
                Judge = db.Judges.ToList(),
                ContestantJudges = db.ContestantJudges.ToList()
            };

            ViewBag.SearchTerm = searchTerm;
            return PartialView("_AppreciationPartial", vm);
        }

        public IActionResult ExportToExcel()
        {
            var contestantsQuery = GetContestantsQuery(null);
            var contestants = contestantsQuery.ToList();
            var projects = db.Projects.ToList();
            var judges = db.Judges.Include(j => j.JudgeProfil).ToList();
            var contestantJudges = db.ContestantJudges.ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = CreateExcelWorksheet(workbook, contestants, projects, judges, contestantJudges);

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Yarismacilar.xlsx");
                }
            }
        }

        // Helper Methods
        public IQueryable<Contestant> GetContestantsQuery(string searchTerm)
        {
            var query = db.Contestants
                .Include(c => c.contestantProfil)
                .Include(c => c.ContestantCategory)
                .Include(c => c.Projects)
                    .ThenInclude(p => p.ProjectCategory)
                .Include(c => c.ContestantJudges)
                    .ThenInclude(cj => cj.Judge)
                        .ThenInclude(j => j.JudgeCategory)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c =>
                    (c.contestantProfil.FullName != null && c.contestantProfil.FullName.Contains(searchTerm)) ||
                    (c.ContestantCategory != null && c.ContestantCategory.Name.Contains(searchTerm)) ||
                    c.Projects.Any(p => (p.Name != null && p.Name.Contains(searchTerm)) ||
                                        (p.ProjectCategory != null && p.ProjectCategory.Name.Contains(searchTerm)))
                );
            }
            return query;
        }

        public List<Contestant> GetContestantsWithScore(List<Contestant> contestantsList, int page, int pageSize)
        {
            return contestantsList
                .Select(contestant =>
                {
                    var sanayiciPuan = db.ContestantJudges
                        .Where(cj => cj.ContestantId == contestant.Id && cj.Judge.JudgeCategory.Name == "Sanayici")
                        .Select(cj => cj.Judge.ProjectEvaluation.Score)
                        .FirstOrDefault();

                    var akademisyenPuan = db.ContestantJudges
                        .Where(cj => cj.ContestantId == contestant.Id && cj.Judge.JudgeCategory.Name == "Akademisyen")
                        .Select(cj => cj.Judge.ProjectEvaluation.Score)
                        .FirstOrDefault();

                    var ortalamaPuan = (sanayiciPuan + akademisyenPuan) / 2;

                    return new { Contestant = contestant, AverageScore = ortalamaPuan };
                })
                .Where(x => x.AverageScore == null || x.AverageScore <= 0) // Puanı olmayan veya 0 olan yarışmacıları al
                .Select(x => x.Contestant)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }


        public IXLWorksheet CreateExcelWorksheet(XLWorkbook workbook, List<Contestant> contestants, List<Project> projects, List<Judge> judges, List<ContestantJudge> contestantJudges)
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
            foreach (var contestant in contestants)
            {
                var project = projects.FirstOrDefault(p => p.ContestantId == contestant.Id);
                var sanayiciHakemi = judges.FirstOrDefault(j => j.JudgeCategory.Name == "Sanayici" &&
                                        contestantJudges.Any(cj => cj.ContestantId == contestant.Id && cj.JudgeId == j.Id));
                var akademisyenHakemi = judges.FirstOrDefault(j => j.JudgeCategory.Name == "Akademisyen" &&
                                        contestantJudges.Any(cj => cj.ContestantId == contestant.Id && cj.JudgeId == j.Id));

                var sanayiciPuan = sanayiciHakemi?.ProjectEvaluation?.Score ?? 0;
                var akademisyenPuan = akademisyenHakemi?.ProjectEvaluation?.Score ?? 0;
                var ortalamaPuan = (sanayiciPuan + akademisyenPuan) / 2;

                if (ortalamaPuan > 0)
                {
                    worksheet.Cell(row, 1).Value = contestant.contestantProfil?.FullName ?? "Ad Soyad Yok";
                    worksheet.Cell(row, 2).Value = contestant.ContestantCategory?.Name ?? "Kategori Yok";
                    worksheet.Cell(row, 3).Value = project?.ProjectCategory?.Name ?? "Tematik Alan Yok";
                    worksheet.Cell(row, 4).Value = project?.Name ?? "Proje Adı Yok";
                    worksheet.Cell(row, 5).Value = project?.Status == true ? "Tamamlandı" : "Devam Ediyor";
                    worksheet.Cell(row, 6).Value = sanayiciHakemi?.JudgeProfil?.FullName ?? "Sanayici Hakemi Yok";
                    worksheet.Cell(row, 7).Value = akademisyenHakemi?.JudgeProfil?.FullName ?? "Akademisyen Hakemi Yok";
                    worksheet.Cell(row, 8).Value = ortalamaPuan.ToString("F2");

                    row++;
                }
            }
            return worksheet;
        }

    }
    }

