using CE.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CE.API.Models.PaginationLinkDto;

namespace CE.API.Services.PaginationServices
{
    public class CreatePaginationLinks : ICreatePaginationLinksWrapper
    {
        private ICreateResourceUri _createResourceUri;
        public CreatePaginationLinks(ICreateResourceUri createResourceUri)
        {
            _createResourceUri = createResourceUri;
        }

        IEnumerable<LinkDto> ICreatePaginationLinksWrapper.CreatePaginationLinks(ResourceParameters resourceParameters,
            bool hasNext, bool hasPrevious, string controllerName)
        {
            var links = new List<LinkDto>();

            // Self
            links.Add(new LinkDto(
                    _createResourceUri.CreateResource(resourceParameters, ResourceUriType.Current, controllerName),
                    "self",
                    "GET"
                    ));

            if (hasPrevious)
            {
                links.Add(new LinkDto(
                    _createResourceUri.CreateResource(resourceParameters, ResourceUriType.PreviousPage, controllerName),
                    "previousPage",
                    "GET"
                    ));
            }

            if (hasNext)
            {
                links.Add(new LinkDto(
                    _createResourceUri.CreateResource(resourceParameters, ResourceUriType.NextPage, controllerName),
                    "nextPage",
                    "GET"
                    ));
            }

            return links;
        }

    }
}
