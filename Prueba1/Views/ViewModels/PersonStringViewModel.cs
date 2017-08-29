using DAL.Entities;
using Prueba1.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prueba1.Views.ViewModels
{
    public class PersonStringViewModel
    {
        public Person myPerson { get; set; }
        public string myString {
            get {
                var fullName = String.Format("{0}, {1}", this.myPerson.Firstname, this.myPerson.Surname);

                return fullName;
            }
            set {
                this.myString = value;
            }
        }

        public MyLindoEnum myEnum { get; set; }

    }
}