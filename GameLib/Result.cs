namespace GameLib
{
    /// <summary>
    /// Data class for storing solver results
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Row index of result
        /// </summary>
        public readonly int Row;
        /// <summary>
        /// Column index of result
        /// </summary>
        public readonly int Column;
        /// <summary>
        /// State of the tile (block == true/space == false)
        /// </summary>
        public readonly bool State;

        /// <summary>
        /// Result constructor
        /// </summary>
        /// <param name="row">Row index of result</param>
        /// <param name="column">Column index of result</param>
        /// <param name="state">State of the tile (block == true/space == false)</param>
        public Result(int row, int column, bool state)
        {
            Row = row;
            Column = column;
            State = state;
        }
    }
}
