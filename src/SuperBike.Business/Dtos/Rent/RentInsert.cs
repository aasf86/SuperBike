using System.ComponentModel.DataAnnotations;
using static SuperBike.Domain.Entities.Rules.Rent;

namespace SuperBike.Business.Dtos.Rent
{
    public class RentInsert
    {

        [Required(ErrorMessage = RentMsgDialog.RequiredRentalPlan)]
        [Range(RentRule.DaysMinimal, int.MaxValue, ErrorMessage = RentMsgDialog.RequiredRentalPlan)]
        public int RentalplanId { get; set; }

        [Required(ErrorMessage = RentMsgDialog.RequiredMotorcycle)]
        [Range(RentRule.DaysMinimal, int.MaxValue, ErrorMessage = RentMsgDialog.RequiredMotorcycle)]
        public int MotorcyleId { get; set; }

        [Required(ErrorMessage = RentMsgDialog.RequiredRenter)]
        [Range(RentRule.DaysMinimal, int.MaxValue, ErrorMessage = RentMsgDialog.RequiredRenter)]
        public int RenterId { get; set; }

        [Required(ErrorMessage = RentMsgDialog.InvalidRentalDays)]
        [Range(RentRule.DaysMinimal, int.MaxValue, ErrorMessage = RentMsgDialog.InvalidRentalDays)]
        public int RentalDays { get; set; }

        public string UserId { get; set; }
    }
}
