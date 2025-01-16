using Cashify.Application.Interfaces.Dependency;
using Cashify.Domain.Common.Base;
using Cashify.Domain.Models;

namespace Cashify.Application.Interfaces.Repository;

public interface IGenericRepository : ITransientService
{
    TEntity GetById<TEntity>(Guid id) where TEntity : BaseEntity;
    
    int GetCount<TEntity>() where TEntity : BaseEntity;
    
    TEntity? GetFirstOrDefault<TEntity>(Func<TEntity, bool> predicate) where TEntity : BaseEntity;
    
    List<TEntity> GetAll<TEntity>(Func<TEntity, bool>? predicate = null) where TEntity : BaseEntity; 

    Task Insert<TEntity>(TEntity entity, bool isCreatedByRequired = true) where TEntity : BaseEntity;

    Task Update<TEntity>(TEntity entity) where TEntity : BaseEntity;

    void Delete<TEntity>(TEntity entity) where TEntity : BaseEntity;
}