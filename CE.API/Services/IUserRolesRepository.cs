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

        // For Get
        Task<IEnumerable<Entities.Usuario>> GetUsersListAsync();
        Task AddAsync(Entities.Usuario user);
        void Update(Entities.Usuario user);
        Task<Entities.Usuario> FindUserAsync(Guid id);
        void Remove(Entities.Usuario user);
        //Task<IEnumerable<Entities.Usuario>> GetUsersFullInfoAsync();
        //Task<Entities.Usuario> GetUserAsync(Guid userId);
        //Task<bool> UserExists(Guid userId);
        //// For Post
        //void AddUser(Entities.Usuario usuario);
        //// For Put and patch
        //void UpdateUser(Entities.Usuario usuario);
        //// For Delete
        //void DeleteUser(Entities.Usuario usuario);

        //Task<bool> SaveChangesAsync();
    }
}
