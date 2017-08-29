
namespace DAL.Context
{
    using EntityFramework.DynamicFilters;
    using DAL.Interfaces;
    using DAL.Unit_of_work;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;
    public class BaseContext : DbContext, IEntityFrameworkUnitOfWork
    {
        protected BaseContext(string connectionStringName)
            : base(connectionStringName)
        {
            if (string.IsNullOrEmpty(connectionStringName))
            {
                throw new ArgumentNullException("connectionStringName");
            }

            this.Configuration.LazyLoadingEnabled = true;

            int timeout;
            this.Database.CommandTimeout = int.TryParse(
                ConfigurationManager.AppSettings["DbContextCommandTimeout"],
                out timeout)
                                               ? timeout
                                               : 60;
        }

        private void AddMyFilters(ref DbModelBuilder modelBuilder)
        {
            modelBuilder.Filter(DAL.Constants.Constants.FILTER_SOFT_DELETED, (ISoftDeleted d) => d.DeleteDate, null);

            // modelBuilder.Filter(Constants.FILTER_TENANT, (ITenant d) => d.TenantId, new Guid("A220B4CD-1C5A-4C51-9BF5-5BE43BE3B1F6"));
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            this.AddMyFilters(ref modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        //public void DisableFilterx(string filterName)
        //{
        //    DisableFilter(filterName);
        //}

        //public void EnableFilter(string filterName)
        //{
        //    this.EnableFilter(filterName);
        //}

        #region Implementación de IEntityFrameworkUnitOfWork

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public void Attach<TEntity>(TEntity entity) where TEntity : class
        {
            if (base.Entry<TEntity>(entity).State == EntityState.Detached)
            {
                base.Set<TEntity>().Attach(entity);
            }
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return base.Database.SqlQuery<TEntity>(sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return base.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        #endregion

        #region Implementación de IUnitOfWork

        public void SetModified<TEntity>(TEntity entity) where TEntity : class
        {
            base.Entry<TEntity>(entity).State = EntityState.Modified;
        }

        public void Commit()
        {
            base.SaveChanges();
        }

        public void CommitAndRefreshChanges()
        {
            bool saveFailed = false;

            do
            {
                try
                {
                    base.SaveChanges();

                    saveFailed = false;

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList()
                              .ForEach(entry =>
                              {
                                  entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                              });
                }
            } while (saveFailed);
        }

        public void Rollback()
        {
            base.ChangeTracker.Entries()
                              .ToList()
                              .ForEach(entry => entry.State = EntityState.Unchanged);
        }

        #endregion

    }
}
