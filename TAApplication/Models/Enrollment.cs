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
* This c# for slot model.
*/

namespace TAApplication.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public Course? Course { get; set; }  
        public string? Date { get; set; }
        public int enrollment { get; set; }
        public string? CourseStr { get; set; }
    }
}
