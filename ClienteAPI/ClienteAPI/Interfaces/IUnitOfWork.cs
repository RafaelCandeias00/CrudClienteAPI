using System.Threading.Tasks;

namespace ClienteAPI.Interfaces
{
    public interface IUnitOfWork
    {
        IClienteRepository ClienteRepository { get; }
        Task Commit();
    }
}
