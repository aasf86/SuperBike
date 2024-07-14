namespace SuperBike.Domain.Entities
{
    public class EntityBase
    {
        public EntityBase() { }
        public EntityBase(int id) => Id = id;
        public EntityBase(Guid guid) => Guid = guid;
        public int Id { get; set; }
        public Guid Guid { get; private set; } = Guid.NewGuid();
        public DateTime Inserted { get; private set; } = DateTime.Now;
        public DateTime Updated { get; private set; } = DateTime.Now;
    }
}
