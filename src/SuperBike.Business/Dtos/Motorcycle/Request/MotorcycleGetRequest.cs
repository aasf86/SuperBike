using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBike.Business.Dtos.Motorcycle.Request
{
    public class MotorcycleGetRequest : RequestBase<MotorcycleGet>
    {
        public MotorcycleGetRequest(MotorcycleGet data) : base(data) { }
    }
}
