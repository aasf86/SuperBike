using SuperBike.Domain.Contracts.UseCases;

namespace SuperBike.Business.Dtos
{
    public class RequestBase<T> : Domain.Contracts.UseCases.RequestBase<T>
    {
        public RequestBase(T data)
        {
            Data = data;
        }
        public T Data { get; set; } = Activator.CreateInstance<T>();
        public string From { get; set; } = "";
        public string Version { get; set; } = "";
    }
}
