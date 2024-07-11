using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBike.Business.Dtos.Motorcycle.Request
{
    public class MotorcycleInsertRequest : RequestBase<MotorcycleInsert>
    {
        public MotorcycleInsertRequest(MotorcycleInsert data) : base(data) { }
    }
}
