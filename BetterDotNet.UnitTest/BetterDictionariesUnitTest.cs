using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterDotNet.UnitTest
{
    [TestClass]
    public class BetterDictionariesUnitTest
    {
        private const string Key = "key";
        private const string Value = "value";
        private const string DefaultValue = "default value";

        private const string MissingKey = "missing key";

        private Dictionary<string, string> Dict { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Dict = new Dictionary<string, string> { { Key, Value } };
        }

        [TestMethod]
        public void GetValue_WithNullDefaultValueAndMissingKey_ReturnsNullValue()
        {
            // Arrange

            // Act
            var actual = Dict.Get(MissingKey, null);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void GetValue_WithNullDefaultValueAndFoundKey_ReturnsFoundValue()
        {
            // Arrange

            // Act
            var actual = Dict.Get(Key, null);

            // Assert
            Assert.AreEqual(Value, actual);
        }

        [TestMethod]
        public void GetValue_WithSomeDefaultValueAndMissingKey_ReturnsFoundValue()
        {
            // Arrange

            // Act
            var actual = Dict.Get(MissingKey, DefaultValue);

            // Assert
            Assert.AreEqual(DefaultValue, actual);
        }
    }
}
