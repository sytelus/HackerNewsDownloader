using System;
using System.IO;
using System.Linq;
using CommonUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonUtilsTests
{
    [TestClass]
    public class CsvParserTests
    {
        [TestMethod]
        public void CsvParseTests()
        {
            var results = Utils.ParseCsvLine("ax bn,b,  c, d p ").ToArray();
            Assert.IsTrue(results.SequenceEqual(new string[] { "ax bn","b","  c"," d p " }));

            results = Utils.ParseCsvLine("ax bn,b,\"  c\", d  ").ToArray();
            Assert.IsTrue(results.SequenceEqual(new string[] { "ax bn","b", "  c", " d  " }));

            results = Utils.ParseCsvLine("ax bn,b,\" \"\" c\", d  ").ToArray();
            Assert.IsTrue(results.SequenceEqual(new string[] { "ax bn","b", " \" c", " d  " }));

            results = Utils.ParseCsvLine("ax bn,b,\" \"\"\"\" c\", d  ").ToArray();
            Assert.IsTrue(results.SequenceEqual(new string[] { "ax bn","b", " \"\" c", " d  " }));

            results = Utils.ParseCsvLine("ax bn,b,\" \"\"\", d  ").ToArray();
            Assert.IsTrue(results.SequenceEqual(new string[] { "ax bn","b", " \"", " d  " }));

            results = Utils.ParseCsvLine("ax bn,b,\"bo \"\" bo \", \"go \"\" go\" ").ToArray();
            Assert.IsTrue(results.SequenceEqual(new string[] { "ax bn", "b", "bo \" bo ", " go \" go " }));
        }

        [ExpectedException(typeof(InvalidDataException))]
        [TestMethod]
        public void UnterminatedQuotedColumnTest1()
        {
            var results = Utils.ParseCsvLine("ax bn,b,\"  c, d  ").ToArray();
        }
    
        [ExpectedException(typeof(InvalidDataException))]
        [TestMethod]
        public void UnterminatedQuotedColumnTest2()
        {
            var results = Utils.ParseCsvLine("ax bn,b,\" \"\"\"  c\", d  ").ToArray();
        }

        [ExpectedException(typeof(InvalidDataException))]
        [TestMethod]
        public void UnterminatedQuotedColumnTest3()
        {
            var results = Utils.ParseCsvLine("ax bn,b,\"").ToArray();
        }

        [ExpectedException(typeof(InvalidDataException))]
        [TestMethod]
        public void UnterminatedQuotedColumnTest4()
        {
            var results = Utils.ParseCsvLine("ax bn,b,\"bo \"bo\" bo \"").ToArray();
        }

        [ExpectedException(typeof(InvalidDataException))]
        [TestMethod]
        public void UnterminatedQuotedColumnTest5()
        {
            var results = Utils.ParseCsvLine("ax bn,b,\"bo \"\" bo \",  \"go\" \"ho\" ").ToArray();
        }
    }
}
