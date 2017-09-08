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
            try
            {
                if (ModelState.IsValid)
                { 
                    using (var context = DbContextFactory.Create())
                    {
                        var userService     = new UserService(context);
                        var userAddResult   = userService.Add(posteado);

                        if (userAddResult.Result == Results.Error || userAddResult.Result == Results.Error)
                        {
                            throw new Exception(userAddResult.ResultErrorMessage);
                        }

                        context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                //Manejo de la exception.
            }
            return View(posteado);
        }

        public ActionResult Test()
        {
            //var user = new User()
            //{
            //    Available       = true,
            //    Denomination    = "MyUsuario",
            //    Firstname       = "My",
            //    Surname         = "Usuario",
            //    Sex             = DAL.Enums.Sex.Femenino,
            //    Password        = "12345678",
            //    Username        = "MyUsuarioName",
            //    Email           = "myEmail@dsl.com"
            //};

            try
            {
                using (var context = DbContextFactory.Create())
                {

                    var userService = new CountryService(context);
                    var userAddResult = userService.Get();
                    var userServic2e = new CountryStateService(context);
                    var userAddResul2t = userServic2e.Get();
                    //var userAddResult = userService.Add(user);

                    if (userAddResult.Result == Results.Error || userAddResult.Result == Results.Error)
                    {
                        throw new Exception(userAddResult.ResultErrorMessage);
                    }

                    //context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                //Manejo de la exception.
            }
            return View();
        }
    }
}