namespace SuperBike.Domain.Events
{
    public class Event
    {
        public Guid RequestId { get; set; } = Guid.NewGuid();
        public string FromApp { get; set; } = "";
        public string Version { get; set; } = "";
        public DateTime WhenEvent { get; set; } = DateTime.Now;
    }
}
