using Prueba1.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Configuration;
using Prueba1.Context;
using Services;
using DAL.Entities;

namespace Prueba1.Controllers
{
    [RoutePrefix("Routing")]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var person = new Person()
            {
                Firstname   = "Carlitosss",
                Surname     = "Tevez",
                Id          = Guid.NewGuid()
            };

            //using (var db = new PersonContext("testDB"))
            //{
            //    List<Person> listPerson = (from currentPerson in db.Persons.ToList()
            //                               select currentPerson
            //                              ).ToList();
            //}

            ViewData["stringInicial1"]  = "Index";
            ViewBag.stringInicial       = "IndexViewBag";
            //string model = "Index";
            return View("Home", new PersonStringViewModel() { myPerson = person, myEnum = Enums.MyLindoEnum.Mandioca });
        }

        [HttpPost]
        public ActionResult Index(PersonStringViewModel person)
        {

            person.myString = "Aca lo asigno";

            var pedro = person.myString;

            return View("Home", person);
        }

        public ActionResult ManualMapping()
        {
            string model = "ManualMapping";
            return PartialView("Home", model);
        }

        [Route("OtraCosa")]
        public ActionResult AttributeMapping()
        {
            string model = "AttributeMapping";
            return PartialView("Home", model);
        }

        public ActionResult LinqTest(/*Person person*/)
        {
            string[] names = { "Burke", "Connor", "Frank",
                       "Everett", "Albert", "George",
                       "Harris", "David" };

            var mysPersonas = new List<Person>();
            var objectList  = new List<object>();
            var dimanic     = new List<dynamic>();
            var anny        = new { MiPropiedad1 = "Hola"};

            var person = new Person()
            {
                Firstname   = "Carlitos",
                Surname     = "Tevez",
                Id          = Guid.NewGuid()
            };

            dimanic.Add(person);
            Person Entero;

            //try
            //{
            //    Entero = new Person() { };
            //}
            //catch (Exception e)
            //{
            //}

            //return Entero;
            mysPersonas.Select(x => x.Firstname);

            
            //for(int personCounter = 0; personCounter < 100; personCounter++)
            //{

            //}

            IEnumerable<string> query = ( 
                                         from currentName in names
                                         where currentName.Length == 5
                                         orderby currentName
                                         select currentName.ToUpper()
                                        );

            //int, float, string, bool

            //if () { }

            //for()

            //foreach () { }

            //while () { }

            return PartialView("Home", query.First());
        }

        public ActionResult PersonServices()
        {
            var persons = new List<Person>();

            try
            {
                using (var context = DbContextFactory.Create())
                {
                    var personService = new PersonService(context);

                    var personGetResult = personService.Get();

                    if (personGetResult.Result == Results.Error || personGetResult.Result == Results.Error)
                    {
                        throw new Exception(personGetResult.ResultErrorMessage);
                    }

                    persons = personGetResult.ResultData;
                }
            }
            catch(Exception e)
            {
                //Manejo de la exception.
            }

            return PartialView("Home");
        }

        public ActionResult NewPerson()
        {
            var persons = new List<Person>();

            try
            {
                using (var context = DbContextFactory.Create())
                {
                    var personService = new PersonService(context);

                    var personGetResult = personService.Get();

                    if (personGetResult.Result == Results.Error || personGetResult.Result == Results.Error)
                    {
                        throw new Exception(personGetResult.ResultErrorMessage);
                    }

                    persons = personGetResult.ResultData;

                    var newPerson = new Person()
                    {
                        Firstname       = "Segundo",
                        Surname         = "Primero",
                        Denomination    = "Primero, segundo",
                        AddDate         = DateTime.Now,
                        UpdateDate      = DateTime.Now
                    };

                    var personAddResult = personService.Add(newPerson);

                    if (personAddResult.Result == Results.Error || personAddResult.Result == Results.Error)
                    {
                        throw new Exception(personAddResult.ResultErrorMessage);
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                //Manejo de la exception.
            }

            return PartialView("Home");
        }

        public ActionResult EditServices()
        {
            var persons = new List<Person>();

            try
            {
                using (var context = DbContextFactory.Create())
                {
                    var personService = new PersonService(context);

                    var personGetResult = personService.Get();

                    if (personGetResult.Result == Results.Error || personGetResult.Result == Results.Error)
                    {
                        throw new Exception(personGetResult.ResultErrorMessage);
                    }

                    persons = personGetResult.ResultData;

                    var updatePerson = persons.FirstOrDefault();

                    updatePerson.Denomination = "Queso, quesito";

                    var personUpdateResult = personService.Update(updatePerson);

                    if (personUpdateResult.Result == Results.Error || personUpdateResult.Result == Results.Error)
                    {
                        throw new Exception(personUpdateResult.ResultErrorMessage);
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                //Manejo de la exception.
            }

            return PartialView("Home");
        }

        public ActionResult DeleteServices()
        {
            var persons = new List<Person>();

            try
            {
                using (var context = DbContextFactory.Create())
                {
                    var personService = new PersonService(context);

                    var personGetResult = personService.GetAll();

                    if (personGetResult.Result == Results.Error || personGetResult.Result == Results.Error)
                    {
                        throw new Exception(personGetResult.ResultErrorMessage);
                    }

                    persons = personGetResult.ResultData;

                    var deletePerson = persons.FirstOrDefault();

                    var personDeleteResult = personService.Delete(deletePerson.Id);

                    if (personDeleteResult.Result == Results.Error || personDeleteResult.Result == Results.Error)
                    {
                        throw new Exception(personDeleteResult.ResultErrorMessage);
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                //Manejo de la exception.
            }

            return PartialView("Home");
        }

        public ActionResult ManualAddAuditServices()
        {
            var persons = new List<Person>();

            try
            {
                using (var context = DbContextFactory.Create())
                {
                    var personService = new PersonService(context);

                    var personGetResult = personService.GetAll();

                    if (personGetResult.Result == Results.Error || personGetResult.Result == Results.Error)
                    {
                        throw new Exception(personGetResult.ResultErrorMessage);
                    }

                    persons = personGetResult.ResultData;

                    var person = persons.FirstOrDefault();

                    var newPersonAudit = new PersonAudit()
                    {
                        AuditDate       = DateTime.Now,
                        Denomination    = "Queso, Quesito",
                        Firstname       = "Quesito",
                        Surname         = "Queso",
                        UserId          = Guid.Empty,
                        Person          = person
                    };

                    person.PersonAudits.Add(newPersonAudit);

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                //Manejo de la exception.
            }

            return PartialView("Home");
        }

    }
}