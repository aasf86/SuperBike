namespace SuperBike.Domain.Contracts.Services
{
    public interface IMessageBroker
    {
        Task Publish<T>(T @event, string queue);
    }
}
