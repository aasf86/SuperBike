namespace SuperBike.Business.Dtos.Motorcycle.Response
{
    public class MotorcycleInsertResponse : ResponseBase<MotorcycleInsert>
    {
        public MotorcycleInsertResponse(MotorcycleInsert data) : base(data) { }
        public MotorcycleInsertResponse(MotorcycleInsert data, List<string?> erros, Guid requestId) : base(data, erros, requestId) { }
    }
}
