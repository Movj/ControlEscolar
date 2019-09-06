using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Services
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
