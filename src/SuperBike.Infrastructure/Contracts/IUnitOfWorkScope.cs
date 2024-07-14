namespace SuperBike.Infrastructure.Contracts
{
    public interface IUnitOfWorkScope
    {
        Task UnitOfWorkExecute(Func<Task> execute);
    }
}
