namespace SuperBike.Domain.Events.Motorcycle
{
    public class MotorcycleInserted : Event
    {
        public int Id { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public int Year { get; set; }
        public string Model { get; set; } = "";
        public string Plate { get; set; } = "";
    }
}
