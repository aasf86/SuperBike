namespace SuperBike.Business.Dtos
{
    public class ResponseBase<T>
    {
        public ResponseBase(T data)
        {
            Data = data;
        }

        public ResponseBase(T data, List<string?> erros, Guid? requestId)
        {
            Data = data;
            Errors = erros;
            requestId = requestId ?? Guid.Empty;
        }

        public Guid RequestId { get; set; } = Guid.Empty;
        public T Data { get; set; } = Activator.CreateInstance<T>();
        public bool IsSuccess => !Errors.Any(); 
        public List<string?> Errors { get; set; } = new List<string?>();
    }
}
