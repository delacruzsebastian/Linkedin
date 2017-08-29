using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prueba1.Models
{
    [Table("Person")]
    public class Person
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Firstname { get; set; }

        [StringLength(50)]
        //[Display(Name = "LastName")]
        public string Surname { get; set; }
    }
}