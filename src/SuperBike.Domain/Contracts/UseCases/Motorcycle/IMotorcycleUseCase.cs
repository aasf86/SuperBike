namespace SuperBike.Domain.Contracts.UseCases.Motorcycle
{
    public interface IMotorcycleUseCase
    /*<
        RequestInsert, ResponseInsert,
        RequestUpdate, ResponseUpdate,
        RequestDelete, ResponseDelete,
        RequestGetGuid, ResponseGetGuid,
        RequestGetAll, ResponseGetAll
    >
    */
    {
        Task<ResponseBase> Insert<T>(RequestBase request);
        /*Task<ResponseUpdate> Update(RequestUpdate request);
        Task<ResponseDelete> Delete(RequestDelete request);
        Task<ResponseGetGuid> GetByGuid(RequestGetGuid request);
        Task<ResponseGetAll> GetAll(RequestGetAll request);*/
    }
}
