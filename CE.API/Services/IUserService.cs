﻿using CE.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Services
{
    public interface IUserService
    {
        (object, PagedList<ModelsDto.UsuarioDto>) GetUsersPagedList(ResourceParameters resourceParameters);
        Task<Communication.UserResponse> SaveAsync(Entities.Usuario user);
        Task<Communication.UserResponse> UpdateAsync(Entities.Usuario user);
        Task<Entities.Usuario> FindUserAsync(Guid id);
        Task<Communication.UserResponse> DeleteAsync(Entities.Usuario user);
    }
}