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
	public class PeriodsController : Controller
	{

		CompetitionDbContext db = new CompetitionDbContext();


		// GET: Management/Periods
		public async Task<IActionResult> Index()
		{
			var Period = db.Periods.ToList();
			return View(Period);
		}

		// GET: Management/Periods/Details/5
		public async Task<IActionResult> Details(int? id)
		{


			return View();
		}

		// GET: Management/Periods/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Management/Periods/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Period model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					
					db.Periods.Add(model);
					db.SaveChanges();
					return RedirectToAction(nameof(Index));
				}
				return View(model);
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				return View(model);
			}
		}

		// GET: Management/Periods/Edit/5
		public IActionResult Edit(int id)
		{
			var Period = db.Periods.Find(id);
			if (Period == null)
			{
				return RedirectToAction(nameof(Index));
			}

			return View(Period);
		}

		// POST: Management/Periods/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Period model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var EditPeriod = db.Periods.Find(model.Id);
					if (EditPeriod == null)
					{
						return RedirectToAction(nameof(Index));
					}
					EditPeriod.PeriodName = model.PeriodName;
					EditPeriod.ContestanStartDate = model.ContestanStartDate;
					EditPeriod.ContestantEndDate = model.ContestantEndDate;
					EditPeriod.ProjectStartDate = model.ProjectStartDate;
					EditPeriod.ProjectEndDate = model.ProjectEndDate;
					EditPeriod.JudgeStartDate = model.JudgeStartDate;
					EditPeriod.JudgeEndDate = model.JudgeEndDate;
					
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

		// GET: Management/Periods/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{

			var Period = db.Periods.Find(id);
			if (Period == null)
			{
				return RedirectToAction(nameof(Index));
			}
			return RedirectToAction(nameof(Index));

		}

		// POST: Management/Periods/Delete/5
		// POST: TrainerController/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			try
			{
				var Period = db.Periods.Find(id);
				if (Period == null)
				{
					return RedirectToAction(nameof(Index));
				}
				db.Periods.Remove(Period);
				db.SaveChanges();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception)
			{

				return RedirectToAction(nameof(Index));
			}
		}
	}
}