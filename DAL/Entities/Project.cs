using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    [Table("Projects")]
    public class Project : IHeadData, ISoftDeleted
    {
        public Project()
        {
            this.ProjectAudits  = new List<ProjectAudit>();
            this.ProjectUsers   = new List<ProjectUser>();
        }
        
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public string Denomination { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }

        public virtual ICollection<ProjectAudit> ProjectAudits { get; set; }
    }
}
