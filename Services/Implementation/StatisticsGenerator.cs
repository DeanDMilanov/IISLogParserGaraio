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
        ConcurrentDictionary<string, Statistics> statistics = new ConcurrentDictionary<string, Statistics>();

        public StatisticsGenerator(IIPExtractor ip, IFQDNExtractor fqdn)
        {
            ipExtractor = ip;
            fqdnExtractor = fqdn;
        }        

        public IEnumerable<Statistics> GenerateStatistics(IEnumerable<string> lines)
        {
            Parallel.ForEach(lines,
                (line) =>
                {
                    var statisticData = GenerateSingleStatisticEntry(line);
                    if (statisticData.HasValue)
                    {
                        statistics.AddOrUpdate(statisticData.Value.IPAddress, statisticData.Value,
                            (oldkey, oldvalue) =>
                                oldvalue = new Statistics
                                (oldvalue.IPAddress, oldvalue.FQDN, oldvalue.CallsPerClientCount + 1)
                            );
                    }
                }
            );
            return statistics.Values;
        }

        private Option<Statistics> GenerateSingleStatisticEntry(string text)
        {
            Option<Statistics> statistic;
            var ip = ipExtractor.ExtractIPString(text);
            if (ip.HasValue)
            {
                if (statistics.ContainsKey(ip.Value))
                {
                    return new Option<Statistics>(statistics[ip.Value]);
                }
                var fqdn = fqdnExtractor.ExtractFQDN(ip.Value);
                if (fqdn.HasValue)
                {
                    statistic = new Option<Statistics>(
                        new Statistics()
                        {
                            IPAddress = ip.Value,
                            FQDN = fqdn.Value
                        });
                }
                else
                {
                    statistic = new Option<Statistics>(
                        new Statistics()
                        {
                            IPAddress = ip.Value,
                            FQDN = "unknown"
                        });
                }
            }
            else
            {
                statistic = new Option<Statistics>();
            }
            return statistic;
        }
    }
}
