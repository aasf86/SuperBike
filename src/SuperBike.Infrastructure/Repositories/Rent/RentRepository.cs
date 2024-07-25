using Dapper;
using SuperBike.Domain.Contracts.Repositories.Rent;
using SuperBike.Domain.Entities.ValueObjects.Rent;
using static Dapper.SqlMapper;
using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Infrastructure.Repositories.Rent
{
    internal class RentRepository : RepositoryBase<Entity.Rent>, IRentRepository
    {
        public async Task<List<RentalPlan>> GetAllPlans()
        {
            var sql = Helpers.StrSql.CreateSqlSelect<RentalPlan>("0=0");
            return (await DbTransaction.Connection.QueryAsync<RentalPlan>(sql)).ToList();
        }

        public async Task<bool> MotorcycleAvailable(int motorcycleId)
        {
            var sql = "select count(1) from rent where motorcycleId = @motorcycleId";
            var flag = (await DbTransaction.Connection.ExecuteScalarAsync<int>(sql, new { motorcycleId })) == 0;
            return flag;
        }

        public override async Task<List<Entity.Rent>> GetAll(dynamic filter)
        {
            var sql = @"
                select 
                    r.*,
                    rp.*
                from 
                    rent r,
                    renter re,
                    rentalplan rp
                where r.renterid = re.id
                and r.rentalplanid = rp.id
                and re.userid = @UserId
            ";

            var listResult = (await DbTransaction.Connection.QueryAsync<Entity.Rent, RentalPlan, Entity.Rent>(
                sql,                 
                (itemRent, rentPlan) => 
                {
                    itemRent.SetRentalPlan(rentPlan);
                    return itemRent;
                },
                filter as object)).ToList();

            return listResult;
        }
    }
}
