using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using TAApplication.Areas.Data;
using TAApplication.Models;

namespace TAApplication.ViewModels
{
    public class ApplicationCreateViewModel : ModificationTracking
    {
        public int ID { get; set; }
        [Required]
        public Pursuing Pursuing { get; set; }

        [Required]
        [Display(Name = "Grade Point Average",
            Description = "Please give a gpa between 0 and 4",
            ShortName = "GPA")]
        [Range(0, 5)]
        public float GPA { get; set; }

        /*        public int UserID { get; set; }*/
        [Required]
        public string? Department { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]
        public int numberOfHour { get; set; }
        [Required]
        public Boolean avaiableBefore { get; set; }

        [Display(Prompt = "0")]
        public int SemestersCount { get; set; }

        public TAUser User { get; set; } = null!;


        public IFormFile? resume { get; set; }

        [Required]
        public IFormFile? photo { get; set; }

    }
}
