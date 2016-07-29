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
        private readonly Regex nl = new Regex(@"(?:\r(?:\n)|\n)");
        public Nonogram(string simple)
        {
            foreach (string s in nl.Split(simple))
            {
                
            }
        }
    }
}
