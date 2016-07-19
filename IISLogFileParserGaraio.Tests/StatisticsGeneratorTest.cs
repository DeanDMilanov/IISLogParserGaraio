using Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Services.Interfaces;
using Xunit;
using Services;

namespace IISLogFileParserGaraio.Tests
{
    class StatisticsGeneratorTest
    {
        Mock<IFQDNExtractor> fqdnMock = new Mock<IFQDNExtractor>();
        public StatisticsGeneratorTest()
        {
            fqdnMock.Setup(c => c.ExtractFQDN("::1")).Returns(new Option<string>("loopback"));
            fqdnMock.Setup(c => c.ExtractFQDN("10.10.2.18")).Returns(new Option<string>("private"));
        }

        [Theory]
        [InlineData("2016-02-15 14:26:03 10.10.2.18 GET /Admin/Setting/Media - 443 test@garaio.com 83.150.38.202 Mozilla/5.0+(Windows+NT+6.3;+WOW64)+AppleWebKit/537.36+(KHTML,+like+Gecko)+Chrome/48.0.2564.109+Safari/537.36 https://10.10.10.10/Admin/Setting/GeneralCommon 200 0 0 31921")]
        [InlineData("2016 - 02 - 15 14:26:03 ::1 GET / bundles / scripts / mldywzht4qbdkpfc1dmyt3fi - woufuop5p4ofykbxhm1 v = tYcwlp7sC3Lprqv3RqjoxyBT7vHf11MDXPv5yNDPwXk1 443 test@garaio.com ::1 Mozilla / 5.0 + (compatible;+MSIE + 10.0;+Windows + NT + 6.2;+WOW64;+Trident / 6.0) https://localhost/Admin/Widget/ConfigureWidget?systemName=Widgets.GoogleAdwords 200 0 0 64")]
        public void GeneratesStatisticsWhenSuppliedWithValidData(string log)
        {
            StatisticsGenerator generator = new StatisticsGenerator(new IPExtractor(), fqdnMock.Object);
            var result = generator.GenerateStatistics(new[] { log });
            Assert.NotEmpty(result);
        }

    }
}
