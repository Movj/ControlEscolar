using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CE.API.Entities;
using CE.API.Extensions;
using CE.API.Helpers;
using CE.API.Services.Communication;
using CE.API.Services.PaginationServices;

namespace CE.API.Services.RolesServices
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly IPropertyMappingService<Models.RoleDtoModels.RoleDto, Entities.Role> _propertyMappingService;
        private readonly ICreateResourceUri _createResourceUri;
        private readonly IApplySort _applySort;

        public RoleService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IRoleRepository roleRepository,
            IPropertyMappingService<Models.RoleDtoModels.RoleDto, Entities.Role> propertyMappingService,
            ICreateResourceUri createResourceUri,
            IApplySort applySort)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _propertyMappingService = propertyMappingService;
            _createResourceUri = createResourceUri;
            _applySort = applySort;
        }

        public Task<RoleResponse> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Role> FindRoleAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public (object, PagedList<Models.RoleDtoModels.RoleDto>) 
            GetRoles(ResourceParameters resourceParameters)
        {
            // Change return data type to pagedlist
            if (!_propertyMappingService
                .ValidMappingExistsFor(resourceParameters.OrderBy, PropertiesMappingProfiles._userPropertyMinInfoMapping))
            {
                return (null, null);
            }

            var collectionBeforePaging = _roleRepository.GetRoles();

            if (collectionBeforePaging == null) return ((null, null));

            var sortingCollection = _applySort.ApplySort(collectionBeforePaging,
                resourceParameters.OrderBy,
                PropertiesMappingProfiles._rolePropertyMapping);

            // Applying querying
            if (!string.IsNullOrEmpty(resourceParameters.SearchQuery))
            {
                // Trim and ignore casing
                var searchQueryForWhereClause = resourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                sortingCollection = sortingCollection
                    .Where(w => w.RoleName.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            var sortedCollection = (IEnumerable<Entities.Role>)sortingCollection.ToList();

            var sortedCollectionDto = _mapper.Map<IEnumerable<Models.RoleDtoModels.RoleDto>>(sortedCollection);

            var sortedCollectionIQuerableDto = sortedCollectionDto.AsQueryable<Models.RoleDtoModels.RoleDto>();

            var pagedListDto = PagedList<Models.RoleDtoModels.RoleDto>.Create(sortedCollectionIQuerableDto,
                resourceParameters.pageNumber,
                resourceParameters.PageSize);

            // Generating PagedList metadata
            // Creating links to send in the response with _createResourceUri service
            // 1st param: ResourceParameters
            // 2nd and 3rd param: boolean properties of PagedList
            // 4th param: ActionName (without "Get"), used to reference its controller (Get)Users

            var previousPageLink = pagedListDto.HasPrevious ?
                    _createResourceUri.CreateResource(resourceParameters,
                    ResourceUriType.PreviousPage, "Roles") : null;

            var nextPageLink = pagedListDto.HasNext ?
                _createResourceUri.CreateResource(resourceParameters,
                ResourceUriType.NextPage, "Roles") : null;

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
