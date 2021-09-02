using System;
using System.Threading.Tasks;

namespace Mandalium.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        Task Save();
        IGenericRepository<T> GetRepository<T>() where T : class;
    }
}
