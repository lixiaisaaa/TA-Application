/**
* Author:    Xia Li
* Partner:   Wenlin Li
* Date:      10/17/2022
* Course:    CS 4540, University of Utah, School of Computing
* Copyright: CS 4540 and Xia Li and Wenlin Li - This work may not be copied for use in Academic Coursework.
*
* I, Xia Li and Wenlin Li, certify that I wrote this code from scratch and did 
* not copy it in part or whole from another source.  Any references used 
* in the completion of the assignment are cited in my README file and in
* the appropriate method header.
*
* File Contents
*
* This c# for whole controller.
*/

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
using Microsoft.Extensions.Hosting.Internal;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.IO;
using WebNetCore5_Img_Storage.Model.Tool;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TAApplication.Controllers
{
    public class ApplicationController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TAUser> _um;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ApplicationController(ApplicationDbContext context, UserManager<TAUser> um, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _um = um;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Application
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Application.Include(o => o.User).ToListAsync());
        }

        [Authorize(Roles = "Admin,professor")]
        public async Task<IActionResult> List()
        {
            return View(await _context.Application.Include(o => o.User).ToListAsync());
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
        public async Task<IActionResult> Create([Bind("ID,Pursuing,GPA,Department,numberOfHour,avaiableBefore,SemestersCount,PersonalStatement,TransferSchool,LinkedinURL,resumeURL,photoURL,resumePath,photoPath")] Application application)
        {
            ModelState.Remove("User");
            application.User = await _um.GetUserAsync(User);
            /*            if (application.GPA != 0)
                        {
                            return Json(new { Error = "You already created an application" });
                        }*/
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (application.resumeURL != null)
                {
                    string filename = application.resumeURL.FileName.Substring(application.resumeURL.FileName.LastIndexOf("."));
                    if (filename != ".pdf")
                    {
                        ModelState.AddModelError("File", "The file is not jpg.");

                        return Json(new { foo = "The file is not jpg" });
                    }
                    if (application.resumeURL.Length > 2097152)
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                    else
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + application.resumeURL.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        application.resumeURL.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }

                string uniqueFileName2 = null;
                if (application.photoURL != null)
                {
                    string filename = application.photoURL.FileName.Substring(application.photoURL.FileName.LastIndexOf("."));
                    if (filename != ".jpg" && filename != ".jpeg" && filename != ".png" && filename != ".gif")
                    {
                        ModelState.AddModelError("File", "The file is not jpg.");

                        return Json(new { foo = "The file is not jpg" });
                    }


                    if (application.photoURL.Length > 2097152)
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                    else
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                        uniqueFileName2 = Guid.NewGuid().ToString() + "_" + application.photoURL.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName2);
                        application.photoURL.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }

                application.resumePath = uniqueFileName;
                application.photoPath = uniqueFileName2;

                _context.Add(application);
                await _context.SaveChangesAsync();
                return Redirect("/Application/Details/" + application.ID);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Pursuing,GPA,Department,numberOfHour,avaiableBefore,SemestersCount,PersonalStatement,TransferSchool,LinkedinURL,resumeURL,photoURL,resumePath,photoPath")] Application application)
        {
            if (id != application.ID)
            {
                return NotFound();
            }
            ModelState.Remove("User");
            application.User = await _um.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = null;
                    if (application.resumeURL != null)
                    {
                        string filename = application.resumeURL.FileName.Substring(application.resumeURL.FileName.LastIndexOf("."));
                        if (filename != ".pdf")
                        {
                            ModelState.AddModelError("File", "The file is not jpg.");

                            return Json(new { foo = "The file is not jpg" });
                        }
                        if (application.resumeURL.Length > 2097152)
                        {
                            ModelState.AddModelError("File", "The file is too large.");
                        }
                        else
                        {
                            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                            uniqueFileName = Guid.NewGuid().ToString() + "_" + application.resumeURL.FileName;
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                            application.resumeURL.CopyTo(new FileStream(filePath, FileMode.Create));
                        }
                    }

                    string uniqueFileName2 = null;
                    if (application.photoURL != null)
                    {
                        string filename = application.photoURL.FileName.Substring(application.photoURL.FileName.LastIndexOf("."));
                        if (filename != ".jpg" && filename != ".jpeg" && filename != ".png" && filename != ".gif")
                        {
                            ModelState.AddModelError("File", "The file is not jpg.");

                            return Json(new { foo = "The file is not jpg" });
                        }


                        if (application.photoURL.Length > 2097152)
                        {
                            ModelState.AddModelError("File", "The file is too large.");
                        }
                        else
                        {
                            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                            uniqueFileName2 = Guid.NewGuid().ToString() + "_" + application.photoURL.FileName;
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName2);
                            application.photoURL.CopyTo(new FileStream(filePath, FileMode.Create));
                        }
                    }

                    application.resumePath = uniqueFileName;
                    application.photoPath = uniqueFileName2;

                    _context.Update(application);
                    await _context.SaveChangesAsync();
                    return Redirect("/Application/Details/" + id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(application.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(application);
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
