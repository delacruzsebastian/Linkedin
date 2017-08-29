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
    [Table("PersonAudits")]
    public class PersonAudit
    {
        [Column("Id"), Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AuditId { get; set; }

        [Column("PersonId"), Required]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Firstname { get; set; }

        [StringLength(50)]
        //[Display(Name = "LastName")]
        public string Surname { get; set; }

        public string Denomination { get; set; }


        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime AuditDate { get; set; }

        public DateTime? DeleteDate { get; set; }

        [ForeignKey("Id")]
        public virtual Person Person { get; set; }
    }
}
