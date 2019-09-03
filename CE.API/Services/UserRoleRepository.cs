using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CE.API.Entities;
using CE.API.ModelsDto;

namespace CE.API.Services
{
    public class UserRoleRepository : IUserRolesRepository
    {
        private CEDatabaseContext _context;
        public UserRoleRepository(CEDatabaseContext context)
        {
            _context = context;
        }

        public void AddUser(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public async Task<Usuario> GetUserAsync(Guid userId)
        {
            return await _context.Usuario.FindAsync(userId);
        }

        public Task<IEnumerable<Usuario>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Usuario>> GetUsersFullInfoAsync()
        {
            throw new NotImplementedException();
        }

        public void PartualUpdateUser(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UserExists(Guid userId)
        {
            var userEntity = await _context.Usuario.FindAsync(userId);
            if (userEntity == null) return false;
            return true;
        }
    }
}
