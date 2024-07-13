using System.Dynamic;

namespace SuperBike.Business.Dtos.Motorcycle
{
    public class MotorcycleGet
    {
        public dynamic Filter { get; private set; } = new ExpandoObject();
    }
}
