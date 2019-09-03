using CE.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private IUserRolesRepository _userRolesRepository;

        public UsuariosController(IUserRolesRepository userRolesRepository)
        {
            _userRolesRepository = userRolesRepository
                ?? throw new ArgumentNullException(nameof(userRolesRepository));
        }

        [HttpGet]
        [Route("{userId}")]
        [ServiceFilter(typeof(Filters.UsuarioMinInfoResultFilterAttribute))]
        public async Task<IActionResult> GetUserAsync(Guid userId)
        {
            if (!await _userRolesRepository.UserExists(userId)) return NotFound();

            var userEntity = await _userRolesRepository.GetUserAsync(userId);

            return Ok(userEntity);
        }
    }
}
