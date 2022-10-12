using EllipticCurve.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Range(0,Int32.MaxValue)]
        public int numberOfHour { get; set; }

        public Boolean avaiableBefore { get; set; }

        [Display(Prompt = "0")]
        public int SemestersCount { get; set;}

        public TAUser User { get; set; } = null!;

        [NotMapped]
        public IFormFile? resume { get; set; }
        [NotMapped]
        public IFormFile? photo { get; set; }

        public string? resumePath { get; set; }

        public string? photoPath { get; set; }
    }
}
