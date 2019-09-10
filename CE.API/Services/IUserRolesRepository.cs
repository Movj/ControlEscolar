using CE.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Services
{
    public interface IUserRolesRepository
    {
        /*
         * You can use this interface to define all methods that will interatct
         * with the DbContext, check them in the implementation class.
         */

        IQueryable<Entities.Usuario> GetUsersList();
        Task AddAsync(Entities.Usuario user);
        void Update(Entities.Usuario user);
        Task<Entities.Usuario> FindUserAsync(Guid id);
        void Remove(Entities.Usuario user);
    }
}
