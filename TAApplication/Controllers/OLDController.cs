using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TAApplication.Areas.Data;
using TAApplication.Models;

namespace TAApplication.Controllers
{
    [Authorize]
    public class OLDController : Controller
    {
        private readonly ILogger<OLDController> _logger;
        UserManager<TAUser> _um;
        public OLDController(ILogger<OLDController> logger, UserManager<TAUser> um)
        {
            _logger = logger;
            _um = um;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
        public IActionResult ApplicantList()
        {
            return View();
        }

        
        public IActionResult ApplicantCreate()
        {
            return View();
        }

        
        public IActionResult ApplicantDetails()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    
}
}
