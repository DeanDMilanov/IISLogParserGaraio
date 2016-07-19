using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class IPExtractor : IIPExtractor
    {
        private static readonly Regex ipv4Regex = new Regex(@"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}", RegexOptions.Compiled);
        private static readonly Regex ipv6Regex = new Regex(@"(?:^|(?<=\s))(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))(?=\s|$)", RegexOptions.Compiled);
        public Option<string> ExtractIPString(string text)
        {
            Option<string> ip = MatchIPV4Address(text);
            if (ip.HasValue)
            {
                return ip;
            }
            // maybe it's a IPV6 address
            ip = MatchIPV6Address(text);
            return ip;

        }

        private Option<string> MatchIPV4Address(string text)
        {
            Option<string> ip;
            var ipv4Address = ipv4Regex.Match(text);
            if (ipv4Address.Success)
            {
                ip = new Option<string>(ipv4Address.Value);
            }
            else
            {
                ip = new Option<string>();
            }
            return ip;
        }

        private Option<string> MatchIPV6Address(string text)
        {
            Option<string> ip;
            var ipv6Address = ipv6Regex.Match(text);
            if (ipv6Address.Success)
            {
                ip = new Option<string>(ipv6Address.Value);
            }
            else
            {
                ip = new Option<string>();
            }
            return ip;
        }
    }
}
