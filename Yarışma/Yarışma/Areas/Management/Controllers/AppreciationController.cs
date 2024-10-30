using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
    [Area("Management")]
    public class AppreciationController : Controller
    {
        CompetitionDbContext db = new CompetitionDbContext();
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var contestants =db.Contestants
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            var totalContestants = db.Contestants.Count();

            var vm = new ContestantTableVM
            {
                Contestant = contestants,
                TotalCount = totalContestants,
                PageSize = pageSize,
                CurrentPage = page,
                contestantCategories = db.ContestantCategories.ToList(),
                projects = db.Projects.ToList(),
                JudgeCategories = db.JudgeCategories.ToList(),
                Judge = db.Judges.ToList(),
                ContestantJudges = db.ContestantJudges.ToList()
            };
            return View(vm);

        }   
       
        public IActionResult GetContestants(int page = 1, int pageSize = 10)
        {
            var contestants = db.Contestants
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalContestants = db.Contestants.Count();

            var vm = new ContestantTableVM
            {
                Contestant = contestants,
                TotalCount = totalContestants,
                PageSize = pageSize,
                CurrentPage = page,
                contestantCategories = db.ContestantCategories.ToList(),
                projects = db.Projects.ToList(),
                JudgeCategories = db.JudgeCategories.ToList(),
                Judge = db.Judges.ToList(),
                ContestantJudges = db.ContestantJudges.ToList()
            };

            return PartialView("_AppreciationPartial", vm);

            
         }

        public IActionResult ExportToExcel()
        {
            // Yarışmacılar, projeler ve hakemler toplu olarak veritabanından alınıyor
            var contestants = db.Contestants.ToList();
            var projects = db.Projects.ToList();
            var judges = db.Judges.ToList();
            var contestantJudges = db.ContestantJudges.ToList();

            // Excel dosyasını oluşturma
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Yarışmacılar");

                // Başlıklar
                worksheet.Cell(1, 1).Value = "Ad Soyad";
                worksheet.Cell(1, 2).Value = "Kategori";
                worksheet.Cell(1, 3).Value = "Tematik Alan";
                worksheet.Cell(1, 4).Value = "Proje Adı";
                worksheet.Cell(1, 5).Value = "Proje Durumu";
                worksheet.Cell(1, 6).Value = "Sanayici Hakemi";
                worksheet.Cell(1, 7).Value = "Akademisyen Hakemi";
                worksheet.Cell(1, 8).Value = "Ortalama Puan";

                // Yarışmacılar arasında gezinme
                int row = 2;
                foreach (var contestant in contestants)
                {
                    // Yarışmacıya ait proje ve hakemler
                    var project = projects.FirstOrDefault(p => p.ContestantId == contestant.Id);
                    var sanayiciHakemi = judges.FirstOrDefault(j => j.JudgeCategory.Name == "Sanayici" && contestantJudges.Any(cj => cj.ContestantId == contestant.Id && cj.JudgeId == j.Id));
                    var akademisyenHakemi = judges.FirstOrDefault(j => j.JudgeCategory.Name == "Akademisyen" && contestantJudges.Any(cj => cj.ContestantId == contestant.Id && cj.JudgeId == j.Id));

                    // Hakemlerin verdiği puanlar (Eğer mevcutsa)
                    var sanayiciPuan = sanayiciHakemi?.ProjectEvaluation?.Score ?? 0;
                    var akademisyenPuan = akademisyenHakemi?.ProjectEvaluation?.Score ?? 0;
                    var ortalamaPuan = (sanayiciPuan + akademisyenPuan) / 2;

                    // Sadece puan almış yarışmacılar ekleniyor
                    if (ortalamaPuan > 0)
                    {
                        worksheet.Cell(row, 1).Value = contestant.contestantProfil?.FullName ?? "Ad Soyad Yok";
                        worksheet.Cell(row, 2).Value = db.ContestantCategories.FirstOrDefault(c => c.Id == contestant.ContestantCategoryId)?.Name ?? "Kategori Yok";
                        worksheet.Cell(row, 3).Value = project?.ProjectCategory?.Name ?? "Tematik Alan Yok";
                        worksheet.Cell(row, 4).Value = project?.Name ?? "Proje Adı Yok";
                        worksheet.Cell(row, 5).Value = project?.Status == true ? "Tamamlandı" : "Devam Ediyor";
                        worksheet.Cell(row, 6).Value = sanayiciHakemi?.JudgeProfil?.FullName ?? "Sanayici Hakemi Yok";
                        worksheet.Cell(row, 7).Value = akademisyenHakemi?.JudgeProfil?.FullName ?? "Akademisyen Hakemi Yok";
                        worksheet.Cell(row, 8).Value = ortalamaPuan.ToString("F2");

                        row++;
                    }
                }

                // Excel dosyasını hafızaya kaydet
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    // Excel dosyasını kullanıcıya indirme olarak gönder
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Yarismacilar.xlsx");
                }
            }







        }



}
    
}
