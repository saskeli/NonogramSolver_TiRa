namespace Util
{
    /// <summary>
    /// Cooridnate class with priority for use with TileHeaps
    /// </summary>
    internal class PrioCord
    {
        /// <summary>
        /// Row index
        /// </summary>
        public readonly int Row;
        /// <summary>
        /// Column index
        /// </summary>
        public readonly int Column;
        /// <summary>
        /// priority
        /// </summary>
        public int Priority;

        public PrioCord(int row, int column, int priority)
        {
            Row = row;
            Column = column;
            Priority = priority;
        }
    }
}
