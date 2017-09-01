using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Entities;
using Prueba1.Context;
using Services;

namespace Prueba1.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Create()
        {
            return View("Create");
        }
        
        [HttpPost]
        public ActionResult Create(User posteado)
        {
            return View(posteado);
        }
    }
}