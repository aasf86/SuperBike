using SuperBike.Domain.Entities.ValueObjects.Rent;
using System.ComponentModel.DataAnnotations.Schema;
using static SuperBike.Domain.Entities.Rules.Rent;

namespace SuperBike.Domain.Entities
{
    public partial class Rent : EntityBase
    {
        public Rent() { }

        public Rent(
            int motorcycleId, 
            int renterId, 
            RentalPlan rentalPlan, 
            int rentalDays)
        {
            if (motorcycleId <= 0) throw new InvalidDataException(RentMsgDialog.RequiredMotorcycle);
            if (renterId <= 0) throw new InvalidDataException(RentMsgDialog.RequiredRenter);
            if (rentalPlan is null) throw new InvalidDataException(RentMsgDialog.RequiredRentalPlan);
            if (rentalDays <= 0) throw new InvalidDataException(RentMsgDialog.InvalidRentalDays);

            MotorcyleId = motorcycleId;
            RenterId = renterId;
            RentalPlan = rentalPlan;
            RentalDays = rentalDays;

            InitialDate = DateTime.Now;
            var endDate = InitialDate.AddDays(rentalPlan.Days);
            var endPredictionDate = InitialDate.AddDays(rentalDays);
            EndDate = new DateTime(endDate.Year, endDate.Month, endDate.Day);
            EndPredictionDate = new DateTime(endPredictionDate.Year, endPredictionDate.Month, endPredictionDate.Day);
        }

        public int RentalplanId { get { return RentalPlan.Id; } private set { RentalPlan.Id = value; } }
        public int MotorcyleId { get; private set; }
        public int RenterId { get; private set; }
        public int RentalDays { get; set; }
        public DateTime InitialDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public DateTime EndPredictionDate { get; private set; }

        [NotMapped]
        public int TotalDaysOfRent => (DateTime.Now - InitialDate).Days;

        [NotMapped]
        public RentalPlan RentalPlan { get; private set; }

        public decimal TotalRentalValue()
        {
            var totalDays = RentalDays;

            if (TotalDaysOfRent > RentalDays) totalDays = TotalDaysOfRent;            

            if (totalDays == RentalPlan.Days) return RentalPlan.TotalValue;
            if (totalDays < RentalPlan.Days) return RentalPlan.TotalValueOfDaysNotEffetived(totalDays);
            if (totalDays > RentalPlan.Days) return RentalPlan.TotalValueOfDaysExceeded(totalDays);

            return 0;
        }
    }
}
