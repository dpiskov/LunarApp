using System.Linq.Expressions;

namespace LunarApp.Data.Repository.Interfaces
{
    /// <summary>
    /// Defines the contract for a repository that provides generic data access operations.
    /// </summary>
    /// <typeparam name="TType">The type of the entity managed by the repository.</typeparam>
    /// <typeparam name="TId">The type of the entity's identifier.</typeparam>
    public interface IRepository<TType, TId>
    {
        /// <summary>
        /// Retrieves an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to retrieve.</param>
        /// <returns>The entity if found; otherwise, <c>null</c>.</returns>
        TType? GetById(TId id);
        /// <summary>
        /// Asynchronously retrieves an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the entity if found; otherwise, <c>null</c>.</returns>
        Task<TType?> GetByIdAsync(TId id);
        /// <summary>
        /// Retrieves the first entity matching the specified predicate.
        /// </summary>
        /// <param name="predicate">The function used to filter the entities.</param>
        /// <returns>The first matching entity, or <c>null</c> if no match is found.</returns>
        TType? FirstOrDefault(Func<TType, bool> predicate);
        /// <summary>
        /// Asynchronously retrieves the first entity matching the specified predicate.
        /// </summary>
        /// <param name="predicate">The expression used to filter the entities.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the first matching entity, or <c>null</c> if no match is found.</returns>
        Task<TType?> FirstOrDefaultAsync(Expression<Func<TType, bool>> predicate);
        /// <summary>
        /// Retrieves all entities from the repository.
        /// </summary>
        /// <returns>A collection of all entities.</returns>
        IEnumerable<TType> GetAll();
        /// <summary>
        /// Asynchronously retrieves all entities from the repository.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains a collection of all entities.</returns>
        Task<IEnumerable<TType>> GetAllAsync();
        /// <summary>
        /// Retrieves all entities as an <see cref="IQueryable{T}"/> for queryable operations.
        /// </summary>
        /// <returns>An <see cref="IQueryable{T}"/> representing the entities.</returns>
        IQueryable<TType> GetAllAttached();
        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        /// <param name="item">The entity to add.</param>
        void Add(TType item);
        /// <summary>
        /// Asynchronously adds a new entity to the repository.
        /// </summary>
        /// <param name="item">The entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(TType item);
        /// <summary>
        /// Adds a range of entities to the repository.
        /// </summary>
        /// <param name="items">The array of entities to add.</param>
        void AddRange(TType[] items);
        /// <summary>
        /// Asynchronously adds a range of entities to the repository.
        /// </summary>
        /// <param name="items">The array of entities to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddRangeAsync(TType[] items);
        /// <summary>
        /// Deletes an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to delete.</param>
        /// <returns><c>true</c> if the entity was successfully deleted; otherwise, <c>false</c>.</returns>
        bool Delete(TId id);
        /// <summary>
        /// Asynchronously deletes an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to delete.</param>
        /// <returns>A task representing the asynchronous operation. The task result is <c>true</c> if the entity was successfully deleted; otherwise, <c>false</c>.</returns>
        Task<bool> DeleteAsync(TId id);
        /// <summary>
        /// Deletes a range of entities from the repository.
        /// </summary>
        /// <param name="entities">The collection of entities to delete.</param>
        void DeleteRange(IEnumerable<TType> entities);
        /// <summary>
        /// Asynchronously deletes a range of entities from the repository.
        /// </summary>
        /// <param name="entities">The collection of entities to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteRangeAsync(IEnumerable<TType> entities);
        /// <summary>
        /// Updates an existing entity in the repository.
        /// </summary>
        /// <param name="item">The entity to update.</param>
        /// <returns><c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
        bool Update(TType item);
        /// <summary>
        /// Asynchronously updates an existing entity in the repository.
        /// </summary>
        /// <param name="item">The entity to update.</param>
        /// <returns>A task representing the asynchronous operation. The task result is <c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
        Task<bool> UpdateAsync(TType item);
        /// <summary>
        /// Saves any changes made to the repository.
        /// </summary>
        void SaveChanges();
        /// <summary>
        /// Asynchronously saves any changes made to the repository.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SaveChangesAsync();
    }
}
