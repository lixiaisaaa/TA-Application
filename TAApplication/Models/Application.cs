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
        [Required]
        public Pursuing Pursuing { get; set; }

        [Required]
        [Display(Name = "Grade Point Average",
            Description ="Please give a gpa between 0 and 4",
            ShortName = "GPA")]
        [Range(0,5)]
        public float GPA { get; set; }

        /*        public int UserID { get; set; }*/
        [Required]
        [Display(Prompt = "e.g., CS, CE, ME")]
        public string? Department { get; set; }

        [Required]
        [Range(5, 20)]
        public int numberOfHour { get; set; }
        [Required]
        public Boolean avaiableBefore { get; set; }
        [Required]
        [Display(Prompt = "0")]
        [Range(0, Int32.MaxValue)]
        public int SemestersCount { get; set;}
        [StringLength(50000, ErrorMessage = " Personal statement can't be more than 50000 characters.")]
        public string? PersonalStatement { get; set; }

        public string? TransferSchool { get; set; }
        
        [Url]
        public string? LinkedinURL { get; set; }
        public TAUser User { get; set; } = null!;

        
        [NotMapped]
        public IFormFile? resume { get; set; }
        
        [NotMapped]
        public IFormFile? photo { get; set; }

        public string? resumePath { get; set; }

        public string? photoPath { get; set; }
    }
}
