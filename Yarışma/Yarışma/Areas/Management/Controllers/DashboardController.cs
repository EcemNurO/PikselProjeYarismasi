using Microsoft.AspNetCore.Mvc;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
    [Area("Management")]
    public class DashboardController : Controller
    {

        CompetitionDbContext db = new CompetitionDbContext();
        public IActionResult Index()
        {

            
            return View();
        }
    }
}