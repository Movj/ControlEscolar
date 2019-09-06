using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Services.RolesServices
{
    public interface IRoleService
    {
        /*
         * This interface will define all methods that will be used to handle 
         * roles information and request
         */

        Task<IEnumerable<Entities.Role>> GetRolesAsync();
        Task<RoleResponse> SaveAsync(Entities.Role role);
        Task<RoleResponse> UpdateAsync(Entities.Role role);
        Task<RoleResponse> DeleteAsync(Guid id);
        Task<Entities.Role> FindRoleAsync(Guid id);
    }
}
