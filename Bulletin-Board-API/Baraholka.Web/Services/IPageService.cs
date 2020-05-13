using System.Linq;
using System.Threading.Tasks;
using Baraholka.Web.Data.Dtos;

namespace Baraholka.Web.Helpers
{
    public interface IPageService<T>
    {
        Task<PageDataContainer<T>> Paginate(IOrderedQueryable<T> queryAbleData, PageArguments pageParams);
    }
}