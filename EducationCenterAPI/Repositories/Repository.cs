using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace EducationCenterAPI.RepositoryContracts;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected DbContext _dbContext;
    public Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void Add(TEntity entity)
    {
        _dbContext.Set<TEntity>().Add(entity);
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, string[]? includes = null)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>().AsQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.FirstOrDefaultAsync(predicate);
    }
    public async Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>>? predicate, string[]? includes = null)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>().AsQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        if (predicate is not null)
            return await query.Where(predicate).ToListAsync();
        return await query.ToListAsync();
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate)
    {
        if (predicate is not null)
            return await _dbContext.Set<TEntity>().CountAsync(predicate);
        return await _dbContext.Set<TEntity>().CountAsync();
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _dbContext.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id).AsTask();
    }

    public void Remove(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
    }

    public void Update(TEntity entity)
    {
        _dbContext.Update(entity);
    }

    public async Task<List<TEntity>> FindAllAsync(int page, int pageSize, Expression<Func<TEntity, bool>>? predicate, Expression<Func<TEntity, object>>? orderKeySelector, string? sortOrder, string[]? includes = null)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>().AsQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (orderKeySelector is not null)
        {
            if (sortOrder?.ToLower() == "asc")
            {
                query = query.OrderBy(orderKeySelector);
            }
            else
            {
                query = query.OrderByDescending(orderKeySelector);
            }
        }
        if (predicate is not null)
            return await query.Where(predicate).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().AddRange(entities);
    }
}
