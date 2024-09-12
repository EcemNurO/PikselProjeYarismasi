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
    public class ContactsController : Controller
    {
       

      

        // GET: Management/Contacts
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Management/Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            

            return View();
        }

        // GET: Management/Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Management/Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        

        // GET: Management/Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            
            return View();
        }

        

        // GET: Management/Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            

            return View();
        }

        

       
    }
}
