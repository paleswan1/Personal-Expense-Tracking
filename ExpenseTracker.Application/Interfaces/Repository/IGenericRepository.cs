using ExpenseTracker.Application.Interfaces.Dependency;
using ExpenseTracker.Domain.Common.Base;
using ExpenseTracker.Domain.Models;

namespace ExpenseTracker.Application.Interfaces.Repository;

public interface IGenericRepository : ITransientService
{
    TEntity GetById<TEntity>(Guid id) where TEntity : BaseEntity;
    
    int GetCount<TEntity>() where TEntity : BaseEntity;
    
    TEntity? GetFirstOrDefault<TEntity>(Func<TEntity, bool> predicate) where TEntity : BaseEntity;
    
    List<TEntity> GetAll<TEntity>() where TEntity : BaseEntity; 

    Task Insert<TEntity>(TEntity entity) where TEntity : BaseEntity;

    Task Update<TEntity>(TEntity entity) where TEntity : BaseEntity;

    void Delete<TEntity>(TEntity entity) where TEntity : BaseEntity;
}