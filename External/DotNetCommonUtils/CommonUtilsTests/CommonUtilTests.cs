using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonUtils;

namespace CommonTest
{
    [TestClass]
    public class CommonUtilTests
    {
        [TestMethod]
        public void ConvertCharsToASCIITest()
        {
            string normalChars = "Hello World!";
            Assert.AreEqual(normalChars, Utils.ConvertCharsToASCII(normalChars));

            string accentChars = "ÀÈÌÒÙàèìòù" + "ÁÉÍÓÚÝáéíóúý" + "ÂÊÎÔÛâêîôû" + "ÃÑÕãñõ" + "ÄËÏÖÜŸäëïöüÿ";
            string accentCharsMapped = "AEIOUaeiou" + "AEIOUYaeiouy" + "AEIOUaeiou" + "ANOano" + "AEIOUYaeiouy";
            Assert.AreEqual(accentCharsMapped, Utils.ConvertCharsToASCII(accentChars));

            string specialChars = "ƒ © ® ™";
            string specialCharsMapped = "f (c) (R) TM";
            Assert.AreEqual(specialCharsMapped, Utils.ConvertCharsToASCII(specialChars));

            string fractionChars = "\u2150 \u2151 \u2152 \u2153 \u2154 \u2155 \u2156 \u2157 \u2158 \u2159 \u215a \u215b \u215c \u215d \u215e \u215f \u2189";
            string fractionCharsMapped = "1/7 1/9 1/10 1/3 2/3 1/5 2/5 3/5 4/5 1/6 5/6 1/8 3/8 5/8 7/8 1/ 0/3";
            Assert.AreEqual(fractionCharsMapped, Utils.ConvertCharsToASCII(fractionChars));

            string romanNumeralsChars1 = "\u2160 \u2161 \u2162 \u2163 \u2164 \u2165 \u2166 \u2167 \u2168 \u2169 \u216a \u216b \u216c \u216d \u216e \u216f";
            string romanNumeralsChars1Mapped = "I II III IV V VI VII VIII IX X XI XII L C D M";
            Assert.AreEqual(romanNumeralsChars1Mapped, Utils.ConvertCharsToASCII(romanNumeralsChars1));

            string romanNumeralsChars2 = "\u2170 \u2171 \u2172 \u2173 \u2174 \u2175 \u2176 \u2177 \u2178 \u2179 \u217a \u217b \u217c \u217d \u217e \u217f";
            string romanNumeralsChars2Mapped = "i ii iii iv v vi vii viii ix x xi xii l c d m";
            Assert.AreEqual(romanNumeralsChars2Mapped, Utils.ConvertCharsToASCII(romanNumeralsChars2));

            string romanNumeralsChars3 = "\u2180 \u2181 \u2182 \u2183 \u2187 \u2188";
            string romanNumeralsChars3Mapped = "(D D) ((|)) ) D)) (((|)))";
            Assert.AreEqual(romanNumeralsChars3Mapped, Utils.ConvertCharsToASCII(romanNumeralsChars3));

            // Chinese, Japanese and Korean characters should map to themselves
            string cjkChars = "中文 ライス 한국";
            Assert.AreEqual(cjkChars, Utils.ConvertCharsToASCII(cjkChars));
        }

        [TestMethod]
        public void GetXmlPathNodeTest()
        {
            var xml = @"<test masterId=""234""><parent a=""parent attr""><child>hello from child!</child><child>hello from child2!</child></parent></test>";
            var childValue = Utils.GetPathNodeValue(xml, null, "parent", "child");
            Assert.AreEqual("hello from child!~hello from child2!", childValue);
            var attrValue = Utils.GetPathNodeValue(xml, "a", "parent");
            Assert.AreEqual("parent attr", attrValue);
            var rootAttrValue = Utils.GetPathNodeValue(xml, "masterId");
            Assert.AreEqual("234", rootAttrValue);
            var nonExistentElement = Utils.GetPathNodeValue(xml, null, "parent", "child1");
            Assert.AreEqual("", nonExistentElement);
        }

        [TestMethod]
        public void NthOrderStatisticsSmallDataTests()
        {
            var a = new[] { 5 };
            Assert.AreEqual(5, a.NthOrderStatistic(0));

            a = new[] { 5, 10 };
            Assert.AreEqual(5, a.NthOrderStatistic(0));
            Assert.AreEqual(10, a.NthOrderStatistic(1));

            a = new[] { 10, 5 };
            Assert.AreEqual(5, a.NthOrderStatistic(0));
            Assert.AreEqual(10, a.NthOrderStatistic(1));

            a = new[] { 10, 5, 5, 10, 10, 5 };
            Assert.AreEqual(5, a.NthOrderStatistic(0));
            Assert.AreEqual(5, a.NthOrderStatistic(1));
            Assert.AreEqual(5, a.NthOrderStatistic(2));
            Assert.AreEqual(10, a.NthOrderStatistic(3));
            Assert.AreEqual(10, a.NthOrderStatistic(4));
            Assert.AreEqual(10, a.NthOrderStatistic(5));
        }

        [TestMethod]
        public void NthOrderStatisticsOrderedDataTests()
        {
            for (var length = 10; length < 12; length++)
            {
                var a = Enumerable.Range(0, length).Select(i => i * length).ToArray();
                for (var i = 0; i < a.Length; i++)
                    Assert.AreEqual(i * length, a.NthOrderStatistic(i));

                a = Enumerable.Range(0, length).Select(i => i * length).OrderByDescending(i => i).ToArray();
                for (var i = 0; i < a.Length; i++)
                    Assert.AreEqual(i * length, a.NthOrderStatistic(i));

                a = Enumerable.Range(0, length).Select(i => i * length).Randomize(42).ToArray();
                for (var i = 0; i < a.Length; i++)
                    Assert.AreEqual(i * length, a.NthOrderStatistic(i));
            }
        }

        [TestMethod]
        public void NthOrderStatisticsRandomDataTests()
        {
            var rnd = new Random(42);
            for (var sample = 0; sample < 30; sample++)
            {
                var a = Enumerable.Range(0, rnd.Next(50, 150)).Select(i => rnd.Next()).ToArray();
                var sorted = a.OrderBy(i => i).ToArray();
                for (var i = 0; i < a.Length; i++)
                    Assert.AreEqual(sorted[i], a.NthOrderStatistic(i));
            }
        }

        [TestMethod]
        public void MedianSmallDataTests()
        {
            var a = new[] { 5 };
            Assert.AreEqual(5, a.Median());

            a = new[] { 2, 10 };
            Assert.AreEqual(6, a.Median());

            a = new[] { 10, 2 };
            Assert.AreEqual(6, a.Median());

            a = new[] { 2, 3, 4, 1, 1, 6, 5, 5 };
            Assert.AreEqual(3.5, a.Median());

            a = new[] { 2, 4, 1, 6, 5, 5, 7 };
            Assert.AreEqual(5, a.Median());
        }

        private static double MedianBySort(IEnumerable<int> list)
        {
            var sorted = list.OrderBy(v => v).ToArray();
            if (sorted.Length%2 == 0)
                return (sorted[(sorted.Length - 1)/2] + sorted[(sorted.Length)/2])/2.0;
            else
                return sorted[(sorted.Length - 1)/2];
        }

        [TestMethod]
        public void MedianOrderedDataTests()
        {
            for (var length = 10; length < 12; length++)
            {
                var a = Enumerable.Range(0, length).Select(i => i * length).ToArray();
                Assert.AreEqual(MedianBySort(a), a.Median());

                a = Enumerable.Range(0, length).Select(i => i * length).OrderByDescending(i => i).ToArray();
                Assert.AreEqual(MedianBySort(a), a.Median());

                a = Enumerable.Range(0, length).Select(i => 5).ToArray();
                Assert.AreEqual(5, a.Median());

                a = a.Randomize(42).ToArray();
                Assert.AreEqual(MedianBySort(a), a.Median());
            }
        }

        [TestMethod]
        public void MedianRandomDataTests()
        {
            var rnd = new Random(42);
            for (var sample = 0; sample < 30; sample++)
            {
                var a = Enumerable.Range(0, rnd.Next(3, 7)).Select(i => rnd.Next(10)).ToArray();
                Assert.AreEqual(MedianBySort(a), a.Median());
            }
            for (var sample = 0; sample < 30; sample++)
            {
                var a = Enumerable.Range(0, rnd.Next(50, 150)).Select(i => rnd.Next(150)).ToArray();
                Assert.AreEqual(MedianBySort(a), a.Median());
            }
        }

        [TestMethod]
        public void WindowRangeTests()
        {
            var normalWindowActual = Utils.WindowRange(3, 10, 3).ToArray();
            var normalWindowExpected = new[] { 0, 1, 2, 3, 4, 5, 6 };
            Assert.IsTrue(normalWindowActual.SequenceEqual(normalWindowExpected), "WindowRange was different than expected for normal case");

            var startWindowActual = Utils.WindowRange(1, 10, 3).ToArray();
            var startWindowExpected = new[] { 8, 9, 0, 1, 2, 3, 4 };
            Assert.IsTrue(startWindowActual.SequenceEqual(startWindowExpected), "WindowRange was different than expected for start case");

            var endWindowActual = Utils.WindowRange(8, 10, 3).ToArray();
            var endWindowExpected = new[] { 5, 6, 7, 8, 9, 0, 1 };
            Assert.IsTrue(endWindowActual.SequenceEqual(endWindowExpected), "WindowRange was different than expected for end case");
        }
    }
}