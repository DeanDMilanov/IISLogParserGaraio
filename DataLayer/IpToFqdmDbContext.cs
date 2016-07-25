using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class IpToFqdmDbContext : DbContext
    {
        public IpToFqdmDbContext() : base("IpToFqdn") { }

        public DbSet<IpFqdn> IpToFqdn { get; set; }
    }
}
