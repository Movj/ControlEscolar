using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CE.API.Entities;
using CE.API.ModelsDto;
using Microsoft.EntityFrameworkCore;

namespace CE.API.Services
{
    public class UserRoleRepository : IUserRolesRepository, IDisposable
    {
        private CEDatabaseContext _context;
        public UserRoleRepository(CEDatabaseContext context)
        {
            _context = context;
        }

        public void AddUser(Usuario usuario)
        {
            if (usuario != null)
            {
                if (usuario.Id == Guid.Empty)
                {
                    usuario.Id = Guid.NewGuid();
                }
                _context.Usuario.Add(usuario);
            }
            else
            {
                throw new ArgumentNullException(nameof(usuario));
            }
            
        }

        public void DeleteUser(Usuario usuario)
        {
            _context.Usuario.Remove(usuario);
        }

        public async Task<Usuario> GetUserAsync(Guid userId)
        {
            return await _context.Usuario.FindAsync(userId);
        }

        public async Task<IEnumerable<Usuario>> GetUsersAsync()
        {
            return await _context.Usuario.ToListAsync();
        }

        public Task<IEnumerable<Usuario>> GetUsersFullInfoAsync()
        {
            throw new NotImplementedException();
        }


        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public void UpdateUser(Usuario usuario)
        {
            // no code in this implementation
            _context.Usuario.Attach(usuario);
        }

        public async Task<bool> UserExists(Guid userId)
        {
            var userEntity = await _context.Usuario.FirstOrDefaultAsync(w => w.Id == userId);
            if (userEntity == null) return false;
            return true;
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
                    _context = null;
                }
                //if (_cancellationTokenSource != null)
                //{
                //    _cancellationTokenSource.Dispose();
                //    _cancellationTokenSource = null;
                //}
            }
        }
    }
}
