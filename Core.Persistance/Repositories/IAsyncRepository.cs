using Core.Persistance.Dynamic;
using Core.Persistance.Paging;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Core.Persistance.Repositories
{
    public interface IAsyncRepository<TEntity,TEntityId> : IQuery<TEntity>
        where TEntity:Entity<TEntityId>
    {
        Task<TEntity?> GetAsync(
            Expression<Func<TEntity,bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include=null,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default 
            );

        Task<Paginate<TEntity>> GetListAsync(
           Expression<Func<TEntity, bool>>? predicate = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
           bool withDeleted = false,
           int index = 0,
           int size = 10,
           bool enableTracking = true,
           CancellationToken cancellationToken = default 
            );

        Task<Paginate<TEntity>> GetListByDynamicAsync(
           DynamicQuery dynamic,
           Expression<Func<TEntity, bool>>? predicate = null,
           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
           bool withDeleted = false,
           int index = 0,
           int size = 10,
           bool enableTracking = true,
           CancellationToken cancellationToken = default
       );

        Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            bool enableTracking = true,
            bool withDeleted = false,
            CancellationToken cancellationToken = default
            );

        Task<TEntity> AddAsync(TEntity entity);

        //Task<IList<TEntity>> AddRangeAsync(IList<TEntity> entity);
        Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities);

        //Task<IList<TEntity>> UpdateRangeAsync(IList<TEntity> entity);

        Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false);

        //Task<IList<TEntity>> DeleteRangeAsync(IList<TEntity> entity);

        Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool permanent = false);
    }
}
