using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class IpToFqdnRepository : IIpToFqdnRepository
    {
        private IpToFqdmDbContext context;
        public IpToFqdnRepository()
        {
            context = new IpToFqdmDbContext();
        }
        public IpToFqdnRepository(IpToFqdmDbContext context)
        {
            this.context = context;
        }

        public async Task AddOrUpdateAsync(IEnumerable<IpFqdn> records)
        {
            IpFqdn ipFqdn;
            foreach (var record in records)
            {
                ipFqdn = await context.IpToFqdn.FindAsync(record.IpAddress);
                if (ipFqdn != null)
                {
                    ipFqdn.DateCreated = DateTime.Now;
                }
                else
                {
                    record.DateCreated = DateTime.Now;
                    context.IpToFqdn.Add(record);
                }
            }
            context.SaveChanges();
        }

        public async Task AddOrUpdateAsync(IpFqdn record)
        {
            var ipFqdn = await context.IpToFqdn.FindAsync(record.IpAddress);
            if (ipFqdn != null)
            {
                ipFqdn.DateCreated = DateTime.Now;
            }
            else
            {
                context.IpToFqdn.Add(record);
            }
            context.SaveChanges();
        }

        public async Task<IpFqdn> GetAsync(string ip)
        {
            var result = await context.IpToFqdn.FindAsync(ip);
            if (result == null)
            {
                result = new IpFqdn() { IpAddress = ip };
            }

            return result;
        }

        public async Task<IEnumerable<IpFqdn>> GetAllAsync(IEnumerable<string> ips)
        {
            List<IpFqdn> entries = new List<IpFqdn>();
            foreach (var ip in ips)
            {
                entries.Add(await GetAsync(ip));
            }
            return entries;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
