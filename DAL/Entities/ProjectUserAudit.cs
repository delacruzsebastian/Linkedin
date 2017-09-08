using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ProjectUserAudit
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }

        public DateTime AuditDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Guid AuditUserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
    }
}
