using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TAApplication.Areas.Data;
using TAApplication.Data;
using TAApplication.Models;
using ActionNameAttribute = Microsoft.AspNetCore.Mvc.ActionNameAttribute;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;
using BindAttribute = Microsoft.AspNetCore.Mvc.BindAttribute;

using ValidateAntiForgeryTokenAttribute = Microsoft.AspNetCore.Mvc.ValidateAntiForgeryTokenAttribute;
using System.Web.Mvc;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace TAApplication.Controllers
{
    public class ApplicationController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TAUser> _um;

        public ApplicationController(ApplicationDbContext context, UserManager<TAUser> um)
        {
            _context = context;
            _um = um;
        }

        // GET: Application
        public async Task<IActionResult> Index()
        {   
              return View(await _context.Application.Include(o=>o.User).ToListAsync());
        }

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Application == null)
            {
                return NotFound();
            }

            var application = await _context.Application
                .FirstOrDefaultAsync(m => m.ID == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // GET: Application/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Application/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] 
        [Authorize]
        public async Task<IActionResult> Create([Bind("ID,Pursuing,GPA,Department,numberOfHour,avaiableBefore,SemestersCount")] Application application)
        {
            ModelState.Remove("User");
            application.User = await _um.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                _context.Add(application);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(application);
        }

        // GET: Application/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Application == null)
            {
                return NotFound();
            }

            var application = await _context.Application.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            return View(application);
        }

        // POST: Application/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*        [Authorize(Roles = "Admin,Applicant")]
                [HttpPost, ActionName("Edit")]
                [ValidateAntiForgeryToken]
                public ActionResult EditPost(int? id)
                {
                    if (id == null)
                    {
                        return BadRequest();
                    }
                    var studentToUpdate = _context.Application.Where(o => o.ID == id).Include(o => o.User).FirstOrDefault();
                    if (studentToUpdate != null) 
                    {
                        if (TryUpdateModelAsync<Application>(studentToUpdate, "", studentToUpdate => studentToUpdate.Pursuing, studentToUpdate => studentToUpdate.GPA))
                        {
                            try {
                                _context.SaveChanges();
                                return RedirectToAction("Details", new { id = studentToUpdate.ID });
                            }
                            catch (DataException *//* dex *//*)
                            {
                                // manage error logging
                                }
                        }
                    }
                    return View(studentToUpdate);

                }*/
        [Authorize(Roles = "Admin,Applicant")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("ID,Pursuing,GPA,Department,numberOfHour,avaiableBefore,SemestersCount")] Application application)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var applicationToUpdate = _context.Application.Where(o => o.ID == id).Include(o => o.User).FirstOrDefault();

            if (applicationToUpdate != null)
            {
                if (await TryUpdateModelAsync<Application>(applicationToUpdate, "",
                           s => s.Pursuing,
                           s => s.GPA)) {
                    try {
                        _context.SaveChanges();
                        return RedirectToAction("Details", new { id = applicationToUpdate.ID });
                    }
                    catch (DataException /* dex */)
                    {
                        // manage error logging
                        }
                }
                    }
            return View(applicationToUpdate);
        }

        // GET: Application/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Application == null)
            {
                return NotFound();
            }

            var application = await _context.Application
                .FirstOrDefaultAsync(m => m.ID == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: Application/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Application == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Application'  is null.");
            }
            var application = await _context.Application.FindAsync(id);
            if (application != null)
            {
                _context.Application.Remove(application);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationExists(int id)
        {
          return _context.Application.Any(e => e.ID == id);
        }
    }
}
