using SuperBike.Domain.Entities;
using SuperBike.Domain.Entities.ValueObjects.Rent;

namespace SuperBike.Test
{
    public class UnitTestRent
    {
        private readonly RentalPlan _plan7Days  = new RentalPlan(07, 30m, 0.2m, 50);
        private readonly RentalPlan _plan15Days = new RentalPlan(15, 28m, 0.4m, 50);
        private readonly RentalPlan _plan30Days = new RentalPlan(30, 22m, 0.0m, 50);
        private readonly RentalPlan _plan45Days = new RentalPlan(45, 20m, 0.0m, 50);
        private readonly RentalPlan _plan50Days = new RentalPlan(50, 18m, 0.0m, 50);

        #region Planos de 7 dias

        [Fact]
        public void Rent_For_7_Days()
        {
            var rent7Days = new Rent(10, 1, _plan7Days, 7);
            var totalRentalValue = rent7Days.TotalRentalValue();
            Assert.True(totalRentalValue > 0);
            Assert.True(totalRentalValue == 210m);
        }

        [Fact]
        public void Rent_Per_5_Days_For_Plan_7_Days()
        {
            var rent5Days = new Rent(10, 1, _plan7Days, 5);
            var totalRentalValue = rent5Days.TotalRentalValue();
            Assert.True(totalRentalValue > 0);
            Assert.True(totalRentalValue == 162m);
        }

        [Fact]
        public void Rent_Per_8_Days_For_Plan_7_Days()
        {
            var rent8Days = new Rent(10, 1, _plan7Days, 8);
            var totalRentalValue = rent8Days.TotalRentalValue();
            Assert.True(totalRentalValue > 0);
            Assert.True(totalRentalValue == 260m);
        }

        #endregion

        #region Planos de 15 dias

        [Fact]
        public void Rent_Per_15_Days_For_Plan_15_Days()
        {
            var rent15Days = new Rent(10, 1, _plan15Days, 15);
            var totalRentalValue = rent15Days.TotalRentalValue();
            Assert.True(totalRentalValue > 0);
            Assert.True(totalRentalValue == 420m);
        }

        [Fact]
        public void Rent_Per_10_Days_For_Plan_15_Days()
        {
            var rent10Days = new Rent(10, 1, _plan15Days, 10);
            var totalRentalValue = rent10Days.TotalRentalValue();
            var expectedValue = 10 * 28 + (28 * 0.4m) * 5;//336.0m
            Assert.True(totalRentalValue > 0);
            Assert.True(totalRentalValue == expectedValue);
        }

        [Fact]
        public void Rent_Per_25_Days_For_Plan_15_Days()
        {
            var rent25Days = new Rent(10, 1, _plan15Days, 25);
            var totalRentalValue = rent25Days.TotalRentalValue();
            var expectedValue = 15 * 28 + (50 * 10);//920.0m
            Assert.True(totalRentalValue > 0);
            Assert.True(totalRentalValue == expectedValue);
        }

        #endregion

        #region Planos de 30 dias

        [Fact]
        public void Rent_Per_30_Days_For_Plan_30_Days()
        {
            var rent30Days = new Rent(10, 1, _plan30Days, 30);
            var totalRentalValue = rent30Days.TotalRentalValue();
            Assert.True(totalRentalValue > 0);
            Assert.True(totalRentalValue == 660m);
        }

        [Fact]
        public void Rent_Per_25_Days_For_Plan_30_Days()
        {
            var rent25Days = new Rent(10, 1, _plan30Days, 25);
            var totalRentalValue = rent25Days.TotalRentalValue();
            var expectedValue = 25 * 22 + (0 * 0m) * 5;//550.0m
            Assert.True(totalRentalValue > 0);
            Assert.True(totalRentalValue == expectedValue);
        }

        [Fact]
        public void Rent_Per_35_Days_For_Plan_30_Days()
        {
            var rent35Days = new Rent(10, 1, _plan30Days, 35);
            var totalRentalValue = rent35Days.TotalRentalValue();
            var expectedValue = 30 * 22 + (50 * 5);//910.0m
            Assert.True(totalRentalValue > 0);
            Assert.True(totalRentalValue == expectedValue);
        }

        #endregion
    }
}
