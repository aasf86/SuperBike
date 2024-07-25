using System.ComponentModel.DataAnnotations.Schema;

namespace SuperBike.Domain.Entities.ValueObjects.Rent
{
    public class RentalPlan : EntityBase
    {
        public RentalPlan() { }

        public RentalPlan(
            int days,
            decimal valuePerDay,
            decimal percentageOfDailyNotEffectived,
            decimal valuePerDayExceeded)
        {
            Days = days;
            ValuePerDay = valuePerDay;            
            PercentageOfDailyNotEffectived = percentageOfDailyNotEffectived;
            ValuePerDayExceeded = valuePerDayExceeded;
        }

        public int Days { get; private set; }
        public decimal ValuePerDay { get; private set; }
        public decimal PercentageOfDailyNotEffectived { get; private set; }
        public decimal ValuePerDayExceeded { get; private set; }

        [NotMapped]
        public decimal TotalValue => Days * ValuePerDay;

        [NotMapped]
        public decimal PercentageValuePerDay => PercentageOfDailyNotEffectived * ValuePerDay;

        public decimal TotalValueOfDaysNotEffetived(int daysNotEffetived)
        {
            if (PercentageOfDailyNotEffectived > 0.0m) return ((Days - daysNotEffetived) * PercentageValuePerDay) + (daysNotEffetived * ValuePerDay);
            return (daysNotEffetived * ValuePerDay);
        }
        public decimal TotalValueOfDaysExceeded(int daysExceeded) => ((daysExceeded - Days) * ValuePerDayExceeded) + TotalValue;
    }
}
