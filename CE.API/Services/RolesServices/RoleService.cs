using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CE.API.Entities;
using CE.API.Services.Communication;

namespace CE.API.Services.RolesServices
{
    public class RoleService : IUserService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IRoleRepository _roleRepository;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper,
            IRoleRepository roleRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _roleRepository = roleRepository;
        }
        public Task<UserResponse> DeleteAsync(Usuario user)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> FindUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Usuario>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserResponse> SaveAsync(Usuario user)
        {
            throw new NotImplementedException();
        }

        public Task<UserResponse> UpdateAsync(Usuario user)
        {
            throw new NotImplementedException();
        }
    }
}
