using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CE.API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CE.API.Services.PaginationServices
{
    public class CreateResourceUri : ICreateResourceUri
    {
        private readonly IUrlHelper _urlHelper;

        public CreateResourceUri(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public string CreateResource(ResourceParameters resourceParameters,
            ResourceUriType type, string actionName)
        {
            actionName.Trim();
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link($"Get{actionName}",
                        new
                        {
                            orderBy = resourceParameters.OrderBy,
                            searchQuery = resourceParameters.SearchQuery,
                            pageNumer = resourceParameters.pageNumber - 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link($"Get{actionName}",
                        new
                        {
                            orderBy = resourceParameters.OrderBy,
                            searchQuery = resourceParameters.SearchQuery,
                            pageNumer = resourceParameters.pageNumber + 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link($"Get{actionName}",
                        new
                        {
                            orderBy = resourceParameters.OrderBy,
                            searchQuery = resourceParameters.SearchQuery,
                            pageNumer = resourceParameters.pageNumber,
                            pageSize = resourceParameters.PageSize
                        });
            }
        }
    }
}
