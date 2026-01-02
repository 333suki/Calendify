using System.Linq.Expressions;
using Backend;
using Microsoft.EntityFrameworkCore;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DatabaseContext _context;
    private readonly DbSet<T> _dbset;

    public Repository(DatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbset = _context.Set<T>();
    }

    public void Add(T entity)
    {
        if(entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        _dbset.Add(entity);
    }

    public void Update(T entity){
        if(entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        _dbset.Update(entity);
    }


    public void Delete(T entity)
    {
        if(entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        _dbset.Remove(entity);
    }

    public IEnumerable<T> GetAll()
    {
        return _dbset;
    }

    public IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate)
    {
        if(predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }
        return _dbset.Where(predicate);
    }

    public T? GetById(params object[] keyValues)
    {
        if (keyValues is null || keyValues.Length == 0)
        {
            throw new ArgumentException("Must provide key values", nameof(keyValues));
        }

        return _dbset.Find(keyValues);
    }

    public IQueryable<T> Query()
    {
        return _dbset;
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }
}
