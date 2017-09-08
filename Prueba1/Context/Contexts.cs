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

        IDbSet<User> users;
        public IDbSet<User> Users => users ?? (users = base.Set<User>());

        IDbSet<Country> countries;
        public IDbSet<Country> Countries => countries ?? (countries = base.Set<Country>());

        IDbSet<CountryState> countryStates;
        public IDbSet<CountryState> CountryStates => countryStates ?? (countryStates = base.Set<CountryState>());

        IDbSet<Project> projects;
        public IDbSet<Project> Projects => projects ?? (projects = base.Set<Project>());

        IDbSet<ProjectUser> projectUsers;
        public IDbSet<ProjectUser> ProjectUsers => projectUsers ?? (projectUsers = base.Set<ProjectUser>());

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