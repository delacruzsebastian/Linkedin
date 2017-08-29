using DAL.Context;
using DAL.Entities;
using Services.Context;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Prueba1.Context
{
    public class Contexts : BaseContext, IEntitiesContext
    {
        public Contexts() : base(ConfigurationManager.AppSettings["ControlGasV2ConnectionStringName"])
        {
        }

        #region DBSet de entidades

        IDbSet<Person> persons;
        public IDbSet<Person> Persons => persons ?? (persons = base.Set<Person>());

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}