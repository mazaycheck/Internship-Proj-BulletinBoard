using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Baraholka.Web.Data.Dtos;
using Baraholka.Web.Helpers;
using Baraholka.Domain.Models;

namespace Baraholka.Web.Data.Repositories
{
    public class AnnoucementRepository : GenericRepository<Annoucement>, IAnnoucementRepository
    {
        private readonly AppDbContext _context;
        private readonly IPageService<Annoucement> _pageService;
        private DbSet<Annoucement> _dataSet;

        public AnnoucementRepository(AppDbContext context, IPageService<Annoucement> pageService) : base(context)
        {
            _context = context;
            _pageService = pageService;
            _dataSet = _context.Annoucements;
        }

        public async Task<Annoucement> GetAnnoucementById(int id)
        {
            var includes = new string[]
            {
                $"{nameof(BrandCategory)}.{nameof(BrandCategory.Brand)}",
                $"{nameof(BrandCategory)}.{nameof(BrandCategory.Category)}",
                $"{nameof(User)}.{nameof(User.Town)}",
                $"{nameof(Annoucement.Photos)}"
            };

            Expression<Func<Annoucement, bool>> condition = x => x.AnnoucementId == id;
            var annoucement = await GetSingle(condition, includes);

            return annoucement;
        }

        public IOrderedQueryable<Annoucement> GetAnnoucementsForPaging(AnnoucementFilterArguments filterOptions,
             PageArguments paginateParams, SortingArguments orderParams)
        {
            _dataSet = _context.Annoucements;
            IQueryable<Annoucement> annoucements = IncludeProperties(_dataSet);
            IQueryable<Annoucement> filteredAnnoucements = ApplySeachQuery(annoucements, filterOptions);
            return OrderAnnoucements(filteredAnnoucements, orderParams);
        }

        private static IQueryable<Annoucement> IncludeProperties(DbSet<Annoucement> dataSet)
        {
            return dataSet
                .AsNoTracking()
                .Include(a => a.BrandCategory)
                    .ThenInclude(b => b.Brand)
                .Include(a => a.BrandCategory)
                    .ThenInclude(c => c.Category)
                .Include(u => u.User)
                    .ThenInclude(t => t.Town)
                .Include(p => p.Photos);
        }

        private static IQueryable<Annoucement> ApplySeachQuery(IQueryable<Annoucement> annoucements, AnnoucementFilterArguments searchOptions)
        {
            if (searchOptions.UserId > 0)
            {
                annoucements = annoucements.Where(x => x.UserId == searchOptions.UserId);
            }

            if (!string.IsNullOrWhiteSpace(searchOptions.Category))
            {
                annoucements = annoucements.Where(x => x.BrandCategory.Category.Title.ToLower().Contains(searchOptions.Category.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(searchOptions.Query))
            {
                annoucements = annoucements.Where(x => x.Title.ToLower().Contains(searchOptions.Query.ToLower()) || x.Description.ToLower().Contains(searchOptions.Query.ToLower()));
            }

            return annoucements;
        }

        private static IOrderedQueryable<Annoucement> OrderAnnoucements(IQueryable<Annoucement> annoucements, SortingArguments orderParams)
        {
            IOrderedQueryable<Annoucement> orderedAnnoucements;

            if (orderParams.Direction == "desc")
            {
                switch (orderParams.OrderBy?.ToLower())
                {
                    case "title": orderedAnnoucements = annoucements.OrderByDescending(x => x.Title); break;
                    case "price": orderedAnnoucements = annoucements.OrderByDescending(x => x.Price); break;
                    case "date": orderedAnnoucements = annoucements.OrderByDescending(x => x.CreateDate); break;
                    default: orderedAnnoucements = annoucements.OrderByDescending(x => x.CreateDate); break;
                }
            }
            else
            {
                switch (orderParams.OrderBy?.ToLower())
                {
                    case "title": orderedAnnoucements = annoucements.OrderBy(x => x.Title); break;
                    case "price": orderedAnnoucements = annoucements.OrderBy(x => x.Price); break;
                    case "date": orderedAnnoucements = annoucements.OrderBy(x => x.CreateDate); break;
                    default: orderedAnnoucements = annoucements.OrderBy(x => x.CreateDate); break;
                }
            }

            return orderedAnnoucements;
        }
    }
}