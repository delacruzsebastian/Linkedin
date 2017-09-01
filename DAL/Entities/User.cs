using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Enums;
using DAL.Interfaces;

namespace DAL.Entities
{
    public class User : IHeadData, ISoftDeleted
    {
        public User()
        {
            this.UserAudits = new List<UserAudit>();
        }

        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Denomination { get; set; }
        public string Username { get; set; }
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
