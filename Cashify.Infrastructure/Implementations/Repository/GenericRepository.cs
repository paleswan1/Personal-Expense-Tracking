using Cashify.Domain.Common.Base;
using Cashify.Application.Utility;
using Cashify.Application.Interfaces.Managers;
using Cashify.Application.Interfaces.Repository;
using Cashify.Application.Interfaces.Utility;

namespace Cashify.Infrastructure.Implementations.Repository;

public class GenericRepository(ISerializeDeserializeManager serializeDeserializeManager, IUserService userService) : IGenericRepository
{
    public TEntity GetById<TEntity>(Guid id) where TEntity : BaseEntity
    {
        try
        {
            var filePath = CreateEntity<TEntity>().ToFilePath();
            
            var entities = GetAll<TEntity>(filePath);

            return entities.FirstOrDefault(e => e.Id == id)
                   ?? throw new Exception($"Entity of type {typeof(TEntity).Name} with identifier {id} could not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw new Exception(ex.Message);
        }
    }

    public int GetCount<TEntity>() where TEntity : BaseEntity
    {
        try
        {
            var filePath = CreateEntity<TEntity>().ToFilePath();
            
            var entities = GetAll<TEntity>(filePath);

            return entities.Count;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw new Exception(ex.Message);
        }
    }

    public TEntity? GetFirstOrDefault<TEntity>(Func<TEntity, bool> predicate) where TEntity : BaseEntity
    {
        try
        {
            var filePath = CreateEntity<TEntity>().ToFilePath();
            
            var entities = GetAll<TEntity>(filePath);

            return entities.FirstOrDefault(predicate);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw new Exception(ex.Message);
        }
    }
    
    public List<TEntity> GetAll<TEntity>() where TEntity : BaseEntity
    {
        try
        {
            var filePath = CreateEntity<TEntity>().ToFilePath();
            
            var entities = GetAll<TEntity>(filePath);

            return entities;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw new Exception(ex.Message);
        }
    }

    public async Task Insert<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        try
        {
            var filePath = entity.ToFilePath();
            
            var entities = GetAll<TEntity>(filePath);

            var userId = await userService.GetUserId();

            entity.CreatedBy = userId;
            entity.CreatedDate = DateTime.Now;
            
            entities.Add(entity);
            
            SaveAll(filePath, entities);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw new Exception(ex.Message);
        }
    }

    public async Task Update<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        try
        {
            var filePath = entity.ToFilePath();
            
            var entities = GetAll<TEntity>(filePath);

            var entityIndex = entities.FindIndex(e => e.Id == entity.Id);
            
            if (entityIndex == -1)
                throw new Exception($"Entity of type {typeof(TEntity).Name} with the provided identifier could not be found.");

            var userId = await userService.GetUserId();

            entity.LastUpdatedBy = userId;
            entity.LastUpdatedDate = DateTime.Now;

            entities[entityIndex] = entity;

            SaveAll(filePath, entities);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw new Exception(ex.Message);
        }
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        try
        {
            var filePath = entity.ToFilePath();
            
            var entities = GetAll<TEntity>(filePath);
            
            entities = entities.Where(e => e.Id != entity.Id).ToList();
            
            SaveAll(filePath, entities);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw new Exception(ex.Message);
        }
    }
    
    private TEntity? CreateEntity<TEntity>() where TEntity : BaseEntity
    {
        return Activator.CreateInstance(typeof(TEntity)) as TEntity;
    }
    
    private List<TEntity> GetAll<TEntity>(string filePath) where TEntity : BaseEntity
    {
        if (!File.Exists(filePath)) return [];
        
        var json = File.ReadAllText(filePath);
        
        return serializeDeserializeManager.Deserialize<TEntity>(json);
    }

    private void SaveAll<TEntity>(string filePath, List<TEntity> entities) where TEntity : BaseEntity
    {
        var json = serializeDeserializeManager.Serialize(entities);
        
        File.WriteAllText(filePath, json);
    }
}