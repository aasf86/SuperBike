using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBike.Infrastructure.Contracts
{
    public interface IUnitOfWorkScope
    {
        Task UnitOfWorkExecute(Func<Task> execute);
    }
}
