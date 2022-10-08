using System.ComponentModel.DataAnnotations;
using TAApplication.Areas.Data;

namespace TAApplication.Models
{
    public enum Pursuing
    {
        BS, MS, PhD
    }
    public class Application : ModificationTracking
    {

        public int ID { get; set;}

        public Pursuing Pursuing { get; set; }

        [Required]
        [Display(Name = "Grade Point Average",
            Description ="Please give a gpa between 0 and 4",
            ShortName = "GPA")]
        [Range(0,5)]
        public float GPA { get; set; }

/*        public int UserID { get; set; }*/

        public string? Department { get; set; }

        public int numberOfHour { get; set; }

        public Boolean avaiableBefore { get; set; }

        public int SemestersCount { get; set;}

        public TAUser User { get; set; } = null!;
    }
}
