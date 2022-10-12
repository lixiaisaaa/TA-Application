using Microsoft.AspNetCore.Mvc;

namespace TAApplication.Controllers
{
    public class UploadFilesController : Controller
    {
        public IActionResult Index() => View();
        
        public IActionResult SingleFile(IFormFile file)
        {

            return RedirectToAction("Index");
        }



    }
}
