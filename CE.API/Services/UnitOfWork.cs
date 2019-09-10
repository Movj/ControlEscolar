using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CE.API.Entities;

namespace CE.API.Services
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private CEDatabaseContext _context;

        public UnitOfWork(Entities.CEDatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> CompleteAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
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
                    _context = null;
                }
                //if (_cancellationTokenSource != null)
                //{
                //    _cancellationTokenSource.Dispose();
                //    _cancellationTokenSource = null;
                //}
            }
        }
    }
}
