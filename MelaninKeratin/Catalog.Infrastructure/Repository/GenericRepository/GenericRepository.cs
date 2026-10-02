using Catalog.Application.Interfaces.IGenericRepository;
using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Catalog.Infrastructure.Repository.GenericRepository
{ 
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly CatalogDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }


        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }


        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.Where(expression).ToListAsync();
        }


        public async Task<T> FindSingleAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }


        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }


        public async Task<T> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async void SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }


    }



}
