namespace SuperBike.Business.Dtos.Motorcycle.Response
{
    public class MotorcycleGetResponse : ResponseBase<MotorcycleGet>
    {
        public MotorcycleGetResponse(MotorcycleGet data) : base(data) { }
        public MotorcycleGetResponse(MotorcycleGet data, List<string?> erros, Guid requestId) : base(data, erros, requestId) { }
    }
}
