namespace SuperBike.Domain.Events
{
    public class Event
    {
        public Guid RequestId { get; set; } = Guid.NewGuid();
        public string From { get; set; } = "";
        public string Version { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
