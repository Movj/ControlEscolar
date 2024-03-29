﻿using AutoMapper;
using CE.API.Helpers;
using CE.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CE.API.Models.PaginationLinkDto;

namespace CE.API.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        public UsuariosController(IUserService userService,
            IMapper mapper,
            IUrlHelper urlHelper)
        {
            _userService = userService
                 ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
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

            if (paginationMetadata == null || pagedList == null)
            {
                return BadRequest();
            }

            // Adding pagination headers to response
            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(paginationMetadata));

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

        [HttpGet]
        [Route("{id}/roles", Name = "GetUserUserRoles")]
        //[ServiceFilter(typeof(Filters.UserMinInfoResultFilterAttribute))]
        public async Task<IActionResult> GetUserRolesAsync(Guid id)
        {
            var userEntity = await _userService.FindUserAsync(id);

            if (userEntity == null) return NotFound();

            var userRoles = await _userService.GetRolesForUser(userEntity);

            if (userRoles == null) return NotFound();

            return Ok(userRoles);
        }

        [HttpPost]
        [Route("{id}/roles")]
        public async Task<IActionResult> AddRoleToUser(Guid id,
            [FromBody] Models.RoleDtoModels.RoleDto roleDto)
        {
            if (roleDto == null) return BadRequest();

            var roleEntity = await _userService.FindRoleByNameAsync(roleDto.RoleName);

            if (roleEntity == null) return NotFound();

            var userEntity = await _userService.FindUserAsync(id);

            if (userEntity == null) return NotFound();

            // add validation of exiting relation
            var userRoles = await _userService.GetRolesForUser(userEntity);

            if (userRoles.Roles.FindAll(f => f.RoleName == roleEntity.RoleName).Count > 0)
            {
                return BadRequest();
            }

            var response = await _userService.AddRoleToUserAsync(id, roleEntity.Id);

            if (!response) return BadRequest();

            var newUserRoles = await _userService.GetRolesForUser(userEntity);

            return CreatedAtRoute("GetUserUserRoles", new { id = userEntity.Id }, newUserRoles);
        }

        [HttpDelete]
        [Route("{userId}/roles")]
        public async Task<IActionResult> RemoveRoleOfUser([FromQuery] string roleName, Guid userId)
        {
            var roleEntity = await _userService.FindRoleByNameAsync(roleName);

            if (roleEntity == null) return NotFound();

            var userEntity = await _userService.FindUserAsync(userId);

            if (userEntity == null) return NotFound();

            var result = await _userService.RemoveRoleOfUser(roleEntity, userEntity);

            if (!result) return BadRequest();

            return NoContent();
        }
    }
}
