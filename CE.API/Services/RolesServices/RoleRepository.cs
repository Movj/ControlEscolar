using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CE.API.Entities;

namespace CE.API.Services.RolesServices
{
    public class RoleRepository : BaseRepository, IRoleRepository, IDisposable
    {
        public RoleRepository(CEDatabaseContext context) : base(context)
        { }

        public Task AddRoleAsync(Role role)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<Role> GetRoleAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Role>> GetRolesAsync()
        {
            throw new NotImplementedException();
        }

        public void RemoveRole(Guid id)
        {
            throw new NotImplementedException();
        }

        public void UpdateRole(Role role)
        {
            throw new NotImplementedException();
        }
    }
}
