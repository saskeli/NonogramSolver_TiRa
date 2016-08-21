using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib
{
    public class Result
    {
        public readonly int Row;
        public readonly int Column;
        public readonly bool State;

        public Result(int row, int column, bool state)
        {
            Row = row;
            Column = column;
            State = state;
        }
    }
}
