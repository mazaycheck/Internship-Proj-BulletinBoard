using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Models;

namespace WebApplication2.Helpers
{
    public static class AnnoucementExtensions
    {
        public static IQueryable<AnnoucementForViewDto> ApplySeachQuery(this IQueryable<AnnoucementForViewDto> annoucements, AnnoucementFilter searchOptions)
        {           
            if (searchOptions.UserId > 0)
            {
                annoucements = annoucements.Where(x => x.UserId == searchOptions.UserId);
            }

            if (!string.IsNullOrWhiteSpace(searchOptions.Category))
            {
                annoucements = annoucements.Where(x => x.Category.ToLower().Contains(searchOptions.Category.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(searchOptions.Query))
            {
                annoucements = annoucements.Where(x => x.Title.ToLower().Contains(searchOptions.Query.ToLower()) || x.Description.ToLower().Contains(searchOptions.Query.ToLower()));
            }

            return annoucements;
        }

        public static IQueryable<AnnoucementForViewDto> OrderAnnoucements(this IQueryable<AnnoucementForViewDto> annoucements, OrderParams orderParams)
        {
            IQueryable<AnnoucementForViewDto> orderedAnnoucements;

            if (orderParams.Direction == "desc")
            {
                switch (orderParams.OrderBy?.ToLower())
                {
                    case "title": orderedAnnoucements = annoucements.OrderByDescending(x => x.Title); break;
                    case "price": orderedAnnoucements = annoucements.OrderByDescending(x => x.Price); break;
                    case "date": orderedAnnoucements = annoucements.OrderByDescending(x => x.Date); break;
                    default: orderedAnnoucements = annoucements.OrderByDescending(x => x.Date); break;
                }

            }

            else { 
                switch (orderParams.OrderBy?.ToLower())
                {
                    case "title": orderedAnnoucements = annoucements.OrderBy(x => x.Title); break;
                    case "price": orderedAnnoucements = annoucements.OrderBy(x => x.Price); break;
                    case "date": orderedAnnoucements = annoucements.OrderBy(x => x.Date); break;
                    default: orderedAnnoucements = annoucements.OrderBy(x => x.Date); break;
                }
            }

            return orderedAnnoucements;

        }
    }
}
