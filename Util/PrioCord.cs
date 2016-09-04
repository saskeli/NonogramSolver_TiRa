using System;

namespace Util
{
    internal class PrioCord
    {
        public readonly int Row;
        public readonly int Column;
        public int Priority;

        public PrioCord(int row, int column, int priority)
        {
            Row = row;
            Column = column;
            Priority = priority;
        }
    }
}
