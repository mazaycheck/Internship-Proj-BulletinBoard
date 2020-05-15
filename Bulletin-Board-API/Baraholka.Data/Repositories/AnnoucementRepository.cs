using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using Baraholka.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
{
    public class AnnoucementRepository : GenericRepository<Annoucement>, IAnnoucementRepository
    {
        private readonly AppDbContext _context;
        private DbSet<Annoucement> _dataSet;

        public AnnoucementRepository(AppDbContext context) : base(context)
        {
            _context = context;
            _dataSet = _context.Annoucements;
        }

        public async Task<Annoucement> GetSingleAnnoucementForViewById(int id)
        {
            var includes = new string[]
            {
                $"{nameof(BrandCategory)}.{nameof(BrandCategory.Brand)}",
                $"{nameof(BrandCategory)}.{nameof(BrandCategory.Category)}",
                $"{nameof(User)}.{nameof(User.Town)}",
                $"{nameof(Annoucement.Photos)}"
            };

            var filters = new List<Expression<Func<Annoucement, bool>>>
            {
                x => x.AnnoucementId == id
            };

            var annoucement = await GetSingle(includes, filters);

            return annoucement;
        }

        public async Task<PageDataContainer<Annoucement>> GetPagedAnnoucements(AnnoucementFilterArguments filterOptions,
             PageArguments paginateParams, SortingArguments orderParams)
        {
            _dataSet = _context.Annoucements;
            IQueryable<Annoucement> annoucements = IncludeProperties(_dataSet);
            IQueryable<Annoucement> filteredAnnoucements = ApplySeachQuery(annoucements, filterOptions);
            IOrderedQueryable<Annoucement> orderedAnnoucements =  OrderAnnoucements(filteredAnnoucements, orderParams);
            return await orderedAnnoucements.GetPage(paginateParams);
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