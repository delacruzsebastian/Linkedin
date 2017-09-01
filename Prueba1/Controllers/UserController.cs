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
        public ActionResult Create(User post)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var context = DbContextFactory.Create())
                    {
                        var userService = new UserService(context);

                        var userGetResult = userService.Get();

                        if (userGetResult.Result == Results.Error || userGetResult.Result == Results.Error)
                        {
                            throw new Exception(userGetResult.ResultErrorMessage);
                        }

                        var users = userGetResult.ResultData;

                        var newUser = new User()
                        {
                            Firstname = post.Firstname,
                            Surname = post.Surname,
                            Denomination = post.Denomination,
                            Username = post.Username,
                            Email = post.Email,
                            Password = post.Password,
                            Sex = post.Sex,
                            BirthDate = post.BirthDate,
                            AddDate = DateTime.Now,
                            UpdateDate = DateTime.Now
                        };



                        var userAddResult = userService.Add(newUser);

                        if (userAddResult.Result == Results.Error || userAddResult.Result == Results.Error)
                        {
                            throw new Exception(userAddResult.ResultErrorMessage);
                        }

                        context.SaveChanges();
                    }
                }
                else
                {
                    ViewBag.error = "Los datos que mandaste no coinciden con el tipo User. Estuviste cerca.";
                    return View("Create");
                }
            }
            catch (Exception e)
            {
                //Manejo de la exception.
            }

            return View(post);
        }
    }
}