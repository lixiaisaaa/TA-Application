using System.ComponentModel.DataAnnotations;

namespace TAApplication.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string? Semester { get; set; }
        public int? Year { get; set; }
        [Display(Prompt = "e.g., Web Software")]
        public string? titleOftheCourse { get; set; }
        [Display(Prompt = "e.g., CS, CE, Comp")]
        public string? Department { get; set; }
        [Display(Prompt = "e.g., 2420")]
        public int CourseNumber { get; set; }
        [Display(Prompt = "e.g., 001")]
        public string? Section { get; set; }
        [Display(Prompt = "i.e., the course catalog description")]
        public string? Description { get; set; }
        public int profID { get; set; }
        public string? profName { get; set; }
        [Display(Prompt = "e.g., M/W 3:30-5:00")]
        public string? TimeAndDays { get; set; }
        [Display(Prompt = "e.g., WEB L 104")]
        public string? Location { get; set; }
        [Display(Prompt = "e.g., 3")]
        public int creditHours { get; set; }
        [Display(Prompt = "e.g., 150")]
        public int Enrollment { get; set; }
        [Display(Prompt = "Needs Extra TAs")]
        public string? note { get; set; }
    }
}
