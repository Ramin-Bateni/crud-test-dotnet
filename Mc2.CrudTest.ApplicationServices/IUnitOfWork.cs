using System.Threading.Tasks;

namespace Mc2.CrudTest.Domain2
{
    public interface IUnitOfWork
    {
        Task<int> SaveAsync();
    }
}
