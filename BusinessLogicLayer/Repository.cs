using BusinessLogicLayer.Interfaces;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<TEntity> _dbSet;
        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = _appDbContext.Set<TEntity>();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
            {
                throw new Exception("Not found or previously deleted");
            }

            _dbSet.Remove(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            return entity;
        }

        public void SetForInsert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public async Task InsertAsync(TEntity entity)
        {
            SetForInsert(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public IQueryable<TEntity> Query()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<TEntity> GetQueryPaged(int currentPage, int recordsPerPage)
        {
            var query = _appDbContext.Set<TEntity>().
            Skip((currentPage - 1) * recordsPerPage).
            Take(recordsPerPage);

            return query;
        }

        public void SetForUpdate(TEntity entity)
        {
            _appDbContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            SetForUpdate(entity);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
