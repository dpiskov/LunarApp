using LunarApp.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LunarApp.Data.Repository
{
    /// <summary>
    /// Provides a base implementation for the <see cref="IRepository{TType, TId}"/> interface,
    /// enabling common data access operations for a specific entity type.
    /// </summary>
    /// <typeparam name="TType">The type of the entity managed by the repository.</typeparam>
    /// <typeparam name="TId">The type of the entity's identifier.</typeparam>
    public class BaseRepository<TType, TId> : IRepository<TType, TId> where TType : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TType> _dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{TType, TId}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TType>();
        }
        /// <inheritdoc/>
        public TType? GetById(TId id)
        {
            TType? entity = _dbSet.Find(id);

            return entity;
        }
        /// <inheritdoc/>
        public async Task<TType?> GetByIdAsync(TId id)
        {
            TType? entity = await _dbSet.FindAsync(id);

            return entity;
        }
        /// <inheritdoc/>
        public TType? FirstOrDefault(Func<TType, bool> predicate)
        {
            TType? entity = _dbSet
                .FirstOrDefault(predicate);

            return entity;
        }
        /// <inheritdoc/>
        public async Task<TType?> FirstOrDefaultAsync(Expression<Func<TType, bool>> predicate)
        {
            TType? entity = await _dbSet
                .FirstOrDefaultAsync(predicate);

            return entity;
        }
        /// <inheritdoc/>
        public IEnumerable<TType> GetAll()
        {
            return _dbSet.ToArray();
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<TType>> GetAllAsync()
        {
            return await _dbSet.ToArrayAsync();
        }
        /// <inheritdoc/>
        public IQueryable<TType> GetAllAttached()
        {
            return _dbSet.AsQueryable();
        }
        /// <inheritdoc/>
        public void Add(TType item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
        }
        /// <inheritdoc/>
        public async Task AddAsync(TType item)
        {
            await _dbSet.AddAsync(item);
            await _context.SaveChangesAsync();
        }
        /// <inheritdoc/>
        public void AddRange(TType[] items)
        {
            _dbSet.AddRange(items);
            _context.SaveChanges();
        }
        /// <inheritdoc/>
        public async Task AddRangeAsync(TType[] items)
        {
            await _dbSet.AddRangeAsync(items);
            await _context.SaveChangesAsync();
        }
        /// <inheritdoc/>
        public bool Delete(TId id)
        {
            TType? entity = GetById(id);
            if (entity is null)
            {
                return false;
            }

            _dbSet.Remove(entity);
            _context.SaveChanges();

            return true;
        }
        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(TId id)
        {
            TType? entity = await GetByIdAsync(id);
            if (entity is null)
            {
                return false;
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
        /// <inheritdoc/>
        public void DeleteRange(IEnumerable<TType> entities)
        {
            //if (entities == null)
            //{
            //    throw new ArgumentNullException(nameof(entities), "The entities collection cannot be null.");
            //}

            var entityList = entities.ToList();

            //if (!entityList.Any())
            //{
            //    throw new ArgumentException("The entities collection cannot be empty.", nameof(entities));
            //}

            _dbSet.RemoveRange(entityList);
            _context.SaveChanges();
        }
        /// <inheritdoc/>
        public async Task DeleteRangeAsync(IEnumerable<TType> entities)
        {
            //if (entities == null)
            //{
            //    throw new ArgumentNullException(nameof(entities), "The entities collection cannot be null.");
            //}

            var entityList = entities.ToList();

            //if (!entityList.Any())
            //{
            //    throw new ArgumentException("The entities collection cannot be empty.", nameof(entities));
            //}

            _dbSet.RemoveRange(entityList);
            await _context.SaveChangesAsync();
        }
        /// <inheritdoc/>
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
        /// <inheritdoc/>
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
        /// <inheritdoc/>
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        /// <inheritdoc/>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
