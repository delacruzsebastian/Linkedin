using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    [Table("CountryStates")]
    public class CountryState : IKey
    {
        public Guid Id { get; set; }

        public Guid CountryId { get; set; }

        [Required]
        [StringLength(256)]
        public string Denomination { get; set; }
        
        public virtual Country Country { get; set; }
    }

}
