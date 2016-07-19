using Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IISLogFileParserGaraio.Tests
{    
    public class IPExtractorTests
    {
        Random rand = new Random();
        private string GeneratePseudoRandomStrings(int minLength)
        {
            string seed = "the quick brown fox jumped over the lazy dog";
            StringBuilder sb = new StringBuilder();
            do
            {
                sb.Append(seed.Substring(rand.Next(seed.Length)));
            } while (sb.Length < minLength);
            sb.Append("{0}");
            return sb.ToString();
        }

        [Theory]
        [InlineData("10.10.2.18")]
        public void CanParseValidIPV4Address(string ipAddress)
        {
            string feed = string.Format(GeneratePseudoRandomStrings(150), ipAddress);
            IPExtractor extractor = new IPExtractor();
            var ip = extractor.ExtractIPString(feed);
            Assert.Equal(ipAddress, ip.Value);
        }


        [Fact]
        public void ReturnsEmptyOptionIfNoIPV4AddressIsFound()
        {
            string feed = GeneratePseudoRandomStrings(150);
            IPExtractor extractor = new IPExtractor();
            var ip = extractor.ExtractIPString(feed);
            Assert.True(ip.IsEmpty);
        }

        [Theory]
        [InlineData("2001:db8::ff00:42:8329")]
        public void CanParseValidIPV6Address(string ipAddress)
        {
            string feed = string.Format(GeneratePseudoRandomStrings(250), ipAddress);
            IPExtractor extractor = new IPExtractor();
            var ip = extractor.ExtractIPString(feed);
            Assert.Equal(ipAddress, ip.Value);
        }


        [Fact]
        public void ReturnsEmptyOptionIfNoIPV6AddressIsFound()
        {
            string feed = GeneratePseudoRandomStrings(200);
            IPExtractor extractor = new IPExtractor();
            var ip = extractor.ExtractIPString(feed);
            Assert.True(ip.IsEmpty);
        }
    }
}
