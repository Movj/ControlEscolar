using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Filters
{
    public class UsersMinInfoResultFilterAttribute : ResultFilterAttribute
    {
        private IMapper _map;

        public UsersMinInfoResultFilterAttribute(IMapper mapper)
        {
            _map = mapper;
        }

        /*
         Overriding OnResultExecutionAsync method in order to stablish
         a pipeline filter, which will be executed before|after the IActionResult method
         in BooksController.
         This method will control the delegate execution, depending on the result of the controller,
         this by using resultFromAction.Value prop.
        */

        public override async Task OnResultExecutionAsync(ResultExecutingContext context,
            ResultExecutionDelegate next)
        {
            var resultFromAction = context.Result as ObjectResult;
            if (resultFromAction?.Value == null
                || resultFromAction.StatusCode < 200
                || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            // Mapping result from GetUserAsync IActionResult method
            //resultFromAction.Value = _map.Map<IEnumerable<ModelsDto.UsuarioDto>>(resultFromAction.Value);
            //resultFromAction.Value = _map.Map<IEnumerable<ModelsDto.UsuarioDto>>(resultFromAction.Value);

            await next();
        }
    }
}
