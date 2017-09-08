using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    [Table("ProjectAudits")]
    public class ProjectAudit
    {
        public Guid Id { get; set; }
        public string Denomination { get; set; }

        [Required]
        public Guid AuditUserId { get; set; }

        [Required]
        public DateTime AuditDate { get; set; }

        public DateTime? DeleteDate { get; set; }

        [ForeignKey("Id")]
        public virtual Project Project { get; set; }
    }
}
