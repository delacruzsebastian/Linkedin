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

    [Table("Countries")]
    public class Country : IKey
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Country()
        {
            CountryStates = new HashSet<CountryState>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Denomination { get; set; }
        public virtual ICollection<CountryState> CountryStates { get; set; }
    }
}

