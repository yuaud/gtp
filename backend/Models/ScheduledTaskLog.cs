namespace backend.Models
{
    public class ScheduledTaskLog
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public DateTime LastRunUtc { get; set; }
    }
}
