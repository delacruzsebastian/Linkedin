using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ProjectUser : IHeadData, ISoftDeleted
    {
        public ProjectUser()
        {
            this.ProjectUserAudits = new List<ProjectUserAudit>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public string Denomination { get; set; }

        public DateTime AddDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
        public virtual ICollection<ProjectUserAudit> ProjectUserAudits { get; set; }
    }
}
