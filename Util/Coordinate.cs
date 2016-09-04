namespace Util
{
    /// <summary>
    /// Data class for storing integer coordinates
    /// </summary>
    public class Coordinate
    {
        /// <summary>
        /// Column index
        /// </summary>
        public readonly int Column;
        /// <summary>
        /// Row index
        /// </summary>
        public readonly int Row;

        /// <summary>
        /// Coordinate constructor
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="column">Column index</param>
        public Coordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
