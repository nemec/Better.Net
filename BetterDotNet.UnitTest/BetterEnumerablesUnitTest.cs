using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace BetterDotNet.UnitTest
{
    [TestClass]
    public class BetterEnumerablesUnitTest
    {

        [TestMethod]
        public void Tee_WithTwoOutputs_AllowsAlternatingConsume()
        {
            var arr = new[] {1, 2, 3};
            var enums = arr.Tee(2);

            var iter1 = enums[0].GetEnumerator();
            var iter2 = enums[1].GetEnumerator();

            iter1.MoveNext();
            Assert.AreEqual(1, iter1.Current);

            iter1.MoveNext();
            Assert.AreEqual(2, iter1.Current);

            iter2.MoveNext();
            Assert.AreEqual(1, iter2.Current);

            iter2.MoveNext();
            Assert.AreEqual(2, iter2.Current);
            
            iter2.MoveNext();
            Assert.AreEqual(3, iter2.Current);
            
            iter1.MoveNext();
            Assert.AreEqual(3, iter1.Current);

        }

        [TestMethod]
        public void Tee_WithTwoOutputsAndOneConsumingCompletelyBeforeOtherStarts_OutputsEquivalentEnumerables()
        {
            var arr = new[] {1, 2, 3};
            var enums = arr.Tee(2);

            Assert.IsTrue(arr.SequenceEqual(enums[0]));
            Assert.IsTrue(arr.SequenceEqual(enums[1]));
        }

        [TestMethod]
        public void Tee_WithOneOutput_ConsumesOutputLikeSingleEnumerable()
        {
            var arr = new[] {1, 2, 3};
            var enums = arr.Tee(1);

            var iter1 = enums[0].GetEnumerator();

            iter1.MoveNext();
            Assert.AreEqual(1, iter1.Current);

            iter1.MoveNext();
            Assert.AreEqual(2, iter1.Current);
            
            iter1.MoveNext();
            Assert.AreEqual(3, iter1.Current);

        }

        [TestMethod]
        public void Tee_WithThreeOutputsAndFirstConsumedCompletelySecondPartiallyThirdCompletely_ConsumesOutputCorrectly()
        {
            var arr = new[] {1, 2, 3};
            var enums = arr.Tee(3);

            Assert.IsTrue(arr.SequenceEqual(enums[0]));

            var iter2 = enums[1].GetEnumerator();

            iter2.MoveNext();
            Assert.AreEqual(1, iter2.Current);

            iter2.MoveNext();
            Assert.AreEqual(2, iter2.Current);

            Assert.IsTrue(arr.SequenceEqual(enums[2]));

            iter2.MoveNext();
            Assert.AreEqual(3, iter2.Current);
        }
    }
}
