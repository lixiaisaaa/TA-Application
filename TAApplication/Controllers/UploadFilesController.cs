
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
* This css file controller for admin.
*/
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
