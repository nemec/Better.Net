using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterDotNet.UnitTest
{
    [TestClass]
    public class DictionaryFormatUnitTest
    {
        [TestMethod]
        public void FormatDict_WithSingleKey()
        {
            var actual = "{hello}".Format(new Dictionary<string, object>
                {
                    {"hello", 4}
                });
            Assert.AreEqual("4", actual);
        }

        [TestMethod]
        public void Format_WithFormatString()
        {
            var actual = "{hello:yyyy}".Format(new Dictionary<string, object>
                {
                    {"hello", new DateTime(1999, 1, 1)}
                });
            Assert.AreEqual("1999", actual);
        }

        [TestMethod]
        public void Format_WithDoubledBrace()
        {
            var actual = "{{ {hello:yyyy}".Format(new Dictionary<string, object>
                {
                    {"hello", new DateTime(1999, 1, 1)}
                });
            Assert.AreEqual("{ 1999", actual);
        }

        [TestMethod]
        public void Format_WithSideBySideBraces()
        {
            var actual = "{{{hello:yyyy}".Format(new Dictionary<string, object>
                {
                    {"hello", new DateTime(1999, 1, 1)}
                });
            Assert.AreEqual("{1999", actual);
        }

        [TestMethod]
        public void Format_WithOpenAndCloseBraces()
        {
            var actual = "{{{hello:yyyy}}}{{}}".Format(new Dictionary<string, object>
                {
                    {"hello", new DateTime(1999, 1, 1)}
                });
            Assert.AreEqual("{1999}{}", actual);
        }

        [TestMethod]
        public void Format_WithAlignment()
        {
            var actual = "{hello,-6}".Format(new Dictionary<string, object>
                {
                    {"hello", 4}
                });
            Assert.AreEqual("4     ", actual);
        }

        [TestMethod]
        public void Format_WithAlignmentAndFormatString()
        {
            var actual = "{hello,-10:yyyy}".Format(new Dictionary<string, object>
                {
                    {"hello", new DateTime(1999, 1, 1)}
                });
            Assert.AreEqual("1999      ", actual);
        }

        [TestMethod]
        public void Format_WithSpaces()
        {
            var actual = "{ \nhello\t :yyyy}".Format(new Dictionary<string, object>
                {
                    {"hello", new DateTime(1999, 1, 1)}
                });
            Assert.AreEqual("1999", actual);
        }

        [TestMethod]
        public void Format_WithExtraParameters_DoesNotThrowException()
        {
            var actual = "{hello}".Format(new Dictionary<string, object>
                {
                    {"hello", 10},
                    {"something", "oh"}
                });
            Assert.AreEqual("10", actual);
        }

        [TestMethod]
        public void Format_WithParametersOutOfOrder_FormatsCorrectParameters()
        {
            var actual = "{hello}{world}".Format(new Dictionary<string, object>
                {
                    {"world", 10},
                    {"hello", "oh"}
                });
            Assert.AreEqual("oh10", actual);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Format_WithNoClosingBrace_ThrowsFormatException()
        {
            "{hello:yyyy".Format(new Dictionary<string, object>
                {
                    {"hello", new DateTime(1999, 1, 1)}
                });
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Format_WithNoOpeningBrace_ThrowsFormatException()
        {
            "hello:yyyy}".Format(new Dictionary<string, object>
                {
                    {"hello", new DateTime(1999, 1, 1)}
                });
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Format_WithMissingParameter_ThrowsFormatException()
        {
            "{hello}".Format(new Dictionary<string, object>
                {
                    {"rodney", new DateTime(1999, 1, 1)}
                });
        }
    }
}
