using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterDotNet.UnitTest
{
    [TestClass]
    public class PluralizeUnitTest
    {
        [TestMethod]
        public void Pluralize_WithIntValueOfOne_ReturnsSingular()
        {
            Assert.AreEqual("sock", 1.Pluralize("sock", "socks"));
        }

        [TestMethod]
        public void Pluralize_WithIntValueOfTen_ReturnsPlural()
        {
            Assert.AreEqual("socks", 10.Pluralize("sock", "socks"));
        }

        [TestMethod]
        public void Pluralize_WithLongValueOfOne_ReturnsSingular()
        {
            Assert.AreEqual("sock", 1L.Pluralize("sock", "socks"));
        }

        [TestMethod]
        public void Pluralize_WithLongValueOfTen_ReturnsPlural()
        {
            Assert.AreEqual("socks", 10L.Pluralize("sock", "socks"));
        }
    }
}
