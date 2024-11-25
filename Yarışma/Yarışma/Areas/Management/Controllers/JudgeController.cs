using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Collections.Generic;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
    [Area("Management")]
    public class JudgeController : Controller
    {
        CompetitionDbContext db = new CompetitionDbContext();
        public IActionResult Judge(int page = 1, int pageSize = 10)
        {
            var totalJudges = db.Judges.Count();

            // Hakemleri çekme ve sayfalama
            var judges = db.Judges
                .Include(j => j.JudgeProfil)
                .Include(j => j.JudgeCategory)
                .Include(j => j.ProjectCategory)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // ViewModel'in doldurulması


            var model = new JudgeTableVM
            {
                Judges = judges.Select(c => new JudgeTableViewModel
                {
                    JudgeId = c.Id,
                    FullName = c.JudgeProfil?.FullName,
                    Phone = c.JudgeProfil?.Phone,
                    Email = c.JudgeProfil?.Email,
                    //University = c.JudgeProfil?.Univercity,
                    JudgeCategoryName = c.JudgeCategory?.Name,
                    ProjectCategories = c.ProjectCategory != null
                        ? c.ProjectCategory.Name // Tekil nesne durumu
                        : "Belirtilmemiş"
                }).ToList(),

                TotalCount = totalJudges,
                PageSize = pageSize,
                CurrentPage = page
            };

            return View(model);
        }
    }
}
