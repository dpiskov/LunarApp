using System.Linq.Expressions;

namespace LunarApp.Data.Repository.Interfaces
{
    public interface IRepository<TType, TId>
    {
        TType? GetById(TId id);
        Task<TType?> GetByIdAsync(TId id);
        TType? FirstOrDefault(Func<TType, bool> predicate);
        Task<TType?> FirstOrDefaultAsync(Expression<Func<TType, bool>> predicate);
        IEnumerable<TType> GetAll();
        Task<IEnumerable<TType>> GetAllAsync();
        IQueryable<TType> GetAllAttached();
        void Add(TType item);
        Task AddAsync(TType item);
        void AddRange(TType[] items);
        Task AddRangeAsync(TType[] items);
        bool Delete(TId id);
        Task<bool> DeleteAsync(TId id);
        void DeleteRange(IEnumerable<TType> entities);
        Task DeleteRangeAsync(IEnumerable<TType> entities);
        bool Update(TType item);
        Task<bool> UpdateAsync(TType item);
        void SaveChanges();
        Task SaveChangesAsync();

    }
}
