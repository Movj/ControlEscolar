using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CE.API.Entities;
using Microsoft.EntityFrameworkCore;

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

        public Task<Role> GetRoleAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Entities.Role> GetRoles()
        {
            IQueryable<Entities.Role> QuerableList = _context.Role.AsNoTracking().AsQueryable();
            return QuerableList;
        }

        public void RemoveRole(Guid id)
        {
            throw new NotImplementedException();
        }

        public void UpdateRole(Role role)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    // _context = null;
                }
            }
        }
    }
}
