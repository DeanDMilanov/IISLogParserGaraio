using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class FQDNCollections
    {
        public Dictionary<string, string> Resolved { get; private set; }
        public List<string> Unresolved { get; private set; }

        public FQDNCollections(Dictionary<string, string> resolved, List<string> unresolved)
        {
            Resolved = resolved;
            Unresolved = unresolved;
        }

        public FQDNCollections()
        {
            Resolved = new Dictionary<string, string>();
            Unresolved = new List<string>();
        }
    }
}
