using CE.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Services
{
    public abstract class BaseRepository
    {
        protected readonly CEDatabaseContext _context;

        public BaseRepository(CEDatabaseContext context)
        {
            _context = context;
        }
    }
}
