using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class Statistics
    {
        [Display(Name = "IP address")]
        public string IPAddress { get; set; }
        [Display(Name = "Fully qualified domain name")]
        public string FQDN { get; set; }
        [Display(Name = "Count of calls per client IP")]
        public int CallsPerClientCount { get; set; }

        public Statistics()
        {

        }

        public Statistics(string ip, string fqdn, int calls)
        {
            IPAddress = ip;
            FQDN = fqdn;
            CallsPerClientCount = calls;
        }
    }
}
