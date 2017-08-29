using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Respositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Unit_of_work;
    public interface IRepository<T>

    {
        /// <summary>
        /// Traemos la Unit of Work al repositorio para así poder hacer Commit, Rollback ... de los cambios
        /// </summary>
        IUnitOfWork Context { get; }
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        T GetById(object id);
        void Add(T entity);
        void Delete(object id);
        void Delete(T entityToDelete);
        void Update(T entityToUpdate);
        IEnumerable<T> GetPagedElements<TKey>(int pageIndex, int pageCount, Expression<Func<T, TKey>> orderByExpression, bool ascending = true);
        IEnumerable<T> GetPagedElements<TKey>(int pageIndex, int pageCount, Expression<Func<T, TKey>> orderByExpression, bool ascending = true, string includeProperties = "");
        IEnumerable<T> GetFromDatabaseWithQuery(string sqlQuery, params object[] parameters);
        int ExecuteInDatabaseByQuery(string sqlCommand, params object[] parameters);

        IQueryable<T> GetAll();

    }
}
