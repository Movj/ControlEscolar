using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CE.API.Entities;
using CE.API.Helpers;
using CE.API.Services.PaginationServices;
using Microsoft.AspNetCore.Mvc;

namespace CE.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRolesRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService<ModelsDto.UsuarioDto, Entities.Usuario> _propertyMappingService;
        private readonly ICreateResourceUri _createResourceUri;
        private readonly IUrlHelper _uriHelper;
        public UserService(IUserRolesRepository userRepository,
            IUnitOfWork unitOfWork, IMapper mapper,
            IPropertyMappingService<ModelsDto.UsuarioDto, Entities.Usuario> propertyMappingService,
            ICreateResourceUri createResourceUri,
            IUrlHelper uriHelper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _propertyMappingService = propertyMappingService;
            _createResourceUri = createResourceUri;
            _uriHelper = uriHelper;
        }

        public async Task<Usuario> FindUserAsync(Guid id)
        {
            return await _userRepository.FindUserAsync(id);
        }

        public (object, PagedList<Entities.Usuario>) GetUsersPagedList(ResourceParameters resourceParameters)
        {
            // Change return data type to pagedlist
            if (!_propertyMappingService
                .ValidMappingExistsFor(resourceParameters.OrderBy, PropertiesMappingProfiles._userPropertyMinInfoMapping))
            {
                return (null, null);
            }
            var usersFromRepo =  _userRepository.GetUsersList(resourceParameters);

            // Generating PagedList metadata

            var previousPageLink = usersFromRepo.HasPrevious ?
                    _createResourceUri.CreateResource(resourceParameters,
                    ResourceUriType.PreviousPage, "Authors") : null;

            var nextPageLink = usersFromRepo.HasNext ?
                _createResourceUri.CreateResource(resourceParameters,
                ResourceUriType.NextPage, "Authors") : null;

            var paginationMetadata = new
            {
                previousPageLink,
                nextPageLink,
                totalCount = usersFromRepo.TotalCount,
                pageSize = usersFromRepo.PageSize,
                currentPage = usersFromRepo.CurrentPage,
                totalPages = usersFromRepo.TotalPages
            };

            return ((paginationMetadata, usersFromRepo));
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
