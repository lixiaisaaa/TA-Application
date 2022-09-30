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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TAApplication.Areas.Data
{
    /*[Index(nameof(Unid), IsUnique = true)]*/
    public class TAUser : IdentityUser
    {   
        public string? Name { get; set; }
        public string? Unid { get; set; }

        public string? RefferedTo { get; set;}
    }
}
