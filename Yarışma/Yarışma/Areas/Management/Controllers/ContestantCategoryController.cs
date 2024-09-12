using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
    [Area("Management")]
    public class ContestantCategoryController : Controller
    {
		CompetitionDbContext db = new CompetitionDbContext();


		

        // GET: Management/ContestantCategory
        public async Task<IActionResult> Index()
        {
            var ContestantCategory = db.ContestantCategories.ToList();
            return View(ContestantCategory);
        }

        // GET: Management/ContestantCategory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var ContestantCategory = db.ContestantCategories.Find(id);
            if (ContestantCategory == null)
            {
                return RedirectToAction("Index");
            }


            return View(ContestantCategory);
        }

        // GET: Management/ContestantCategory/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContestantCategory model)
        {
            try
            {
                if (ModelState.IsValid) { 

                  db.ContestantCategories.Add(model);
                    db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch (Exception ex)
            {
				ViewBag.ErrorMessage = ex.Message;
				return View (model);
            }
        }
       

        // GET: Management/ContestantCategory/Edit/5
        public ActionResult Edit(int id)
        {
            var ContestantCategory = db.ContestantCategories.Find(id);
                if (ContestantCategory == null)
            {
                return RedirectToAction(nameof(Index));

            }

            return View(ContestantCategory);
        }

        // POST: Management/ContestantCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContestantCategory model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var EditContestantCat=db.ContestantCategories.Find(model.Id);
                    if(EditContestantCat == null)
                    {  return RedirectToAction(nameof(Index));
                    }
                    EditContestantCat.Name = model.Name;
                    db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
				return View(model);
			}
            catch 
            {

				return View(model);
			}
        }

        // GET: Management/ContestantCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
			var ContestantCategory = db.ContestantCategories.Find(id);
			if (ContestantCategory == null)
			{
				return RedirectToAction(nameof(Index));

			}

			return View(ContestantCategory);
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
				var ContestantCategory = db.ContestantCategories.Find(id);
				if (ContestantCategory == null)
				{
					return RedirectToAction(nameof(Index));
				}
                db.ContestantCategories.Remove(ContestantCategory);
                db.SaveChanges();
			
				return RedirectToAction(nameof(Index));
			}
            catch (Exception)
            {

                throw;
            }
        }
       
        
    }
}
