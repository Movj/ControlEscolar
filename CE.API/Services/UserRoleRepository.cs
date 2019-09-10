using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CE.API.Entities;
using CE.API.Extensions;
using CE.API.Helpers;
using CE.API.ModelsDto;
using CE.API.Services.PaginationServices;
using Microsoft.EntityFrameworkCore;

namespace CE.API.Services
{
    public class UserRoleRepository : BaseRepository, IUserRolesRepository, IDisposable
    {
        private IPropertyMappingService<ModelsDto.UsuarioDto, Entities.Usuario> _propertyMappingService;
        private IMapper _mapper;

        public UserRoleRepository(CEDatabaseContext context,
            IPropertyMappingService<ModelsDto.UsuarioDto, Entities.Usuario> propertyMappingService,
            IMapper mapper) : base(context)
        {
            _propertyMappingService = propertyMappingService;
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddAsync(Usuario usuario)
        {
            await _context.Usuario.AddAsync(usuario);
        }


        public async Task<Usuario> FindUserAsync(Guid id)
        {
            return await _context.Usuario.AsNoTracking().FirstOrDefaultAsync(w=>w.Id == id);
        }

        public IQueryable<Usuario> GetUsersList()
        {
            // var userRoles = _context.RolesUsuario.Include(i => i.Usuario).Include(r => r.Role);
            //var collectionBeforePaging = _context.Usuario.AsQueryable();
            IQueryable<Entities.Usuario> collectionBeforePaging = _context.Usuario.AsNoTracking().AsQueryable();

            // Mapping data from Entities.Usuario to ModelsDto.UsuarioDto
            return collectionBeforePaging;
        }

        public void Update(Usuario usuario)
        {
            _context.Usuario.Update(usuario);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    // _context = null;
                }
                //if (_cancellationTokenSource != null)
                //{
                //    _cancellationTokenSource.Dispose();
                //    _cancellationTokenSource = null;
                //}
            }
        }

        public void Remove(Usuario user)
        {
            _context.Usuario.Remove(user);
        }
    }
}
