using Microsoft.VisualStudio.TestTools.UnitTesting;
using Util;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Tests
{
    [TestClass()]
    public class ListTests
    {
        [TestMethod()]
        public void ListInitTest()
        {
            List<int> li = new List<int>();
            Assert.AreEqual(0, li.Count);
            li = new List<int>(15);
            Assert.AreEqual(0, li.Count);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException),
            "Negative List capacity allowed")]
        public void ListNegativeInitTest()
        {
            new List<int>(-1);
        }

        [TestMethod()]
        public void IndexorTest()
        {
            List<int> li = new List<int>();
            li.Add(3);
            li.Add(5);
            Assert.AreEqual(3, li[0]);
            Assert.AreEqual(5, li[-1]);
        }

        [TestMethod()]
        public void ResizeFromZeroTest()
        {
            List<int> li = new List<int>(0);
            Assert.AreEqual(0, li.Count);
            li.Add(1);
            Assert.AreEqual(1, li.Count);
            Assert.AreEqual(1, li[0]);
        }

        [TestMethod()]
        public void ResizeTest()
        {
            List<bool> li = new List<bool>();
            for (int i = 0; i < 100; i++)
            {
                li.Add(i % 2 == 0);
            }
            Assert.AreEqual(100, li.Count);
        }

        [TestMethod()]
        [ExpectedException(typeof(OutOfMemoryException),
            "Something very strange happened. Created bigger array than possible")]
        public void ToioBigListTest()
        {
            List<int> li = new List<int>(Int32.MaxValue);
        }

        [TestMethod()]
        public void ToArrayTest()
        {
            List<int> li = new List<int>();
            li.Add(2);
            li.Add(123);
            Assert.AreEqual(2, li.Count);
            int[] arr = li.ToArray();
            Assert.AreEqual(2, arr.Length);
            Assert.AreEqual(2, arr[0]);
            Assert.AreEqual(123, arr[1]);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InvalidIndexTest()
        {
            List<int> li = new List<int>();
            int i = li[0];
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InvalidIndex2Test()
        {
            List<int> li = new List<int>();
            int i = li[-1];
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InvalidIndex3Test()
        {
            List<int> li = new List<int>();
            li.Add(2);
            li.Add(15);
            int i = li[2];
        }

        [TestMethod()]
        public void StackTest()
        {
            List<long> st = new List<long>();
            Assert.IsTrue(st.IsEmpty, "st.IsEmpty should return true but returns false");
            st.Push(8);
            Assert.IsFalse(st.IsEmpty, "st.IsEmpty should return false but returns true");
            Assert.AreEqual(1, st.Count, "st.Count should return 1 but returns " + st.Count);
            st.Push(12);
            Assert.IsFalse(st.IsEmpty, "st.IsEmpty should return false but returns true");
            Assert.AreEqual(2, st.Count, "st.Count should return 1 but returns " + st.Count);
            Assert.AreEqual(12l, st.Peek(), "st.Peek should return 12 but returns " + st.Peek());
            long r = st.Pop();
            Assert.AreEqual(12l, r, "st.Pop() should return 12 but returned " + r);
            Assert.AreEqual(1, st.Count, "st.Count should return 1 but returns " + st.Count);
            Assert.AreEqual(8l, st.Peek(), "st.Peek should return 12 but returns " + st.Peek());
        }

        [TestMethod()]
        public void QueueResizeTest()
        {
            List<int> q = new List<int>(10);
            for (int i = 0; i < 8; i++)
            {
                q.Enqueue(i);
            }
            for (int i = 0; i < 6; i++)
            {
                q.Dequeue();
            }
            for (int i = 0; i < 5; i++)
            {
                q.Enqueue(i);
            }
            Assert.AreEqual(7, q.Count);
            Assert.AreEqual(6, q.Poll());
        }

        [TestMethod()]
        public void QueueTest()
        {
            List<int> q = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                q.Enqueue(i);
            }
            Assert.AreEqual(8, q.Count);
            Assert.IsFalse(q.IsEmpty);
            for (int i = 0; i < 8; i++)
            {
                Assert.AreEqual(i, q.Poll());
                Assert.AreEqual(i, q.Dequeue());
            }
            Assert.IsTrue(q.IsEmpty);
        }
    }
}