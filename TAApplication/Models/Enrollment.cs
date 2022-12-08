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
