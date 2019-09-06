using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Services.RolesServices
{
    public interface IRoleRepository
    {
        /*
         * You can use this interface to define all methods that will interatct
         * with the DbContext, check them in the implementation class.
         */

        Task<IEnumerable<Entities.Role>> GetRolesAsync();
        Task<Entities.Role> GetRoleAsync(Guid id);
        Task AddRoleAsync(Entities.Role role);
        void UpdateRole(Entities.Role role);
        void RemoveRole(Guid id);
    }
}
