using Baraholka.Data.Dtos;
using System.Linq;
using System.Threading.Tasks;

namespace Baraholka.Web.Helpers
{
    public interface IPageService<T>
    {
        Task<PageDataContainer<T>> Paginate(IOrderedQueryable<T> queryAbleData, PageArguments pageParams);
    }
}