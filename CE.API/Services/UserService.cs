﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CE.API.Entities;
using CE.API.Extensions;
using CE.API.Helpers;
using CE.API.Models.RoleDtoModels;
using CE.API.Services.Communication;
using CE.API.Services.PaginationServices;
using Microsoft.AspNetCore.Mvc;
using static CE.API.Models.PaginationLinkDto;

namespace CE.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRolesRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService<ModelsDto.UsuarioDto, Entities.Usuario> _propertyMappingService;
        private readonly ICreateResourceUri _createResourceUri;
        private readonly IApplySort _applySort;
        public UserService(IUserRolesRepository userRepository,
            IUnitOfWork unitOfWork, IMapper mapper,
            IPropertyMappingService<ModelsDto.UsuarioDto, Entities.Usuario> propertyMappingService,
            ICreateResourceUri createResourceUri,
            IApplySort applySort)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _propertyMappingService = propertyMappingService;
            _createResourceUri = createResourceUri;
            _applySort = applySort;
        }

        public async Task<Usuario> FindUserAsync(Guid id)
        {
            return await _userRepository.FindUserAsync(id);
        }

        public (object, PagedList<ModelsDto.UsuarioDto>) GetUsersPagedList(ResourceParameters resourceParameters)
        {
            // Change return data type to pagedlist
            if (!_propertyMappingService
                .ValidMappingExistsFor(resourceParameters.OrderBy, PropertiesMappingProfiles._userPropertyMinInfoMapping))
            {
                return (null, null);
            }

            // Getting an IQuerable obj of Entities.Usuario
            var collectionBeforePaging =  _userRepository.GetUsersList();

            if (collectionBeforePaging == null) return ((null, null));

            // Using sorting service into IQuerable obj
            var sortingCollection = 
                _applySort.ApplySort(collectionBeforePaging, 
                resourceParameters.OrderBy, 
                PropertiesMappingProfiles._userPropertyMinInfoMapping);

            // Applying querying
            if (!string.IsNullOrEmpty(resourceParameters.SearchQuery))
            {
                // Trim and ignore casing
                var searchQueryForWhereClause = resourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                sortingCollection = sortingCollection
                    .Where(w => w.Email.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || w.ApellidoPaterno.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || w.ApellidoMaterno.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || w.Nombre.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || w.CodigoPostal.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || w.TelefonoCasa.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || w.TelefonoCelular.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            // Parsing IQuerable obj to a IEnumerable obj in order to map them
            var sortedCollection = (IEnumerable<Entities.Usuario>) sortingCollection.ToList();

            // Mapping from IEnumerable<Entities.Usuario> to a UsuarioDto model
             var sortedCollectionDto = _mapper.Map<IEnumerable<ModelsDto.UsuarioDto>>(sortedCollection);

            // Parsing them again in order to add them to a PagedList<T>
            var sortedCollectionIQuerableDto = sortedCollectionDto.AsQueryable<ModelsDto.UsuarioDto>();

            var pagedListDto = PagedList<ModelsDto.UsuarioDto>.Create(sortedCollectionIQuerableDto,
                resourceParameters.pageNumber,
                resourceParameters.PageSize);

            // Generating PagedList metadata
            // Creating links to send in the response with _createResourceUri service
            // 1st param: ResourceParameters
            // 2nd and 3rd param: boolean properties of PagedList
            // 4th param: ActionName (without "Get"), used to reference its controller (Get)Users

            var previousPageLink = pagedListDto.HasPrevious ?
                    _createResourceUri.CreateResource(resourceParameters,
                    ResourceUriType.PreviousPage, "Users") : null;

            var nextPageLink = pagedListDto.HasNext ?
                _createResourceUri.CreateResource(resourceParameters,
                ResourceUriType.NextPage, "Users") : null;

            var paginationMetadata = new
            {
                previousPageLink,
                nextPageLink,
                totalCount = pagedListDto.TotalCount,
                pageSize = pagedListDto.PageSize,
                currentPage = pagedListDto.CurrentPage,
                totalPages = pagedListDto.TotalPages
            };


            // Returning a tuple of: object metadata and PagedList<T>
            return ((paginationMetadata, pagedListDto));
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
                var result = await _unitOfWork.CompleteAsync();

                if (!result)
                {
                    return new Communication.UserResponse($"An error occurred when saving");
                }
                return new Communication.UserResponse(user);
            }
            catch (Exception ex)
            {
                return new Communication.UserResponse($"An error occurred when saving the user: {ex.Message}");
            }
        }

        public async Task<Communication.UserResponse> UpdateAsync(Usuario user)
        {
            try
            {
                _userRepository.Update(user);
                var result =  await _unitOfWork.CompleteAsync();

                if (!result)
                {
                    return new Communication.UserResponse($"An error occurred when saving");
                }
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
                var result = await _unitOfWork.CompleteAsync();

                if (!result)
                {
                    return new Communication.UserResponse($"An error occurred when saving");
                }

                return new Communication.UserResponse(user);
            }
            catch (Exception ex)
            {
                return new Communication.UserResponse($"An error occurred when saving the category: {ex.Message}");
            }
        }

        public async Task<Models.UsuarioRolesDto> GetRolesForUser(Entities.Usuario user)
        {
            // Get all related roles to a specific user
            var userRoleEntityList = await _userRepository.GetRolesForUserAsync(user.Id);

            Models.UsuarioRolesDto userRoleDto = _mapper.Map<Models.UsuarioRolesDto>(user);

            var roleListDto = new List<Models.RoleDtoModels.RoleDto>();
            foreach (var item in userRoleEntityList)
            {
                var map = _mapper.Map<Models.RoleDtoModels.RoleDto>(item.Role);
                roleListDto.Add(map);
            }

            userRoleDto.Roles = roleListDto;

            return userRoleDto;
        }

        public async Task<bool> AddRoleToUserAsync(Guid userId, Guid roleId)
        {
            var userEntity = await _userRepository.FindUserAsync(userId);
            if (userEntity == null)
            {
                return false;
            }
            var roleEntity = await _userRepository.GetRoleAsync(roleId);
            if (roleEntity == null) return false;
            // For demo purposes we'll use a simple try catch
                _userRepository.AddRoleToUser(userId, roleId);
            var result = await _unitOfWork.CompleteAsync();
            return result;
        }

        public async Task<Role> FindRoleByNameAsync(string name)
        {
            return await _userRepository.FindRoleByNameAsync(name);
        }

        public async Task<bool> RemoveRoleOfUser(Entities.Role role, Entities.Usuario user)
        {
            Entities.RolesUsuario rolesUsuario = new RolesUsuario() { Role = role, Usuario = user };
            _userRepository.RemoveRoleOfUser(rolesUsuario);
            var result = await _unitOfWork.CompleteAsync();
            return result;
        }
    }
}
