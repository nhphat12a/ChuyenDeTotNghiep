using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aristino.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByWhereAsync(int id);
        Task<T> AddEntity(T entity);
        Task<bool> UpdateEntity(T entity);
        Task<bool>DeleteEntity(T entity);
        DbSet<T> GetAll();
        IActionResult Notfound(int? id, T entity);
        void SaveChanges();
    }
}
