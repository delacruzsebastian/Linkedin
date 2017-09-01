using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Enums;
using DAL.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    [Table("Users")]
    public class User : IHeadData, ISoftDeleted
    {
        public User()
        {
            this.UserAudits = new List<UserAudit>();
        }

        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Firstname { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        public string Denomination { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        public Sex Sex { get; set; }

        public DateTime BirthDate { get; set;}

        public Boolean Available { get; set; }

        public Guid? CountryId { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public virtual ICollection<UserAudit> UserAudits { get; set; }

        public DateTime? DeleteDate { get; set; }

    }
}
