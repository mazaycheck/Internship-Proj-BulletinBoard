using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Models;

namespace WebApplication2.Data.Repositories
{
    public interface IAnnoucementRepository : IGenericRepository<Annoucement>
    {
        Task<int> DeleteAllFromUser(int id);
        Task UpdateFromDto(AnnoucementPartialUpdateDto annoucement);
    }
}
