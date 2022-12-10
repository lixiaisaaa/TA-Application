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
* This c# for course controller.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using TAApplication.Data;
using TAApplication.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using NuGet.Protocol;
using System.Text.Json;
using SendGrid.Helpers.Mail;
using System.Xml.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Web.Helpers;
using System.Text.Json.Nodes;
using System.Runtime.Serialization.Json;
using System.Text;
using OpenQA.Selenium;
using System.Collections;

namespace TAApplication.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,professor")]
        // GET: Courses
        public async Task<IActionResult> Index()
        {
              return View(await _context.Course.ToListAsync());
        }

        // GET: Courses/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }


        // GET: Courses/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Semester,Year,titleOftheCourse,Department,CourseNumber,Section,Description,profID,profName,name,TimeAndDays,Location,creditHours,Enrollment,note")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Semester,Year,titleOftheCourse,Department,CourseNumber,Section,Description,profID,profName,name,TimeAndDays,Location,creditHours,Enrollment,note")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            return View(course);
        }
        [Authorize(Roles = "Admin")]
        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Course'  is null.");
            }
            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                _context.Course.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<ActionResult> UploadCourses([FromBody] List<string[]> courses)
        {

            foreach (string[] course in courses)
            {
                Course c = new Course();
                c.creditHours = Int32.Parse(course[0]);
                c.titleOftheCourse = course[1];
                c.CourseNumber = Int32.Parse(course[2]);
                c.Section = course[3];
                c.Semester = course[4];
                c.Year = Int32.Parse(course[5]);
                c.profID = course[6];
                c.Enrollment = Int32.Parse(course[7]);
                c.Department = course[8];
                _context.Add(c);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        private bool CourseExists(int id)
        {
          return _context.Course.Any(e => e.Id == id);
        }
    }
}
