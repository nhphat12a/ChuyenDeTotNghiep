using Aristino.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace Aristino.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private AristinoDbContext _context;
        DbSet<T> _DbSet;
        public Repository(AristinoDbContext context)
        {
            _context = context;
            _DbSet = _context.Set<T>();
        }

        public async Task<T> AddEntity(T entity)
        {
            await _DbSet.AddAsync(entity);
            return entity;
        }

        public async Task<bool> DeleteEntity(T entity)
        {
            _DbSet.Remove(entity);
            return true;
        }

        public DbSet<T> GetAll()
        {
            return _DbSet;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _DbSet.ToListAsync();
        }

        public async Task<T> GetByWhereAsync(int id)
        {
            return await _DbSet.FindAsync(id);
        }

        public IActionResult Notfound(int? id, T entity)
        {
            NotFoundResult result = new();
            if (id == null || !_DbSet.Contains(entity))
            {
                return result;
            }
            return result;
        }

        public async Task<bool> UpdateEntity(T entity)
        {
            //là khi ta truyền entity vào _dbset.attach, là ta truyền 1 model rỗng, và ở dưới ta sẽ gắn thủ công từng value cho từng cột trong db, sau gọi savechange thì mới lưu vào db
            _DbSet.Attach(entity); //
            _context.Entry(entity).State = EntityState.Modified;
            //_DbSet.Update(entity);
            return true;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

    }
}
