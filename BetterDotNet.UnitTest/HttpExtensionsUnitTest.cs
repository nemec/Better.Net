using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterDotNet.UnitTest
{
    [TestClass]
    public class HttpExtensionsUnitTest
    {
        [TestMethod]
        public void ConstructUri_WithUsername_StoresUsernameInProperty()
        {
            const string str = "http://test@google.com";
            var uri = new ExtendedUri(str);

            Assert.AreEqual("test", uri.Username);
        }

        [TestMethod]
        public void ConstructUri_WithEmptyUsernameAndPassword_StoresEmptyUsernameInProperty()
        {
            const string str = "http://@google.com";
            var uri = new ExtendedUri(str);

            Assert.AreEqual("", uri.Username);
        }

        [TestMethod]
        public void ConstructUri_WithEmptyUsernameAndNoPassword_DoesNotStorePasswordInProperty()
        {
            const string str = "http://@google.com";
            var uri = new ExtendedUri(str);

            Assert.AreEqual(null, uri.Password);
        }

        [TestMethod]
        public void ConstructUri_WithUsernameAndEmptyPassword_StoresEmptyPasswordInProperty()
        {
            const string str = "http://test:@google.com";
            var uri = new ExtendedUri(str);

            Assert.AreEqual("", uri.Password);
        }

        [TestMethod]
        public void ConstructUri_WithPassword_StoresPasswordInProperty()
        {
            const string str = "http://test:pass@google.com";
            var uri = new ExtendedUri(str);

            Assert.AreEqual("pass", uri.Password);
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void ConstructUri_WithPathParameterContainingMultipleEqualsSigns_ThrowsException()
        {
            const string str = "http://google.com/path;key=value=none/path";
            var uri = new ExtendedUri(str);
        }

        [TestMethod]
        public void ConstructUri_WithOnePathParameterKeyValue_CreatesAPathParameter()
        {
            const string str = "http://google.com/path;key=value/other";
            var uri = new ExtendedUri(str);

            Assert.AreEqual(
                new KeyValuePair<string,string>("key", "value"),
                uri.Segments[1].MatrixParameters[0]);
        }

        [TestMethod]
        public void ConstructUri_WithOnePathParameterKeyAndNoValue_CreatesAPathParameterWithNullValue()
        {
            const string str = "http://google.com/path;key/other";
            var uri = new ExtendedUri(str);

            Assert.AreEqual(
                new KeyValuePair<string,string>("key", null),
                uri.Segments[1].MatrixParameters[0]);
        }

        [TestMethod]
        public void ConstructUri_WithTwoPathParametersKeyValue_CreatesTwoPathParameters()
        {
            const string str = "http://google.com/path;key=value;a=b/other";
            var uri = new ExtendedUri(str);

            Assert.AreEqual(
                new KeyValuePair<string,string>("key", "value"),
                uri.Segments[1].MatrixParameters[0]);
            Assert.AreEqual(
                new KeyValuePair<string,string>("a", "b"),
                uri.Segments[1].MatrixParameters[1]);
        }

        [TestMethod]
        public void ConstructUri_WithQueryParameter_CreatesEntryInLookup()
        {
            const string str = "http://google.com/path?key=value";
            var uri = new ExtendedUri(str);

            Assert.AreEqual("value", uri.QueryParameters["key"]);
        }

        [TestMethod]
        public void ConstructUri_WithDuplicateKeyInQueryParameter_PutsBothInKey()
        {
            const string str = "http://google.com/path?key=value1&key=value2";
            var uri = new ExtendedUri(str);

            Assert.AreEqual("value1,value2", uri.QueryParameters["key"]);
        }
    }
}
