using SuperBike.Business.UseCases.Validators;
using System.ComponentModel.DataAnnotations;

namespace SuperBike.Business.UseCases
{
    public abstract class UseCaseBase
    {     
        public virtual ResultValidation Validate(object entity)
        {
            if (entity == null) return new ResultValidation(Enumerable.Empty<ValidationResult>().ToList());

            var valid = new ValidationContext(entity);
            var valids = new List<ValidationResult>();
            var result = Validator.TryValidateObject(entity, valid, valids, true);

            return new ResultValidation(valids);
        }
    }
}
