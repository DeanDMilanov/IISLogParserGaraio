using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IFQDNExtractor
    {
        Task<Option<string>> ExtractFQDNAsync(string ipString);
        Task<FQDNCollections> ExtractSavedFQDNAsync(IEnumerable<string> ips);
        Task<FQDNCollections> ExtractFQDNfromDNSAsync(IEnumerable<string> ips);
        Task SaveFQDNAsync(Dictionary<string, string> ipAndFqdn);
    }
}
