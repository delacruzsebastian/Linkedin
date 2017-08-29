using DAL.Context;
using DAL.Unit_of_work;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Respositories
{
    public class BaseRepository<T> : IRepository<T>
              where T : class
    {

        /// <summary>
        /// Unidad de trabajo de este contexto de trabajo
        /// </summary>
        BaseContext context;

        /// <summary>
        /// Unidad de trabajo expuesta a la aplicacion
        /// </summary>
        /// <param name="unitofwork">The current Unit of Work</param>
        public IUnitOfWork Context
        {
            get
            {
                return this.context;
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="context">Unidad de trabajo</param>
        public BaseRepository(BaseContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Metodo generico para recuperar una coleccion de entidades
        /// </summary>
        /// <param name="filter">Expresion para filtrar las entidades</param>
        /// <param name="orderBy">Orden en el que se quiere recuperar las entidades</param>
        /// <param name="includeProperties">Propiedades de Navegacion a incluir</param>
        /// <returns>Un listado de objetos de la entidadgenerica</returns>
        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!String.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual IQueryable<T> GetAll()
        {
            return context.Set<T>();
        }

        /// <summary>
        /// Metodo generico para recuperar una entidad a partir de su identidad
        /// </summary>
        /// <param name="id">La identidad de la entidad</param>
        /// <returns>La entidad</returns>
        public virtual T GetById(object id)
        {
            return context.Set<T>().Find(id);
        }

        /// <summary>
        /// Metodo generico para añadir una entidad al contexto de trabajo
        /// </summary>
        /// <param name="entity">La entidad para añadir</param>
        public virtual void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }

        /// <summary>
        /// Metodo generico para eliminar una entidad del contexto de trabajo
        /// </summary>
        /// <param name="id">La identidad de la entidad</param>
        public virtual void Delete(object id)
        {
            T entityToDelete = context.Set<T>().Find(id);
            Delete(entityToDelete);
        }

        /// <summary>
        /// Metodo generico para eliminar una entidad del contexto de trabajo pasandole la entidad
        /// </summary>
        /// <param name="entityToDelete">Entidad a eliminar</param>
        public virtual void Delete(T entityToDelete)
        {
            context.Attach<T>(entityToDelete);
            context.Set<T>().Remove(entityToDelete);
        }

        /// <summary>
        /// Metodo generico para modificar una entidad en el contexto de trabajo
        /// </summary>
        /// <param name="entityToUpdate">La entidad a modificar</param>
        public virtual void Update(T entityToUpdate)
        {
            context.SetModified<T>(entityToUpdate);
        }

        /// <summary>
        /// Implementacion generica de un metodo para paginar
        /// </summary>
        /// <typeparam name="TKey">Clave para el orden</typeparam>
        /// <param name="pageIndex">Indice de la pagina a recuperar</param>/// 
        /// <param name="pageCount">Numero de entidades a recuperar</param>
        /// <param name="orderByExpression">Order expression</param>
        /// <param name="ascending">Orden ascendente o descendente</param>
        /// <returns>Listado de todas las entidades qeu cumplan los requisitos</returns>
        public IEnumerable<T> GetPagedElements<TKey>(int pageIndex, int pageCount, Expression<Func<T, TKey>> orderByExpression, bool ascending = true)
        {
            if (pageIndex < 1) { pageIndex = 1; }

            if (orderByExpression == (Expression<Func<T, TKey>>)null)
                throw new ArgumentNullException();

            return (ascending)
                            ?
                        context.Set<T>().OrderBy(orderByExpression)
                            .Skip((pageIndex - 1) * pageCount)
                            .Take(pageCount)
                            .ToList()
                            :
                        context.Set<T>().OrderByDescending(orderByExpression)
                            .Skip((pageIndex - 1) * pageCount)
                            .Take(pageCount)
                            .ToList();
        }

        /// <summary>
        /// Implementacion generica de un metodo para paginar
        /// </summary>
        /// <typeparam name="TKey">Clave para el orden</typeparam>
        /// <param name="pageIndex">Indice de la pagina a recuperar</param>/// 
        /// <param name="pageCount">Numero de entidades a recuperar</param>
        /// <param name="orderByExpression">La expresion para establecer el orden</param>
        /// <param name="ascending">Si el orden es ascendente o descendente</param>
        /// <param name="includeProperties">Includes</param>
        /// <returns>Listado con todas las entidades que cumplan los criterios</returns>        
        public IEnumerable<T> GetPagedElements<TKey>(int pageIndex, int pageCount, Expression<Func<T, TKey>> orderByExpression, bool ascending = true, string includeProperties = "")
        {
            IQueryable<T> query = context.Set<T>();

            if (!String.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (pageIndex < 1) { pageIndex = 1; }

            if (orderByExpression == (Expression<Func<T, TKey>>)null)
                throw new ArgumentNullException();

            return (ascending)
                            ?
                        query.OrderBy(orderByExpression)
                            .Skip((pageIndex - 1) * pageCount)
                            .Take(pageCount)
                            .ToList()
                            :
                        query.OrderByDescending(orderByExpression)
                            .Skip((pageIndex - 1) * pageCount)
                            .Take(pageCount)
                            .ToList();
        }

        /// <summary>
        /// Ejecutar una query en la base de datos
        /// </summary>
        /// <param name="sqlQuery">La Query</param>
        /// <param name="parameters">The parameters</param>
        /// <returns>Listado de entidades que recupera la query</returns>
        public IEnumerable<T> GetFromDatabaseWithQuery(string sqlQuery, params object[] parameters)
        {
            return this.context.ExecuteQuery<T>(sqlQuery, parameters);
        }

        /// <summary>
        /// Ejecutar un command en la base de datos 
        /// </summary>
        /// <param name="sqlCommand">La query</param>
        /// <param name="parameters">Los parametros</param>
        /// <returns>El sql code que devuelve la query</returns>
        public int ExecuteInDatabaseByQuery(string sqlCommand, params object[] parameters)
        {
            return this.context.ExecuteCommand(sqlCommand, parameters);
        }



    }
}
