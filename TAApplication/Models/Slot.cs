using TAApplication.Areas.Data;

namespace TAApplication.Models
{
    public class Slot
    {
        public int ID { get; set; }
        public TAUser User { get; set; } = null!;
        public string? time { get; set; }
        public bool IsActive { get; set; }
        public List<position>? timeArray { get; set; }
    }

    public class position
{
        public int ID { get; set; }
        public int x { get; set; }
    public int y { get; set; }
}
}
