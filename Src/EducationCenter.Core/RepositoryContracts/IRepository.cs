using System;
using System.Linq.Expressions;

namespace EducationCenter.Core.RepositoryContracts;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(int id);
    Task<List<TEntity>> GetAllAsync();
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, string[]? includes = null);
    Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>>? predicate, string[]? includes = null);
    Task<List<TEntity>> FindAllAsync(int page, int pageSize, Expression<Func<TEntity, bool>>? predicate, Expression<Func<TEntity, object>>? orderKeySelector, string? sortOrder, string[]? includes = null);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate);
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}