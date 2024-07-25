using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SuperBike.Domain.Entities.ValueObjects.Rent
{
    public class RentalPlan : EntityBase
    {
        private readonly decimal _percentageOfDailyNotEffectived;
        private readonly decimal _percentageValuePerDay;
        public RentalPlan() { }

        public RentalPlan(
            int days,
            decimal valuePerDay,
            decimal percentageOfDailyNotEffectived,
            decimal valuePerDayExceeded)
        {
            Days = days;
            ValuePerDay = valuePerDay;
            //_percentageOfDailyNotEffectived = percentageOfDailyNotEffectived;
            PercentageOfDailyNotEffectived = percentageOfDailyNotEffectived;
            ValuePerDayExceeded = valuePerDayExceeded;
        }

        public int Days { get; private set; }
        public decimal ValuePerDay { get; private set; }
        public decimal PercentageOfDailyNotEffectived { get; private set; }
        /*{ 
            get {  return _percentageOfDailyNotEffectived == 0 ? 1 : _percentageOfDailyNotEffectived; }            
        }*/
        public decimal ValuePerDayExceeded { get; private set; }
        public decimal TotalValue => Days * ValuePerDay;
        public decimal PercentageValuePerDay => PercentageOfDailyNotEffectived * ValuePerDay;
        public decimal TotalValueOfDaysNotEffetived(int daysNotEffetived)
        {
            if (PercentageOfDailyNotEffectived > 0.0m) return ((Days - daysNotEffetived) * PercentageValuePerDay) + (daysNotEffetived * ValuePerDay);
            return (daysNotEffetived * ValuePerDay);
        }
        public decimal TotalValueOfDaysExceeded(int daysExceeded) => ((daysExceeded - Days) * ValuePerDayExceeded) + TotalValue;
    }
}
