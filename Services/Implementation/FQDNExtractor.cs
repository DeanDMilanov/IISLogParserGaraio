﻿using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class FQDNExtractor : IFQDNExtractor
    {
        public Option<string> ExtractFQDN(string ipString)
        {
            Option<string> fqdn;
            if (IsLoopBackAddress(ipString))
            {
                return new Option<string>("loopback address");
            }
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ipString);
                IPHostEntry hostEntry = Dns.GetHostEntry(ipAddress);
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

        private bool IsLoopBackAddress(string ipString)
        {
            return ipString == "::1" || ipString == "127.0.0.0";
        }
    }
}
