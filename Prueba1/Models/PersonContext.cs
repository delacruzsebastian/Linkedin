using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Prueba1.Models
{
    public class PersonContext : DbContext
    {
        public PersonContext(string connectionStringName) : base(connectionStringName)
        {
            if (string.IsNullOrEmpty(connectionStringName))
            {
                throw new ArgumentNullException("connectionStringName");
            }

            this.Configuration.LazyLoadingEnabled = true;

            int timeout;
            this.Database.CommandTimeout = int.TryParse(ConfigurationManager.AppSettings["DbContextCommandTimeout"], out timeout)
                                               ? timeout
                                               : 60;
        }

        public DbSet<Person> Persons { get; set; }
    }
}