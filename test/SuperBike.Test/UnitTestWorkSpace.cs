using SuperBike.Business.Dtos.Renter;
using static Dapper.SqlMapper;
using SuperBike.Business.UseCases.Validators;
using System.ComponentModel.DataAnnotations;
using SuperBike.Domain.Entities;

namespace SuperBike.Test
{
    public class UnitTestWorkSpace
    {
        [Fact]
        public void TestDomain()
        {
            Renter renterEntity = new Renter();

            var renter = new RenterInsert()
            {
                Name = "Test",
                CNH = "00987654321",
                CnpjCpf = "83122753200",
                DateOfBirth = DateTime.Now,
                CNHImg = "Test",
                CNHType = "A"
            };
            
            var valid = new ValidationContext(renter);
            var valids = new List<ValidationResult>();
            var result = Validator.TryValidateObject(renter, valid, valids, true);

            if (result) 
            {
                renterEntity = new Renter(renter.Name, renter.CnpjCpf, renter.DateOfBirth, renter.CNH, renter.CNHType, renter.CNHImg);
            }

            Assert.True(renter.Name == renterEntity.Name);
            Assert.True(renter.CNH == renterEntity.CNH);
            Assert.True(renter.CnpjCpf == renterEntity.CnpjCpf);
            Assert.True(renter.DateOfBirth == renterEntity.DateOfBirth);
            Assert.True(renter.CNHImg == renterEntity.CNHImg);
            Assert.True(renter.CNHType == renterEntity.CNHType);

        }
    }
}