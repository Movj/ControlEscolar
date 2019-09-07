using CE.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Services.PaginationServices
{
    public interface ICreateResourceUri
    {
        string CreateResource(ResourceParameters resourceParameters,
            ResourceUriType resourceUriType, string actionName);
    }
}
