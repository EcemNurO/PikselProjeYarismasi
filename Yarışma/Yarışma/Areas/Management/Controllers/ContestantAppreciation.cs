using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yarışma.Models;


namespace Yarışma.Areas.Management.Controllers
{
    public class ContestantAppreciation : Controller
    {
        CompetitionDbContext db = new CompetitionDbContext();
        

     

        public IActionResult Index()
        {

            var projects = db.Projects
              .Include(p => p.Contestant) 
              .Include(p => p.ProjectCategory) 
              .ToList();
            return View(projects);
        }
    }
}
