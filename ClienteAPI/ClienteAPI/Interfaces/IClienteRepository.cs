using ClienteAPI.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClienteAPI.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<Cliente> GetByCPF(string cpf);
        Task<IEnumerable<Cliente>> GetByNome(string nome);
    }
}
