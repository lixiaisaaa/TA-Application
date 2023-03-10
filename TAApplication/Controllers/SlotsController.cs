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
* This c# for slot controller.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TAApplication.Areas.Data;
using TAApplication.Data;
using TAApplication.Models;
using static System.Net.Mime.MediaTypeNames;
using Application = TAApplication.Models.Application;

namespace TAApplication.Controllers
{
    public class SlotsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SlotsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Slots
        public async Task<IActionResult> Index()
        {
            return View(await _context.Slot.ToListAsync());
        }
        [HttpPost]
        public async Task<ActionResult> SetSchedule(Sample sample)
        {
            Slot? sl = await _context.Slot.FirstOrDefaultAsync(o => o.User.Unid == sample.Unid);
            if (sl is null)
            {
                Slot slot = new Slot();
                TAUser? user = await _context.Users.FirstOrDefaultAsync(o => o.Unid == sample.Unid);
                slot.User = user;
                slot.time = sample.time;
                slot.timeArray = sample.timeArray;
                slot.IsActive = sample.IsActive;
                _context.Add(slot);
            }
            else {
                sl.time = sample.time;  
                sl.timeArray = sample.timeArray;
                sl.IsActive = sample.IsActive;
                _context.Update(sl);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<ActionResult> GetSchedule(Sample sample) {
            Slot? slot = await _context.Slot.FirstOrDefaultAsync(o => o.User.Unid == sample.Unid);
            List<int[]>? timeArray = new List<int[]>();
            sendBackData data = new sendBackData();
            if (slot is not null)
            {
                if (slot.timeArray is not null)
                {
                    var timeList = slot.timeArray;
                    string[] time = timeList.Split('#');
                    foreach (var timeItem in time)
                    {
                        if (timeItem.Length > 0)
                        {
                            string[] t = timeItem.Split(' ');
                            int[] array = new int[2];
                            array[0] = int.Parse(t[0]);
                            array[1] = int.Parse(t[1]);
                            timeArray.Add(array);
                        }
                    }
                    data.timeArray = timeArray;
                }
                if (slot.time is not null) {
                    data.time = slot.time;
                }
            }
            return Json(data);
        }
    }
    public class Sample
    {
        public string? Unid { get; set; }
        public string? time { get; set; }
        public bool IsActive { get; set; }
        public string? timeArray { get; set; }
    }

    public class sendBackData
    {
        public string? time { get; set; }
        public List<int[]>? timeArray { get; set; }
    }

}
