using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolverLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLib;

namespace SolverLib.Tests
{
    [TestClass()]
    public class TreeSolverTests
    {
        private const string Simple = "4,5\r\n4\r\n1,1\r\n1,1\r\n1,2\r\n3\r\n5\r\n1,1\r\n1,2\r\n4\r\n";
        private readonly bool[][] _simpleSolved = new bool[][] {
            new bool[] { true, true, true, true },
            new bool[] { true, false, false, true},
            new bool[] { true, false, false, true},
            new bool[] { true, false, true, true},
            new bool[] { true, true, true, false}};
        private TreeSolver ts;

        [TestInitialize()]
        public void Before()
        {
            ts = new TreeSolver();
        }

        [TestMethod()]
        public void InitTest()
        {
            Assert.AreEqual(false, ts.Solved());
            Assert.AreEqual(TimeSpan.Zero, ts.BenchTime());
        }

        [TestMethod()]
        public void RunTest()
        {
            Nonogram ng = NonoGramFactory.ParseFromString(Simple);
            Assert.AreEqual(20, ts.Run(ng), 
                "Unexpected amount of resolved tiles reported");
            Assert.IsTrue(CheckNonogram(_simpleSolved, ng), 
                "Unexpected solution.");
            Assert.IsTrue(ts.Solved());
            Assert.AreNotEqual(TimeSpan.Zero.TotalMilliseconds, 
                ts.BenchTime().TotalMilliseconds);
        }

        private bool CheckNonogram(bool[][] jArr, Nonogram ng)
        {
            for (int i = 0; i < jArr.Length; i++)
            {
                for (int j = 0; j < jArr[0].Length; j++)
                {
                    if (!ng.Resolved(i, j) || jArr[i][j] != ng.IsTrue(i, j))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}