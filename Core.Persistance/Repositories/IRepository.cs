using Core.Persistance.Dynamic;
using Core.Persistance.Paging;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistance.Repositories
{
    public interface IRepository<TEntity, TEntityId> : IQuery<TEntity>
     where TEntity: Entity<TEntityId>
    {
        TEntity? Get(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool enableTracking = true
        );

        Paginate<TEntity> GetList(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int index = 0,
            int size = 10,
            bool enableTracking = true
        );

        Paginate<TEntity> GetListByDynamic(
            DynamicQuery dynamic,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int index = 0,
            int size = 10,
            bool enableTracking = true
        );

        bool Any(Expression<Func<TEntity, bool>>? predicate = null, bool enableTracking = true);
        TEntity Add(TEntity entity);
        IList<TEntity> AddRange(IList<TEntity> entities);
        TEntity Update(TEntity entity);
        IList<TEntity> UpdateRange(IList<TEntity> entities);
        TEntity Delete(TEntity entity);
        IList<TEntity> DeleteRange(IList<TEntity> entity);
    }
}
