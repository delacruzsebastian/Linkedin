using DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    [Table("UserAudits")]
    public class UserAudit
    {
        [Column("Id"), Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AuditId { get; set; }

        [Column("UserId"), Required]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Firstname { get; set; }

        [StringLength(50)]
        //[Display(Name = "LastName")]
        public string Surname { get; set; }
        public string Denomination { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Sex Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public Boolean Available { get; set; }
        public Guid? CountryId { get; set; }

        [Required]
        public Guid AuditUserId { get; set; }

        [Required]
        public DateTime AuditDate { get; set; }

        public DateTime? DeleteDate { get; set; }

        [ForeignKey("Id")]
        public virtual User User { get; set; }
    }
}
