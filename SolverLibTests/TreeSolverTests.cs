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
        private readonly bool[][] _simpleSolved = {
            new[] { true, true, true, true },
            new[] { true, false, false, true},
            new[] { true, false, false, true},
            new[] { true, false, true, true},
            new[] { true, true, true, false}};
        private TreeSolver _ts;

        [TestInitialize()]
        public void Before()
        {
            _ts = new TreeSolver();
        }

        [TestMethod()]
        public void InitTest()
        {
            Assert.AreEqual(false, _ts.Solved());
            Assert.AreEqual(TimeSpan.Zero, _ts.BenchTime());
        }

        [TestMethod()]
        public void RunTest()
        {
            Nonogram ng = NonoGramFactory.ParseFromString(Simple);
            Assert.AreEqual(20, _ts.Run(ng), 
                "Unexpected amount of resolved tiles reported");
            Assert.IsTrue(CheckNonogram(_simpleSolved, ng), 
                "Unexpected solution.");
            Assert.IsTrue(_ts.Solved());
            Assert.AreNotEqual(TimeSpan.Zero.TotalMilliseconds, 
                _ts.BenchTime().TotalMilliseconds);
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