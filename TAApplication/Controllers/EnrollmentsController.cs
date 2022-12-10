/**
* Author:    Xia Li
* Partner:   Wenlin Li
* Date:      12/09/2022
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
* This c# for enrollment controller.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TAApplication.Data;
using TAApplication.Models;
using ZendeskApi_v2.Models.Shared;

namespace TAApplication.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> EnrollmentTrend()
        {
            return View(await _context.Enrollment.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> GetEnrollmentData(string startDate, string endDate, string dept, string courseNumber)
        {
            Course? c = await _context.Course.FirstOrDefaultAsync(o => o.Department == dept && o.CourseNumber == Int32.Parse(courseNumber));
            string[] s = startDate.Split("/");
            string queryStartDate = "";
            int dateNumber = Int32.Parse(s[1]);          
            if (s[0] == "11") {
                queryStartDate += "Nov";
            }

            string queryEndDate = "";
            string[] s2 = endDate.Split("/");
            int dateNumber2 = Int32.Parse (s2[1]);
            if (s2[0] == "11") {
                queryEndDate += "Nov";
            }
            queryEndDate += s2[1];
            List<string> date = new List<string>();
            List<int>? enrollment = new List<int>();
            for (int i = 0; i <= dateNumber2 - dateNumber; i++) {
                int dateNum = dateNumber + i;
                string queryDate = queryStartDate + " " + dateNum.ToString();

                Enrollment? e = await _context.Enrollment.FirstOrDefaultAsync(o => o.Date == queryDate && o.CourseStr == dept + " " + courseNumber);
                if (e is not null)
                {
                    if (e.Date is not null)
                    {
                        date.Add(e.Date);
                        enrollment.Add(e.enrollment);
                    }
                }
            }
            sendBackData send = new sendBackData();
            send.date = date;
            send.enrollment = enrollment;

            return Json(send);
        }


        public class sendBackData
        {
            public List<string>? date { get; set; }
            public List<int>? enrollment { get; set; }
        }
    }
}
