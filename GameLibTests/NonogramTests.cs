using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Tests
{
    [TestClass()]
    public class NonogramTests
    {
        private const string Simple = "4,5\r\n4\r\n1,1\r\n1,1\r\n1,2\r\n3\r\n5\r\n1,1\r\n1,2\r\n4\r\n";
        private Nonogram ng;

        [TestInitialize()]
        public void Before()
        {
            ng = NonoGramFactory.ParseFromString(Simple);
        }


        [TestMethod()]
        public void InitTest()
        {
            Assert.AreEqual(4, ng.Width);
            Assert.AreEqual(5, ng.Height);
            Assert.AreEqual(20, ng.LeftToClear);
        }
        
        [TestMethod()]
        public void ResolvedTest()
        {
            Assert.AreEqual(false, ng.Resolved(0, 0));
        }

        [TestMethod()]
        public void SetAndClearTest()
        {
            ng.Set(4, 3, true);
            Assert.AreEqual(true, ng.IsTrue(4, 3));
            Assert.AreEqual(true, ng.Resolved(4, 3));
            Assert.AreEqual(19, ng.LeftToClear);
            ng.Set(4, 3, false);
            Assert.AreEqual(false, ng.IsTrue(4, 3));
            Assert.AreEqual(true, ng.Resolved(4, 3));
            Assert.AreEqual(19, ng.LeftToClear);
            ng.Clear(4, 3);
            Assert.AreEqual(false, ng.IsTrue(4, 3));
            Assert.AreEqual(false, ng.Resolved(4, 3));
            Assert.AreEqual(20, ng.LeftToClear);
        }
        
        [TestMethod()]
        public void GetRowNumTest()
        {
            Assert.AreEqual(4, ng.GetRowNum(0, 0));
            Assert.AreEqual(0, ng.GetRowNum(0, 1));
            Assert.AreEqual(1, ng.GetRowNum(1, 1));
        }

        [TestMethod()]
        public void GetColumnNumTest()
        {
            Assert.AreEqual(5, ng.GetColumnNum(0, 0));
            Assert.AreEqual(0, ng.GetColumnNum(0, 1));
            Assert.AreEqual(2, ng.GetColumnNum(2, 1));
        }

        [TestMethod()]
        public void RowSumTest()
        {
            Assert.AreEqual(4, ng.RowSum(0));
            Assert.AreEqual(3, ng.RowSum(3));
        }

        [TestMethod()]
        public void ColumnSumTest()
        {
            Assert.AreEqual(5, ng.ColumnSum(0));
            Assert.AreEqual(3, ng.ColumnSum(2));
        }

    }
}