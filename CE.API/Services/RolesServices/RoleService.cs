using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CE.API.Entities;
using CE.API.Services.Communication;

namespace CE.API.Services.RolesServices
{
    public class RoleService : IRoleService
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

        public Task<RoleResponse> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Role> FindRoleAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Role>> GetRolesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RoleResponse> SaveAsync(Role role)
        {
            throw new NotImplementedException();
        }

        public Task<RoleResponse> UpdateAsync(Role role)
        {
            throw new NotImplementedException();
        }
    }
}
