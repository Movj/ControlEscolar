using AutoMapper;
using CE.API.Services;
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
            if (!await _userRolesRepository.UserExists(userId))
            {
                return NotFound();
            }

            // Model state validation
            if (!ModelState.IsValid) return new UnprocessableEntityObjectResult(ModelState);

            // Getting user from db
            var userEntity = await _userRolesRepository.GetUserAsync(userId);

            // Mapping
            _mapper.Map(userForUpdate, userEntity);

            if (!ModelState.IsValid) return new UnprocessableEntityObjectResult(ModelState);

            // Applying update
            _userRolesRepository.UpdateUser(userEntity);

            if (!await _userRolesRepository.SaveChangesAsync())
            {
                throw new Exception($"Error al momento de guardar los cambios - Actualizando {userId}");
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
