using CE.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CE.API.Models.PaginationLinkDto;

namespace CE.API.Services.PaginationServices
{
    public interface ICreatePaginationLinksWrapper
    {
        IEnumerable<LinkDto> CreatePaginationLinks(ResourceParameters resourceParameters,
            bool hasNext, bool hasPrevious, string controllerName);
    }
}
