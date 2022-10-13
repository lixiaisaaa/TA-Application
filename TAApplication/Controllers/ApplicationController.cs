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
using TAApplication.ViewModels;
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
              return View(await _context.Application.Include(o=>o.User).ToListAsync());
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
        [Authorize]
        public async Task<IActionResult> Create([Bind("ID,Pursuing,GPA,Department,numberOfHour,avaiableBefore,SemestersCount,resume,photo,resumePath,photoPath")] Application application)
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
                if (application.resume != null)
                {
                    string filename = application.resume.FileName.Substring(application.resume.FileName.LastIndexOf("."));
                    if (filename != ".pdf")
                    {
                        ModelState.AddModelError("File", "The file is not jpg.");

                        return Json(new { foo = "The file is not jpg" });
                    }
                    if (application.resume.Length > 2097152)
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                    else {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + application.resume.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        application.resume.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }

                string uniqueFileName2 = null;
                if (application.photo != null)
                {
                    string filename = application.photo.FileName.Substring(application.photo.FileName.LastIndexOf("."));
                    if (filename != ".jpg" && filename != ".jpeg" && filename != ".png" && filename != ".gif")
                    {
                        ModelState.AddModelError("File", "The file is not jpg.");

                        return Json(new { foo = "The file is not jpg" });
                    }


                    if (application.photo.Length > 2097152)
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                    else
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                        uniqueFileName2 = Guid.NewGuid().ToString() + "_" + application.photo.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName2);
                        application.photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }

                application.resumePath = uniqueFileName;
                application.photoPath = uniqueFileName2;

                _context.Add(application);
                await _context.SaveChangesAsync();
                await Details(application.ID);
            }
            return View(application);
        }

/*        public async Task<IActionResult> FileUpload(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size });
        }*/

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Pursuing,GPA,Department,numberOfHour,avaiableBefore,SemestersCount,resume,photo,resumePath,photoPath")] Application application)
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
                if (application.resume != null)
                {
                    string filename = application.resume.FileName.Substring(application.resume.FileName.LastIndexOf("."));
                    if (filename != ".pdf")
                    {
                        ModelState.AddModelError("File", "The file is not jpg.");

                        return Json(new { foo = "The file is not jpg" });
                    }
                    if (application.resume.Length > 2097152)
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                    else {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + application.resume.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        application.resume.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }

                string uniqueFileName2 = null;
                if (application.photo != null)
                {
                    string filename = application.photo.FileName.Substring(application.photo.FileName.LastIndexOf("."));
                    if (filename != ".jpg" && filename != ".jpeg" && filename != ".png" && filename != ".gif")
                    {
                        ModelState.AddModelError("File", "The file is not jpg.");

                        return Json(new { foo = "The file is not jpg" });
                    }


                    if (application.photo.Length > 2097152)
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                    else
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                        uniqueFileName2 = Guid.NewGuid().ToString() + "_" + application.photo.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName2);
                        application.photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }

                application.resumePath = uniqueFileName;
                application.photoPath = uniqueFileName2;

                _context.Update(application);
                await _context.SaveChangesAsync();
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
