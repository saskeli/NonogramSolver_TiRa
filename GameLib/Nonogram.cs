using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameLib
{
    public class Nonogram
    {
        private TileData tiles;
        private NumberData numbers;
        public Nonogram(int[][] colums, int[][] rows)
        {
            tiles = new TileData(colums.Length, rows.Length);
            numbers = new NumberData(colums, rows);
        }
    }
}
