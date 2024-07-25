namespace SuperBike.Domain.Entities
{
    public class EntityBase
    {
        public EntityBase() { }
        public int Id { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public DateTime Inserted { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
    }
}
