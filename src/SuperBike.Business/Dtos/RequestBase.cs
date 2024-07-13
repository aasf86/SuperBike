namespace SuperBike.Business.Dtos
{
    public class RequestBase<T>
    {
        public RequestBase(T data)
        {
            Data = data;
        }
        public Guid RequestId { get; set; } = Guid.NewGuid();
        public T Data { get; set; } = Activator.CreateInstance<T>();
        public string From { get; set; } = "";
        public string Version { get; set; } = "";
    }
}
