using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IIpToFqdnRepository : IDisposable
    {
        Task AddOrUpdateAsync(IpFqdn record);
        Task AddOrUpdateAsync(IEnumerable<IpFqdn> records);
        Task<IEnumerable<IpFqdn>> GetAllAsync(IEnumerable<string> ips);
        Task<IpFqdn> GetAsync(string ip);
    }
}
