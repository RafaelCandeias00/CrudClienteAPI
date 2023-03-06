using ClienteAPI.Context;
using ClienteAPI.Interfaces;
using ClienteAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClienteAPI.Respositorys
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        private readonly AppDbContext _context;
        public ClienteRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Cliente> GetByCPF(string cpf)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Cpf == cpf);
        }

        public async Task<IEnumerable<Cliente>> GetByNome(string nome)
        {
            return await _context.Clientes.Where(n => n.Nome.Contains(nome)).ToListAsync();
        }
    }
}
