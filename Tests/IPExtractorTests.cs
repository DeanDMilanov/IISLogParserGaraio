using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class MyTestClass
    {
        Random rand = new Random();
        string ipAddress;
        private string GeneratePseudoRandomStrings(int minLength)
        {
            string seed = "the quick brown fox jumped over the lazy dog";
            StringBuilder sb = new StringBuilder();
            do
            {
                sb.Append(seed.Substring(rand.Next(seed.Length)));
            } while (sb.Length < minLength);
            sb.Append(" {0}");
            return sb.ToString();
        }

        [TestMethod]
        public void CanParseValidIPV4Address()
        {
            ipAddress = "10.10.2.18";
            string feed = string.Format(GeneratePseudoRandomStrings(150), ipAddress);
            IPExtractor extractor = new IPExtractor();
            var ip = extractor.ExtractIPString(feed);
            Assert.AreEqual(ipAddress, ip.Value);
        }


        [TestMethod]
        public void ReturnsEmptyOptionIfNoIPV4AddressIsFound()
        {
            string feed = GeneratePseudoRandomStrings(150);
            IPExtractor extractor = new IPExtractor();
            var ip = extractor.ExtractIPString(feed);
            Assert.IsTrue(ip.IsEmpty);
        }

        [TestMethod]
        public void CanParseValidIPV6Address()
        {
            ipAddress = "2001:cdba:0000:0000:0000:0000:3257:9652";
            string feed = string.Format(GeneratePseudoRandomStrings(250), ipAddress);
            IPExtractor extractor = new IPExtractor();
            var ip = extractor.ExtractIPString(feed);
            Assert.AreEqual(ipAddress, ip.Value);
        }


        [TestMethod]
        public void ReturnsEmptyOptionIfNoIPV6AddressIsFound()
        {
            string feed = GeneratePseudoRandomStrings(200);
            IPExtractor extractor = new IPExtractor();
            var ip = extractor.ExtractIPString(feed);
            Assert.IsTrue(ip.IsEmpty);
        }
    }
    
}
