using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Models;
using System.Collections.Concurrent;

namespace Services.Implementation
{
    public class StatisticsGenerator : IStatisticsGenerator
    {
        private IIPExtractor ipExtractor;
        private IFQDNExtractor fqdnExtractor;
        private ConcurrentDictionary<string, int> ipCalls = new ConcurrentDictionary<string, int>();
        private ConcurrentBag<Statistics> statistics = new ConcurrentBag<Statistics>();

        public StatisticsGenerator(IIPExtractor ip, IFQDNExtractor fqdn)
        {
            ipExtractor = ip;
            fqdnExtractor = fqdn;
        }

        public IEnumerable<Statistics> GenerateFullStatistics(IEnumerable<string> lines)
        {
            Parallel.ForEach(lines, (line) =>
            {
                var ipAddress = ipExtractor.ExtractIPString(line);
                if (ipAddress.HasValue)
                {
                    ipCalls.AddOrUpdate(ipAddress.Value, 1,
                        (oldkey, oldvalue) => oldvalue = oldvalue + 1);
                }
            });

            Parallel.ForEach(ipCalls, async (entry) =>
            {
                statistics.Add(await GenerateSingleStatistic(entry.Key, entry.Value));
            });
            return statistics;
        }

        private async Task<Statistics> GenerateSingleStatistic(string entry, int calls)
        {
            var fqdn = await fqdnExtractor.ExtractFQDNAsync(entry);
            return new Statistics(entry, fqdn.Value ?? "unknown", calls);
        }

        public async Task<Dictionary<string, string>> GeneratePartialStatistics(IEnumerable<string> ips)
        {
            var fromDb = await fqdnExtractor.ExtractSavedFQDNAsync(ips);
            var fromDns = await fqdnExtractor.ExtractFQDNfromDNSAsync(fromDb.Unresolved);
            await fqdnExtractor.SaveFQDNAsync(fromDns.Resolved);
            Dictionary<string, string> result = fromDb.Resolved;

            return result;
        }

        private Dictionary<string, string> CombineDictionaries(params Dictionary<string, string>[] dictionaries)
        {
            Dictionary<string, string> combined = new Dictionary<string, string>(dictionaries.Sum(c => c.Count));

            return combined;
        }
    }
}
