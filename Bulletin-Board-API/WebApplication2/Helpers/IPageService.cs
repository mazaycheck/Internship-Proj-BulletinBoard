using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;

namespace WebApplication2.Helpers
{
    public interface IPageService<T>
    {
        Task<PageDataContainer<T>> Paginate(IOrderedQueryable<T> queryAbleData, PageArguments pageParams);        
    }
}