using System.Threading.Tasks;

namespace Mc2.CrudTest.ApplicationServices
{
    public interface IUnitOfWork
    {
        Task<int> SaveAsync();
    }
}
