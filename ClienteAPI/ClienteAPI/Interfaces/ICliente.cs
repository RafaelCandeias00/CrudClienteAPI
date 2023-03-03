using ClienteAPI.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClienteAPI.Interfaces
{
    public interface ICliente
    {
        Task<IEnumerable<Cliente>> GetAll();
        Task<Cliente> GetById(int id);
        Task Post(Cliente cliente);
        Task Put(Cliente cliente);
        Task Delete(Cliente cliente);
    }
}
