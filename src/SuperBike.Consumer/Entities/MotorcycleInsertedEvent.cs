using SuperBike.Domain.Events.Motorcycle;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperBike.Consumer.Entities
{
    [Table("events_motorcycle_inserted")]
    public class MotorcycleInsertedEvent : MotorcycleInserted
    {        
        public new int Id { get; set; }
        public string Queue { get; set; } = "";
        public string MQ { get; set; } = "";
        public new int MotorcycleId { get { return base.Id; } set { base.Id = value; } }
        public DateTime Inserted { get; set; } = DateTime.Now;
    }
}
