using Microsoft.VisualStudio.TestTools.UnitTesting;
using Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Tests
{
    [TestClass()]
    public class TileHeapTests
    {
        public TileHeap th;

        [TestInitialize]
        public void Before()
        {
            th = new TileHeap(10, 10);
        }

        [TestMethod()]
        public void ConstructorTest()
        {
            Assert.IsTrue(th.IsEmpty);
            Assert.AreEqual(0, th.Count);
        }

        [TestMethod()]
        public void AddRemoveTest()
        {
            th.Add(1, 1, 0);
            th.Add(1, 0, 1);
            th.Add(0, 0, 2);
            Assert.AreEqual(3, th.Count);
            Assert.IsFalse(th.IsEmpty);
            Assert.IsTrue(AreEqual(th.Peek(), 0, 0));
            Assert.IsTrue(AreEqual(th.Poll(), 0, 0));
            Assert.AreEqual(2, th.Count);
            Assert.IsTrue(AreEqual(th.Peek(), 1, 0));
            Assert.IsTrue(AreEqual(th.Poll(), 1, 0));
            Assert.AreEqual(1, th.Count);
            Assert.IsTrue(AreEqual(th.Peek(), 1, 1));
            Assert.IsTrue(AreEqual(th.Poll(), 1, 1));
            Assert.AreEqual(0, th.Count);
            Assert.IsTrue(th.IsEmpty);
        }

        private bool AreEqual(Coordinate coord, int row, int col)
        {
            return coord.Row == row && coord.Column == col;
        }
    }
}