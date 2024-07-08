using System.Runtime.Serialization;

namespace SuperBike.Business.Dtos
{
    public class ResponseBase<T>
    {
        public T Data { get; set; } = Activator.CreateInstance<T>();
        public bool IsSuccess => !Errors.Any(); 
        public List<string?> Errors { get; set; } = new List<string?>();

#if !DEBUG
        [IgnoreDataMember]
#endif
        public Exception? Exception { get; set; }
    }
}
