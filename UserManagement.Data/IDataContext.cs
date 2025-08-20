using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManagement.Data;

public interface IDataContext
{
    /// <summary>
    /// Get a list of items
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns> //added this so that a property can set to no tracking

    Task<List<TEntity>> GetAll<TEntity>() where TEntity : class;

    Task<TEntity?> GetById<TEntity>(long id) where TEntity : class;

    /// <summary>
    /// Create a new item
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task Create<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Uodate an existing item matching the ID
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task Update<TEntity>(TEntity entity) where TEntity : class;

    Task Delete<TEntity>(TEntity entity) where TEntity : class;
}
