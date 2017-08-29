using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.Context;

namespace Prueba1.Context
{
    public class DbContextFactory
    {
        public static BaseContext Create()
        {
            return new Contexts();
        }
    }
}