using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Yarışma.Models;

namespace Yarışma.Controllers
{
    public class ContestantsController : Controller
    {
        private readonly CompetitionDbContext _context;

        public ContestantsController(CompetitionDbContext context)
        {
            _context = context;
        }
        CompetitionDbContext db = new CompetitionDbContext();
        // GET: Contestants
        public async Task<IActionResult> Index()
        {
            var competitionDbContext = _context.Contestants.Include(c => c.ContestantCategory).Include(c => c.contestantProfil);
            return View(await competitionDbContext.ToListAsync());
        }

        // GET: Contestants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contestant = await _context.Contestants
                .Include(c => c.ContestantCategory)
                .Include(c => c.contestantProfil)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contestant == null)
            {
                return NotFound();
            }

            return View(contestant);
        }

        // GET: Contestants/Create
        public IActionResult Create()
        {
            ViewData["ContestantCategoryId"] = new SelectList(_context.ContestantCategories, "Id", "Id");
            ViewData["ContestantProfilId"] = new SelectList(_context.ContestantProfils, "Id", "Id");
            return View();
        }

        // POST: Contestants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ContestantProfilId,ContestantCategoryId")] Contestant contestant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contestant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContestantCategoryId"] = new SelectList(_context.ContestantCategories, "Id", "Id", contestant.ContestantCategoryId);
            ViewData["ContestantProfilId"] = new SelectList(_context.ContestantProfils, "Id", "Id", contestant.ContestantProfilId);
            return View(contestant);
        }

        // GET: Contestants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contestant = await _context.Contestants.FindAsync(id);
            if (contestant == null)
            {
                return NotFound();
            }
            ViewData["ContestantCategoryId"] = new SelectList(_context.ContestantCategories, "Id", "Id", contestant.ContestantCategoryId);
            ViewData["ContestantProfilId"] = new SelectList(_context.ContestantProfils, "Id", "Id", contestant.ContestantProfilId);
            return View(contestant);
        }

        

        
      
    }
}
