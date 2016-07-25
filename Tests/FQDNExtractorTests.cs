using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class FQDNExtractorTests
    {
        [TestMethod]
        public void IPV4AddressIsPrivate()
        {
            FQDNExtractor extractor = new FQDNExtractor();
            Assert.IsTrue(extractor.IsPrivateAddress("172.31.255.255 "));
        }
    }
}
