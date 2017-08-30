using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("Person")]
    public class Person : IHeadData, ISoftDeleted
    {
        public Person()
        {
            this.PersonAudits = new List<PersonAudit>();
        }
        
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Firstname { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        public string Denomination { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime? DeleteDate { get; set; }

        public virtual ICollection<PersonAudit> PersonAudits { get; set; }

    }
}
