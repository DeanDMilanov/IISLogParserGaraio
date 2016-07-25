using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class IpFqdn
    {
        [Key]
        [MaxLength(64)]
        public string IpAddress { get; set; }
        [MaxLength(128)]
        public string FQDN { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
