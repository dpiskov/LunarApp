using LunarApp.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Data.Repository
{
    public class BaseRepository<TType, TId> : IRepository<TType, TId> where TType : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TType> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TType>();
        }
        public TType GetById(TId id)
        {
            TType entity = _dbSet.Find(id);

            return entity;
        }

        public async Task<TType> GetByIdAsync(TId id)
        {
            TType entity = await _dbSet.FindAsync(id);

            return entity;
        }

        public IEnumerable<TType> GetAll()
        {
            return _dbSet.ToArray();
        }

        public async Task<IEnumerable<TType>> GetAllAsync()
        {
            return await _dbSet.ToArrayAsync();
        }

        public IQueryable<TType> GetAllAttached()
        {
            return _dbSet.AsQueryable();
        }

        public void Add(TType item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
        }

        public async Task AddAsync(TType item)
        {
            await _dbSet.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public void AddRange(TType[] items)
        {
            _dbSet.AddRange(items);
            _context.SaveChanges();
        }

        public async Task AddRangeAsync(TType[] items)
        {
            await _dbSet.AddRangeAsync(items);
            await _context.SaveChangesAsync();
        }

        public bool Delete(TId id)
        {
            TType entity = GetById(id);
            if (entity is null)
            {
                return false;
            }

            _dbSet.Remove(entity);
            _context.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteAsync(TId id)
        {
            TType entity = await GetByIdAsync(id);
            if (entity is null)
            {
                return false;
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool Update(TType item)
        {
            try
            {
                _dbSet.Attach(item);
                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TType item)
        {
            try
            {
                _dbSet.Attach(item);
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
