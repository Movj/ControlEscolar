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
        private IUserRolesRepository _userRolesRepository;
        private IMapper _mapper;

        public UsuariosController(IUserRolesRepository userRolesRepository,
            IMapper mapper)
        {
            _userRolesRepository = userRolesRepository
                ?? throw new ArgumentNullException(nameof(userRolesRepository));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ServiceFilter(typeof(Filters.UsersMinInfoResultFilterAttribute))]
        public async Task<IActionResult> GetUsersAsync(Guid userId)
        { 
            var userEntity = await _userRolesRepository.GetUsersAsync();
            return Ok(userEntity);
        }

        [HttpGet]
        [Route("{userId}", Name = "GetUserMinInfo")]
        [ServiceFilter(typeof(Filters.UserMinInfoResultFilterAttribute))]
        public async Task<IActionResult> GetUserAsync(Guid userId)
        {
            if (!await _userRolesRepository.UserExists(userId)) return NotFound();

            var userEntity = await _userRolesRepository.GetUserAsync(userId);

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

            _userRolesRepository.AddUser(userEntity);

            if (!await _userRolesRepository.SaveChangesAsync())
            {
                throw new Exception($"Error al momento de guardar los cambios - Creando usuario");
            }

            var userToReturn = await _userRolesRepository.GetUserAsync(userEntity.Id);

            return CreatedAtRoute("GetUserMinInfo",
                new { userId = userEntity.Id },
                userToReturn);
        }

        [HttpPut]
        [Route("{userId}", Name = "FullUpdateUserAsync")]
        public async Task<IActionResult> UpdateUserAsync(Guid userId,
            [FromBody] ModelsDto.UsuarioForUpdateDto userForUpdate)
        {
            if (userForUpdate == null) return BadRequest();

            if (!await _userRolesRepository.UserExists(userId)) return NotFound();

            // Getting user from db
            var userEntity = await _userRolesRepository.GetUserAsync(userId);

            // Mapping
            _mapper.Map(userForUpdate, userEntity);

            // Checking model state after update
            if (!ModelState.IsValid) return new UnprocessableEntityObjectResult(ModelState);

            // Applying update, making context aware of changes
            _userRolesRepository.UpdateUser(userEntity);

            if (!await _userRolesRepository.SaveChangesAsync())
            {
                //throw new Exception($"Error al momento de guardar los cambios - Actualizando {userId}");
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPatch]
        [Route("{id}", Name = "PartialUpdateUserAsync")]
        public async Task<IActionResult> PartualUpdateUserAsync(Guid id,
            [FromBody] JsonPatchDocument<ModelsDto.UsuarioForUpdateDto> PatchDoc)
        {
            if (PatchDoc == null) return BadRequest();

            if (!await _userRolesRepository.UserExists(id)) return NotFound();

            // Getting user from db
            var userEntity = await _userRolesRepository.GetUserAsync(id);

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
            _userRolesRepository.UpdateUser(userEntity);

            if (!await _userRolesRepository.SaveChangesAsync())
            {
                //throw new Exception($"Error al momento de guardar los cambios - Actualizando {userId}");
                return BadRequest();
            }

            return NoContent();
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> RemoveUserAsync(Guid id)
        {
            if (id == null) return BadRequest();

            if (!await _userRolesRepository.UserExists(id))
            {
                return NotFound();
            }

            var userToDelete = await _userRolesRepository.GetUserAsync(id);

            _userRolesRepository.DeleteUser(userToDelete);

            if (!await _userRolesRepository.SaveChangesAsync())
            {
                throw new Exception($"Error al momento de guardar los cambios - Eliminación {id}");
            }

            return NoContent();
        }
    }
}
