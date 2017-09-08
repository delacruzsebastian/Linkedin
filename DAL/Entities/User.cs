using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Enums;
using DAL.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("User")]
    public class User : IHeadData, ISoftDeleted
    {
        public User()
        {
            this.UserAudits            = new List<UserAudit>();
            this.OwnedProjects         = new List<Project>();
            this.ProjectSubscriptions  = new List<ProjectUser>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Denomination { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Sex Sex { get; set; }
        public DateTime? BirthDate { get; set;}
        public Boolean Available { get; set; }
        public Guid? CountryId { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        public virtual ICollection<Project> OwnedProjects { get; set; }
        public virtual ICollection<ProjectUser> ProjectSubscriptions { get; set; }
        
        public virtual ICollection<UserAudit> UserAudits { get; set; }
    }
}
