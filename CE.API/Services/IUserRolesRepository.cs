using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Services
{
    public interface IUserRolesRepository
    {
        // For Get
        Task<IEnumerable<Entities.Usuario>> GetUsersAsync();
        Task<IEnumerable<Entities.Usuario>> GetUsersFullInfoAsync();
        Task<Entities.Usuario> GetUserAsync(Guid userId);
        Task<bool> UserExists(Guid userId);
        // For Post
        void AddUser(Entities.Usuario usuario);
        // For Put and patch
        void UpdateUser(Entities.Usuario usuario);
        // For Delete
        void DeleteUser(Entities.Usuario usuario);

        Task<bool> SaveChangesAsync();
    }
}
