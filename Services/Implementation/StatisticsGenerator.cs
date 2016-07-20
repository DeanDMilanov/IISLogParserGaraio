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

        public IEnumerable<Statistics> GenerateStatistics(IEnumerable<string> lines)
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

            Parallel.ForEach(ipCalls, (entry) =>
            {
                statistics.Add(GenerateSingleStatistic(entry.Key, entry.Value));
            });
            return statistics;
        }

        private Statistics GenerateSingleStatistic(string entry, int calls)
        {
            var fqdn = fqdnExtractor.ExtractFQDN(entry);
            return new Statistics(entry, fqdn.Value ?? "unknown", calls);
        }        
    }
}
