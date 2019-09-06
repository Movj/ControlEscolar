using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CE.API.Entities;
using CE.API.Helpers;
using CE.API.Services.PaginationServices;

namespace CE.API.Services
{
    public class UserService : IUserService
    {
        private IUserRolesRepository _userRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IPropertyMappingService<ModelsDto.UsuarioDto, Entities.Usuario> _propertyMappingService;

        public UserService(IUserRolesRepository userRepository,
            IUnitOfWork unitOfWork, IMapper mapper,
            IPropertyMappingService<ModelsDto.UsuarioDto, Entities.Usuario> propertyMappingService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<Usuario> FindUserAsync(Guid id)
        {
            return await _userRepository.FindUserAsync(id);
        }

        public async Task<IEnumerable<Usuario>> GetUsersAsync(ResourceParameters resourceParameters)
        {
            // Change return data type to pagedlist
            //if (!_propertyMappingService.ValidMappingExistsFor<ModelsDto.UsuarioDto, Entities.Usuario>
            //    (resourceParameters.OrderBy))
            //{
            //    return new List<Usuario>();
            //}
            if (!_propertyMappingService
                .ValidMappingExistsFor(resourceParameters.OrderBy, PropertiesMappingProfiles._userPropertyMinInfoMapping))
            {
                return new List<Usuario>();
            }
            return await _userRepository.GetUsersListAsync();
        }

        public async Task<Communication.UserResponse> SaveAsync(Usuario user)
        {
            // For demo purposes we'll use a simple try catch
            try
            {
                if (user.Id == Guid.Empty)
                {
                    user.Id = Guid.NewGuid();
                }
                await _userRepository.AddAsync(user);
                await _unitOfWork.CompleteAsync();

                return new Communication.UserResponse(user);
            }
            catch (Exception ex)
            {
                return new Communication.UserResponse($"An error occurred when saving the category: {ex.Message}");
            }
        }

        public async Task<Communication.UserResponse> UpdateAsync(Usuario user)
        {
            try
            {
                _userRepository.Update(user);
                await _unitOfWork.CompleteAsync();
                return new Communication.UserResponse(user);
            }
            catch (Exception ex)
            {
                return new Communication.UserResponse($"An error occurred when saving the category: {ex.Message}");
            }
        }

        public async Task<Communication.UserResponse> DeleteAsync(Entities.Usuario user)
        {
            // For demo purposes we'll use a simple try catch
            try
            {
                _userRepository.Remove(user);
                await _unitOfWork.CompleteAsync();

                return new Communication.UserResponse(user);
            }
            catch (Exception ex)
            {
                return new Communication.UserResponse($"An error occurred when saving the category: {ex.Message}");
            }
        }
    }
}
