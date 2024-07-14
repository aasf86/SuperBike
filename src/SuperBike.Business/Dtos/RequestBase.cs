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
    public class RequestBase
    {
        public static RequestBase<T> New<T>(T data)
        {
            return new RequestBase<T>(data);
        }

        public static RequestBase<T> New<T>(T data, string from, string version)
        {
            var request = new RequestBase<T>(data);
            request.From = from;
            request.Version = version;
            return request;
        }
    }
}
