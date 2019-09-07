using CE.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Services
{
    public interface IUserService
    {
        //Task<IEnumerable<Entities.Usuario>> GetUsersAsync(ResourceParameters resourceParameters);
        (object, PagedList<Entities.Usuario>) GetUsersPagedList(ResourceParameters resourceParameters);
        Task<Communication.UserResponse> SaveAsync(Entities.Usuario user);
        Task<Communication.UserResponse> UpdateAsync(Entities.Usuario user);
        Task<Entities.Usuario> FindUserAsync(Guid id);
        Task<Communication.UserResponse> DeleteAsync(Entities.Usuario user);
    }
}
