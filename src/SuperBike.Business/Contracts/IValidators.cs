using SuperBike.Business.UseCases.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBike.Business.Contracts
{
    public interface IValidators
    {
        ResultValidation Validate(object entity);
    }
}
