/**
* Author:    Xia Li
* Partner:   Wenlin Li
* Date:      09/29/2022
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
* This css file controller for home.
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TAApplication.Areas.Data;
using TAApplication.Models;

namespace TAApplication.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        UserManager<TAUser> _um;
        public HomeController(ILogger<HomeController> logger, UserManager<TAUser> um)
        {
            _logger = logger;
            _um = um;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

/*        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Admin,professor")]
        public IActionResult ApplicantList() { 
            return View();
        }

        [Authorize(Roles = "Applicant")]
        public IActionResult ApplicantCreate() {
            return View();
        }

        [Authorize(Roles = "Admin,professor,Applicant")]
        public IActionResult ApplicantDetails()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
    }
}