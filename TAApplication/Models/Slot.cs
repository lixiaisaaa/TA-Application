using TAApplication.Areas.Data;

namespace TAApplication.Models
{
    public class Slot
    {
        public int ID { get; set; }
        public TAUser User { get; set; } = null!;
        public string? time { get; set; }
        public bool IsActive { get; set; }
        public string? timeArray { get; set; }
    }
}
