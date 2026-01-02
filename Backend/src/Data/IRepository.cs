using System.Linq.Expressions;

public interface IRepository<T> where T : class
{
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    IEnumerable<T> GetAll();
    IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate);
    T? GetById(params object[] keyValues);
    IQueryable<T> Query();
    int SaveChanges();
}