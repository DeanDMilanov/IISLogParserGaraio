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
            Parallel.ForEach(statistics, (entry) =>
            {
                LookUpFQDN(entry);
            });
            return statistics.Values;
        }

        private void LookUpFQDN(KeyValuePair<string, Statistics> entry)
        {
            var fqdn = fqdnExtractor.ExtractFQDN(entry.Key);

            if (fqdn.HasValue)
            {
                statistics.AddOrUpdate(entry.Key, entry.Value,
                        (oldkey, oldvalue) =>
                            oldvalue = new Statistics
                            (oldvalue.IPAddress, fqdn.Value, oldvalue.CallsPerClientCount)
                        );
            }
        }

        private Option<Statistics> GenerateSingleStatisticEntry(string text)
        {
            Option<Statistics> statistic;
            var ip = ipExtractor.ExtractIPString(text);
            if (ip.HasValue)
            {
                statistic = CheckIfEntryIsDuplicated(ip.Value);
            }
            else
            {
                statistic = new Option<Statistics>();
            }
            return statistic;
        }

        private Option<Statistics> CheckIfEntryIsDuplicated(string ip)
        {
            if (statistics.ContainsKey(ip))
            {
                return new Option<Statistics>(statistics[ip]);
            }
            else
            {
                return new Option<Statistics>(new Statistics()
                {
                    IPAddress = ip,
                    FQDN = "unknown"
                });
            }
        }
    }
}
