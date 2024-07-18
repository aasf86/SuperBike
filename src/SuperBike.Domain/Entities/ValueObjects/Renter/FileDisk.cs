namespace SuperBike.Domain.Entities.ValueObjects.Renter
{
    public class FileDisk
    {
        public int Id { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public DateTime Inserted { get; set; } = DateTime.Now;
        public string Key { get; set; } = "";
        public string FileName { get; set; } = "";
        public long Length { get; set; }
        public string ContentType { get; set; } = "";
        public string LocalPath { get; set; } = "";
    }
}
