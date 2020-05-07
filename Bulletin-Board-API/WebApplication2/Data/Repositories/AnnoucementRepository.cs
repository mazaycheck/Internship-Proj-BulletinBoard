using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Models;

namespace WebApplication2.Data.Repositories
{
    public class AnnoucementRepository : IAnnoucementRepository
    {
        private readonly AppDbContext _context;

        public AnnoucementRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task Create(Annoucement entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.ExpirationDate = DateTime.Now.AddDays(30);
            entity.IsActive = true;
            await _context.Annoucements.AddAsync(entity);       
        }

        public async Task Delete(Annoucement annoucement)
        {     
            _context.Annoucements.Remove(annoucement);
            return;
        }

        public IQueryable<Annoucement> GetAll()
        {
            return _context.Annoucements.AsNoTracking()
                .Include(a => a.BrandCategory)
                    .ThenInclude(b => b.Brand)
                .Include(a => a.BrandCategory)
                    .ThenInclude(c => c.Category)
                .Include(u => u.User)
                    .ThenInclude(t => t.Town)
                .Include(p => p.Photos);
        }
    
        public async Task<Annoucement> GetById(int id)
        {            
            return await GetAll().SingleOrDefaultAsync(p=> p.AnnoucementId == id);
        }

        public async Task Update(Annoucement annoucementFromUser)
        {
            Annoucement annoucementFromDb = _context.Annoucements.AsNoTracking().Include(p => p.Photos).SingleOrDefault(x =>  x.AnnoucementId == annoucementFromUser.AnnoucementId);
            if(annoucementFromDb == null)
            {
                throw new NullReferenceException("No annoucement with id:" + annoucementFromUser.AnnoucementId);
            }
            annoucementFromUser.CreateDate = annoucementFromDb.CreateDate;
            annoucementFromUser.ExpirationDate = annoucementFromDb.ExpirationDate;
            annoucementFromUser.Photos = annoucementFromDb.Photos;
            _context.Update(annoucementFromUser);
            //context.Entry(annoucementFromDb).CurrentValues.SetValues(annoucementFromUser);
        }

        public async Task UpdateFromDto(AnnoucementPartialUpdateDto annoucementDto)
        {
            var annoucementFromDb = await _context.Annoucements.FindAsync(annoucementDto.AnnoucementId);
            _context.Entry(annoucementFromDb).CurrentValues.SetValues(annoucementDto);
        }

        public async Task<int> Save()
        {
            var rowcount = await _context.SaveChangesAsync();
            return rowcount;
        }

        public async Task<int> DeleteAllFromUser(int id)
        {
            
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    //var annoucementsToDelete = await _context.Annoucements.Where(x => x.UserId == id).ToListAsync();
                    var rowcount = _context.Database.ExecuteSqlInterpolated($"DELETE FROM Annoucements WHERE UserId = {id}");
                    await trans.CommitAsync();
                    return rowcount;
                }
                catch (Exception e)
                {
                    await trans.RollbackAsync();
                    return 0;
                }
            }                                    
        }

        public async Task<bool> Exists(int entityId)
        {
            return await _context.Annoucements.AnyAsync(p => p.AnnoucementId == entityId);
        }

        public IQueryable<Annoucement> GetByIdQueryable(int id)
        {
            return _context.Annoucements.Where(x => x.AnnoucementId == id);
        }

   
    }
}
