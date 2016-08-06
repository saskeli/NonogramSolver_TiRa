namespace GameLib
{
    internal class Number
    {
        public int Value { get; }

        public Number(int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}