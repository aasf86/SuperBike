using SuperBike.Business.UseCases.Validators;

namespace SuperBike.Business.Contracts
{
    public interface IValidators
    {
        ResultValidation Validate(object entity);
    }
}
