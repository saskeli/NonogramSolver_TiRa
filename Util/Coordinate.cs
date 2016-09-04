using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public class Coordinate
    {
        public readonly int Column;
        public readonly int Row;

        public Coordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
