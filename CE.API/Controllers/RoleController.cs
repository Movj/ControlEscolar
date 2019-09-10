using AutoMapper;
using CE.API.Helpers;
using CE.API.Services.RolesServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Controllers.RoleController.v1
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        private readonly IRoleService _roleService;

        public RoleController(IMapper mapper,
            IUrlHelper urlHelper,
            IRoleService roleService)
        {
            _mapper = mapper
               ?? throw new ArgumentNullException(nameof(mapper));
            _urlHelper = urlHelper
                ?? throw new ArgumentNullException(nameof(urlHelper));
            _roleService = roleService
                ?? throw new ArgumentNullException(nameof(roleService));
        }

        [HttpGet(Name = "GetRoles")]
        public IActionResult GetRoles([FromQuery] ResourceParameters resourceParameters)
        {
            var (paginationMetadata, pagedListDto) =
                ((object, Helpers.PagedList<Models.RoleDtoModels.RoleDto>))_roleService.GetRoles(resourceParameters);

            if (paginationMetadata == null || pagedListDto == null) return BadRequest();

            // Adding pagination headers to response
            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(paginationMetadata));

            return Ok(pagedListDto);
        }

    }
}
