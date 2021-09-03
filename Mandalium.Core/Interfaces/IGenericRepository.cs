using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mandalium.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get<Type>(Type id);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(ISpecification<T> specification = null);
        Task Delete<Type>(Type id);
        Task Update(T entity);
        Task Save(T entity);
    }
}
