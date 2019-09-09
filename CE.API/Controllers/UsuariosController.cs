using AutoMapper;
using CE.API.Helpers;
using CE.API.Services;
using CE.API.Services.PaginationServices;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CE.API.Models.PaginationLinkDto;

namespace CE.API.Controllers
{

    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ICreatePaginationLinksWrapper _createPaginationLinksWrapper;
        private readonly IUrlHelper _urlHelper;
        public UsuariosController(IUserService userService,
            IMapper mapper,
            ICreatePaginationLinksWrapper createPaginationLinksWrapper,
            IUrlHelper urlHelper)
        {
            _userService = userService
                 ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));

            _createPaginationLinksWrapper = createPaginationLinksWrapper
                ?? throw new ArgumentNullException(nameof(createPaginationLinksWrapper));
            _urlHelper = urlHelper
                ?? throw new ArgumentNullException(nameof(urlHelper));
        }

        [HttpGet(Name = "GetUsers")]
        //[ServiceFilter(typeof(Filters.UsersMinInfoResultFilterAttribute))]
        public IActionResult GetUsers([FromQuery] ResourceParameters resourceParameters)
        {
            // Return a pagedList with an IEnulerable Dto obj
            var (paginationMetadata, pagedList) =
                ((object, PagedList<ModelsDto.UsuarioDto>))_userService.GetUsersPagedList(resourceParameters);

            if (paginationMetadata == null && pagedList == null)
            {
                return BadRequest();
            }

            // Adding pagination headers to response
            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(paginationMetadata));

            // Creating links to send in the response
            // 1st param: ResourceParameters
            // 2nd and 3rd param: boolean properties of PagedList
            // 4th param: ActionName (without "Get"), used to reference this controller (Get)Users
            var paginationLinks =
                _createPaginationLinksWrapper
                .CreatePaginationLinks(resourceParameters,
                pagedList.HasNext, pagedList.HasPrevious,
                "Users");

            // Adding Links for every element
            

            //// Add its own navigation links to every object in the paged list... 
            /// HATEOAS IMPLEMENTATION
            //var listUserWithLinks = new List<object>();

            //foreach (var user in pagedList)
            //{
            //    listUserWithLinks.Add(CreateLinksForUser(user));
            //}


            // in the end, return an annonimous object with,
            // the paginationlist and its links and the general links
            //var resourceWithLinks = new
            //{
            //    users = pagedList,
            //    links = paginationLinks
            //};
            return Ok(pagedList);
        }

        [HttpGet]
        [Route("{id}", Name = "GetUserMinInfo")]
        [ServiceFilter(typeof(Filters.UserMinInfoResultFilterAttribute))]
        public async Task<IActionResult> GetUserAsync(Guid id)
        {
            var userEntity = await _userService.FindUserAsync(id);

            if (userEntity == null) return NotFound();

            return Ok(userEntity);
        }

        [HttpPost(Name = "CreateUserAsync")]
        [ServiceFilter(typeof(Filters.UserMinInfoResultFilterAttribute))]
        public async Task<IActionResult> CreateUserAsync([FromBody] ModelsDto.UsuarioForCreationDto userToCreate)
        {
            // Use BadRequest for errors from client
            if (userToCreate == null) return BadRequest();

            // Model state validation
            if (!ModelState.IsValid) return new UnprocessableEntityObjectResult(ModelState);

            var userEntity = _mapper.Map<Entities.Usuario>(userToCreate);

            var result = await _userService.SaveAsync(userEntity);

            if (!result.Success)
                return BadRequest(result.Message);

            //var userToReturn = await _userRolesRepository.GetUserAsync(userEntity.Id);

            //return CreatedAtRoute("GetUserMinInfo",
            //    new { userId = userEntity.Id },
            //    userToReturn);
            return Ok(userEntity);
        }

        [HttpPut]
        [Route("{id}", Name = "FullUpdateUserAsync")]
        public async Task<IActionResult> UpdateUserAsync(Guid id,
            [FromBody] ModelsDto.UsuarioForUpdateDto userForUpdate)
        {
            if (userForUpdate == null) return BadRequest();

            var userEntity = await _userService.FindUserAsync(id);

            if (userEntity == null) return NotFound();

            // get the user from the repository

            //// Mapping
            _mapper.Map(userForUpdate, userEntity);

            // Checking model state after update
            if (!ModelState.IsValid) return new UnprocessableEntityObjectResult(ModelState);

            //// Applying update, making context aware of changes
            var result = await _userService.UpdateAsync(userEntity);

            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpPatch]
        [Route("{id}", Name = "PartialUpdateUserAsync")]
        public async Task<IActionResult> PartialUpdateUserAsync(Guid id,
            [FromBody] JsonPatchDocument<ModelsDto.UsuarioForUpdateDto> PatchDoc)
        {
            if (PatchDoc == null) return BadRequest();

            // Getting user from db
            var userEntity = await _userService.FindUserAsync(id);

            if (userEntity == null) return NotFound();

            // ModelDto for patch update
            var userToPatch = _mapper.Map<ModelsDto.UsuarioForUpdateDto>(userEntity);

            PatchDoc.ApplyTo(userToPatch, ModelState);

            if (!ModelState.IsValid) return new UnprocessableEntityObjectResult(ModelState);

            // validating model
            TryValidateModel(userToPatch);

            // Checking model state after update
            if (!ModelState.IsValid) return new UnprocessableEntityObjectResult(ModelState);

            // mapping all updated info to a Entity model
            _mapper.Map(userToPatch, userEntity);

            // Applying update, making context aware of changes
            var result = await _userService.UpdateAsync(userEntity);

            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }


        [HttpDelete]
        [Route("{id}", Name = "DeleteUserAsync")]
        public async Task<IActionResult> RemoveUserAsync(Guid id)
        {
            if (id == null) return BadRequest();

            // Getting user from db
            var userEntity = await _userService.FindUserAsync(id);

            if (userEntity == null) return NotFound();

            var result = await _userService.DeleteAsync(userEntity);

            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }

        private ModelsDto.UsuarioDto CreateLinksForUser(ModelsDto.UsuarioDto user)
        {
            /// HATEOAS implementation on a Links attribute in the dto

            //user.Links.Add(new LinkDto(_urlHelper.Link("GetUserMinInfo",
            //    new { id = user.Id }),
            //    "self",
            //    "GET"));

            //user.Links.Add(
            //    new LinkDto(_urlHelper.Link("DeleteUserAsync",
            //    new { id = user.Id }),
            //    "delete_user",
            //    "DELETE"));

            //user.Links.Add(
            //    new LinkDto(_urlHelper.Link("FullUpdateUserAsync",
            //    new { id = user.Id }),
            //    "update_user",
            //    "PUT"));

            //user.Links.Add(
            //    new LinkDto(_urlHelper.Link("PartialUpdateUserAsync",
            //    new { id = user.Id }),
            //    "partially_update_user",
            //    "PATCH"));

            return user;
        }
        private IEnumerable<LinkDto> CreateLinksForUser(Guid id)
        {
            /// HATEOAS implementation
            var links = new List<LinkDto>();

            links.Add(new LinkDto(_urlHelper.Link("GetUserMinInfo",
                new { id }),
                "self",
                "GET"));

            links.Add(
                new LinkDto(_urlHelper.Link("DeleteUserAsync",
                new { id }),
                "delete_user",
                "DELETE"));


            return links;
        }
    }
}
