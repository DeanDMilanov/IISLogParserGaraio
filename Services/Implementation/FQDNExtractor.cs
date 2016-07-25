using DataLayer;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Services.Models;

namespace Services.Implementation
{
    public class FQDNExtractor : IFQDNExtractor
    {
        private IIpToFqdnRepository repository;

        public FQDNExtractor(IIpToFqdnRepository repository)
        {
            this.repository = repository;
        }
        public FQDNExtractor()
        {
            repository = new IpToFqdnRepository();
        }

        public async Task<Option<string>> ExtractFQDNAsync(string ipString)
        {
            Option<string> fqdn;
            if (IsLoopBackAddress(ipString))
            {
                return new Option<string>("loopback address");
            }
            else if (IsPrivateAddress(ipString))
            {
                return new Option<string>("private address");
            }
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ipString);
                IPHostEntry hostEntry = await Dns.GetHostEntryAsync(ipAddress);
                IPGlobalProperties domainEntry = IPGlobalProperties.GetIPGlobalProperties();
                fqdn = new Option<string>(
                    string.Format("{0} {1}", hostEntry.HostName, domainEntry.DomainName));
            }
            catch (Exception)
            {
                fqdn = new Option<string>();
            }
            return fqdn;
        }

        public async Task<FQDNCollections> ExtractFQDNfromDNSAsync(IEnumerable<string> ips)
        {
            IPAddress ipAddress;
            IPHostEntry hostEntry;
            FQDNCollections fqdn = new FQDNCollections();
            Parallel.ForEach(ips, async (ip) =>
            {
                ipAddress = IPAddress.Parse(ip);
                hostEntry = await Dns.GetHostEntryAsync(ipAddress);
                lock (fqdn)
                {
                    if (!string.IsNullOrWhiteSpace(hostEntry.HostName))
                    {
                        fqdn.Resolved.Add(ip, hostEntry.HostName);
                    }
                    else
                    {
                        fqdn.Unresolved.Add(ip);
                    }
                }
            });
            return fqdn;
        }

        public async Task<FQDNCollections> ExtractSavedFQDNAsync(IEnumerable<string> ips)
        {
            FQDNCollections result = new FQDNCollections();
            using (repository)
            {
                var availablePairs = await repository.GetAllAsync(ips);
                foreach (var pair in availablePairs)
                {
                    if (!string.IsNullOrWhiteSpace(pair.FQDN))
                    { result.Resolved.Add(pair.IpAddress, pair.FQDN); }
                    else
                    { result.Unresolved.Add(pair.IpAddress); };
                }
            }
            return result;
        }

        public async Task SaveFQDNAsync(Dictionary<string, string> ipAndFqdn)
        {
            IEnumerable<IpFqdn> ipFqdn = GenerateIpFqdn(ipAndFqdn);
            using (repository)
            {
                await repository.AddOrUpdateAsync(ipFqdn);
            }
        }

        private IEnumerable<IpFqdn> GenerateIpFqdn(Dictionary<string, string> ipAndFqdn)
        {
            List<IpFqdn> transformed = new List<IpFqdn>(ipAndFqdn.Count);
            foreach (var item in ipAndFqdn)
            {
                transformed.Add(new IpFqdn() { IpAddress = item.Key, FQDN = item.Value });
            }
            return transformed;
        }

        private bool IsLoopBackAddress(string ipString)
        {
            return ipString == "::1" || ipString == "127.0.0.0";
        }

        public bool IsPrivateAddress(string ipString)
        {
            int[] ipBits = ipString.Split('.').Select(c => int.Parse(c)).ToArray();
            return ipBits[0] == 10 || (ipBits[0] == 192 && ipBits[1] == 168) ||
                (ipBits[0] == 172 && (ipBits[1] >= 16 && ipBits[1] <= 31));
        }
    }
}
