using System.Linq.Expressions;

namespace Core.Shared
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
         Task<TEntity?> GetByIdAsync(Guid id);
         Task<TEntity> InsertAsync(TEntity entity);
         Task<TEntity> UpdateAsync(TEntity entity);
         Task<TEntity> DeleteAsync(TEntity entity);
         Task<IQueryable<TEntity>> GetRangeAsync(Expression<Func<TEntity,bool>>? expression =null, int pageNumber=1,int pageSize=int.MaxValue);
         
         

    }
}
