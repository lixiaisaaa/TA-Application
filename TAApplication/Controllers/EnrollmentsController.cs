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

        // GET: Enrollments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Enrollment.ToListAsync());
        }

        public async Task<IActionResult> EnrollmentTrends()
        {
            return View(await _context.Enrollment.ToListAsync());
        }
        [HttpPost]
        public async Task<ActionResult> GetEnrollmentData(string startDate, string endDate, string dept, string courseNumber)
        {
            string course = dept + " " + courseNumber;
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
                Enrollment? e = await _context.Enrollment.FirstOrDefaultAsync(o => o.Date == queryDate && o.Course == course);
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
