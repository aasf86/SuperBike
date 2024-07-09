namespace SuperBike.Business.Dtos
{
    public class ResponseBase<T>
    {
        public ResponseBase(T data)
        {
            Data = data;
        }

        public ResponseBase(T data, List<string?> erros)
        {
            Data = data;
            Errors = erros;
        }

        public T Data { get; set; } = Activator.CreateInstance<T>();
        public bool IsSuccess => !Errors.Any(); 
        public List<string?> Errors { get; set; } = new List<string?>();
    }
}
