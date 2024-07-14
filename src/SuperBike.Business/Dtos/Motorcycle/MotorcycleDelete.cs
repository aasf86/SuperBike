using System.ComponentModel.DataAnnotations;
using static SuperBike.Domain.Entities.Motorcycle;

namespace SuperBike.Business.Dtos.Motorcycle
{
    public class MotorcycleDelete
    {
        [Required(ErrorMessage = MotorcycleMsgDialog.RequiredId)]
        [Range(1, int.MaxValue, ErrorMessage = MotorcycleMsgDialog.RequiredId)]
        public int Id { get; set; }
    }
}
