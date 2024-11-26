using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
    [Area("Management")]
    public class ContestantController : Controller
    {
        CompetitionDbContext db = new CompetitionDbContext();
        public IActionResult Contestant(string searchQuery, int page = 1, int pageSize = 10)
                {
                        var filteredContestants = db.Contestants
              .Include(c => c.Projects)
                  .ThenInclude(p => p.ProjectCategory)
              .Include(c => c.contestantProfil)
              .Include(c => c.ContestantCategory)
              .Where(c =>
                  string.IsNullOrEmpty(searchQuery) ||
                  c.contestantProfil.FullName.Contains(searchQuery) ||
                  c.ContestantCategory.Name.Contains(searchQuery) ||
                  c.Projects.Name.Contains(searchQuery) ||
                  c.Projects.ProjectCategory.Name.Contains(searchQuery))
              .Skip((page - 1) * pageSize)
              .Take(pageSize)
              .ToList();


            var model = new ContestantTableVM
            {
                Contestants = filteredContestants.Select(c => new ContestantViewModel
                {
                    ContestantId = c.Id,
                    ContestantName = c.contestantProfil?.FullName,
                    ContestantCategoryName = c.ContestantCategory?.Name,
                    ProjectName = c.Projects?.Name,
                    ProjectCategoryName = c.Projects?.ProjectCategory?.Name,
                    AssignedAcademicJudgeName = db.ProjectEvaluations
                        .Where(pe => pe.ProjectId == c.Projects.Id && pe.JudgeCategoryId == 1)
                        .Select(pe => pe.Judge.JudgeProfil.FullName)
                        .FirstOrDefault(), // Akademik hakem adı
                    AssignedIndustrialJudgeName = db.ProjectEvaluations
                        .Where(pe => pe.ProjectId == c.Projects.Id && pe.JudgeCategoryId == 2)
                        .Select(pe => pe.Judge.JudgeProfil.FullName)
                        .FirstOrDefault(), // Endüstriyel hakem adı
                    IsAcademicJudgeAssigned = db.ProjectEvaluations.Any(pe => pe.ProjectId == c.Projects.Id && pe.JudgeCategoryId == 1),
                    IsIndustrialJudgeAssigned = db.ProjectEvaluations.Any(pe => pe.ProjectId == c.Projects.Id && pe.JudgeCategoryId == 2),
                }).ToList(),
                TotalContestants = filteredContestants.Count(),
                PageSize = pageSize,
                CurrentPage = page,
                SearchQuery = searchQuery
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult ExportToExcel(string searchQuery)
        {
            // Yarışmacı ve ilişkili verileri getir
            var contestants = db.Contestants
                .Include(c => c.Projects)
                    .ThenInclude(p => p.ProjectCategory)
                .Include(c => c.contestantProfil)
                .Include(c => c.ContestantCategory)
                .ToList();

            // Excel oluşturmak için ClosedXML kullanıyoruz
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Yarışmacılar ve Hakemler");

                // Başlıkları ekle
                worksheet.Cell(1, 1).Value = "Sıra No";
                worksheet.Cell(1, 2).Value = "Yarışmacı Adı";
                worksheet.Cell(1, 3).Value = "Kategori Adı";
                worksheet.Cell(1, 4).Value = "Proje Adı";
                worksheet.Cell(1, 5).Value = "Proje Kategorisi";
                worksheet.Cell(1, 6).Value = "Atanmış Akademik Hakem";
                worksheet.Cell(1, 7).Value = "Atanmış Endüstriyel Hakem";

                int row = 2; // Başlıklardan sonra veriler 2. satırdan başlar

                foreach (var contestant in contestants)
                {
                    // Akademik ve Endüstriyel hakemleri belirle
                    var academicJudgeName = db.ProjectEvaluations
                        .Where(pe => pe.ProjectId == contestant.Projects.Id && pe.JudgeCategoryId == 1)
                        .Select(pe => pe.Judge.JudgeProfil.FullName)
                        .FirstOrDefault();

                    var industrialJudgeName = db.ProjectEvaluations
                        .Where(pe => pe.ProjectId == contestant.Projects.Id && pe.JudgeCategoryId == 2)
                        .Select(pe => pe.Judge.JudgeProfil.FullName)
                        .FirstOrDefault();

                    // Excel'e yarışmacı bilgilerini ekle
                    worksheet.Cell(row, 1).Value = contestant.Id;
                    worksheet.Cell(row, 2).Value = contestant.contestantProfil?.FullName ?? "Bilinmiyor";
                    worksheet.Cell(row, 3).Value = contestant.ContestantCategory?.Name ?? "Bilinmiyor";
                    worksheet.Cell(row, 4).Value = contestant.Projects?.Name ?? "Bilinmiyor";
                    worksheet.Cell(row, 5).Value = contestant.Projects?.ProjectCategory?.Name ?? "Bilinmiyor";
                    worksheet.Cell(row, 6).Value = academicJudgeName ?? "Atanmamış";
                    worksheet.Cell(row, 7).Value = industrialJudgeName ?? "Atanmamış";

                    row++; // Bir sonraki satıra geç
                }

                // Sütun genişliklerini otomatik ayarla
                worksheet.Columns().AdjustToContents();

                // Excel dosyasını oluştur ve döndür
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;

                    return File(
                        stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "YarismacilarVeHakemler.xlsx"
                    );
                }
            }
        }


    }
}
