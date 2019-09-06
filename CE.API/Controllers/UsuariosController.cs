using AutoMapper;
using CE.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Controllers
{
    
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;

        public UsuariosController(IUserService userService,
            IMapper mapper)
        {
           _userService = userService
                ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ServiceFilter(typeof(Filters.UsersMinInfoResultFilterAttribute))]
        public async Task<IActionResult> GetUsersAsync()
        { 
            //var userEntity = await _userRolesRepository.GetUsersAsync();
            var userEntity = await _userService.GetUsersAsync();
            return Ok(userEntity);
        }

        [HttpGet]
        [Route("{userId}", Name = "GetUserMinInfo")]
        [ServiceFilter(typeof(Filters.UserMinInfoResultFilterAttribute))]
        public async Task<IActionResult> GetUserAsync(Guid userId)
        {
            var userEntity = await _userService.FindUserAsync(userId);

            if (userEntity == null) return NotFound();

            return Ok(userEntity);
        }

        [HttpPost]
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
        [Route("{userId}", Name = "FullUpdateUserAsync")]
        public async Task<IActionResult> UpdateUserAsync(Guid userId,
            [FromBody] ModelsDto.UsuarioForUpdateDto userForUpdate)
        {
            if (userForUpdate == null) return BadRequest();

            var userEntity = await _userService.FindUserAsync(userId);

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
        [Route("{id}")]
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
    }
}
