using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtils;
using System.Data;

namespace CommonUtilsTests
{
    [TestClass]
    public class IifParserTests
    {
        [TestMethod]
        public void TestIifParser()
        {
            using (var parser = new CommonUtils.FileFormatParsers.QuickbooksIifParser())
            {
                DataSet d = parser.Parse(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"IifTestFile.iif"));
                Assert.IsTrue(d.Tables.Count > 2);
            }
        }
    }
}
