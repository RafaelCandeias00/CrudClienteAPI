using ClienteAPI.Context;
using ClienteAPI.Interfaces;
using System.Threading.Tasks;

namespace ClienteAPI.Respositorys
{
    public class UnitOfWork : IUnitOfWork
    {
        private ClienteRepository _clienteRepo;
        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context= context;
        }

        public IClienteRepository ClienteRepository
        {
            get { 
                return _clienteRepo = _clienteRepo ?? new ClienteRepository(_context); 
            }
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
